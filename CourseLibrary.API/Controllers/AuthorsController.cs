using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;

        public AuthorsController(ICourseLibraryRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authors = _repo.GetAuthors();
            return Ok(authors);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetAuthors(Guid id)
        {
            var author = _repo.GetAuthor(id);
            if (author==null)
            {
                return NotFound();
            }
            return Ok(author);
        }
    }
}
