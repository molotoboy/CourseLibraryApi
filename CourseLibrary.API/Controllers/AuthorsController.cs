using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<AuthorGetDto> GetAuthors([FromQuery] AuthorResourceParameters authorResourceParameters)
        {
            var authors = _repo.GetAuthors(authorResourceParameters);

            return Ok(_mapper.Map<IEnumerable<AuthorGetDto>>(authors));
        }

        [HttpGet("{authorId:guid}", Name = "GetAuthor")]
        [HttpHead("{authorId:guid}")]
        public ActionResult<AuthorGetDto> GetAuthors(Guid authorId)
        {
            var author = _repo.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AuthorGetDto>(author));
        }

        [HttpPost]
        public ActionResult<AuthorGetDto> CreateAuthor(AuthorCreateDto author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _repo.AddAuthor(authorEntity);
            _repo.Save();
            var authorToReturn = _mapper.Map<AuthorGetDto>(authorEntity);
            return CreatedAtAction(nameof(GetAuthors),
                new { authorId = authorToReturn.Id },
                authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS.POST");
            return Ok();
        }
    }
}
