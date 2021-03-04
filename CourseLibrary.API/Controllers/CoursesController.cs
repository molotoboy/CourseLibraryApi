using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]

    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseGetDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courses = _repo.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CourseGetDto>>(courses));
        }

        [HttpGet("{courseId}")]
        public ActionResult<IEnumerable<CourseGetDto>> GetCoursesForAuthor(Guid authorId, Guid courseId)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var course = _repo.GetCourse(authorId, courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseGetDto>(course));
        }

        [HttpPost]
        public ActionResult<CourseGetDto> CreateCourseForAuthor(Guid authorId, CourseCreateDto course)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = _mapper.Map<Entities.Course>(course);
            _repo.AddCourse(authorId, courseEntity);
            _repo.Save();

            var courseToReturn = _mapper.Map<CourseGetDto>(courseEntity);
            return CreatedAtAction(nameof(GetCoursesForAuthor),
                new { authorId = authorId, courseId = courseToReturn.Id },
                courseToReturn);
        }
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId,
            Guid courseId,
            CourseUpdateDto course)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseFromRepo = _repo.GetCourse(authorId, courseId);
            if (courseFromRepo == null)
            {
                var courseToAdd = _mapper.Map<Entities.Course>(course);
                courseToAdd.Id = courseId;
                _repo.AddCourse(authorId, courseToAdd);
                _repo.Save();
                var courseToReturn = _mapper.Map<CourseGetDto>(courseToAdd);
                return CreatedAtAction(nameof(GetCoursesForAuthor), new { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }
            _mapper.Map(course, courseFromRepo);
            _repo.UpdateCourse(courseFromRepo);
            _repo.Save();
            return Ok(); // NoContent(); // Ok();
        }
    }
}
