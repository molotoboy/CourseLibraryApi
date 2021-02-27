using CourseLibrary.API.Helpers;
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
            var authorsFromRepo = _repo.GetAuthors();
            var authors = new List<AuthorDto>();

            foreach (var author in authorsFromRepo)
            {
                authors.Add(new AuthorDto()
                {
                    Id=author.Id,
                    Name=$"{author.FirstName} ${author.LastName}",
                    MainCategory=author.MainCategory,
                    Age=author.DateOfBirth.GetCurrentAge()
                });
            }

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
