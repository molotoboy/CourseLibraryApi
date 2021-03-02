using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollection")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;

        public AuthorCollectionController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})")]
        public IActionResult GetAuthorCollection(
            [FromRoute] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var authorEntities = _repo.GetAuthors(ids);
            if (ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }
            var authorToReturn = _mapper.Map<IEnumerable<AuthorGetDto>>(authorEntities);
            return Ok(authorToReturn);

        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorGetDto>> CreateAuthorCollection(IEnumerable<AuthorCreateDto> authorCollection)
        {
            var authorEntities = _mapper.Map<IEnumerable<Entities.Author>>(authorCollection);
            foreach (var author in authorEntities)
            {
                _repo.AddAuthor(author);
            }
            _repo.Save();
            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorGetDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
            return CreatedAtAction(nameof(GetAuthorCollection),
                new { ids = idsAsString },
                authorCollectionToReturn);
        }
    }
}
