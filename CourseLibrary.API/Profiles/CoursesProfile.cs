﻿using AutoMapper;

namespace CourseLibrary.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entities.Course, Models.CourseGetDto>();
            CreateMap<Models.CourseCreateDto, Entities.Course>();
            CreateMap<Models.CourseUpdateDto, Entities.Course>();
        }
    }
}
