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

    public class CoursesController:ControllerBase
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
            if (course==null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseGetDto>(course));
        }
    }
}
