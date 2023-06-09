﻿using AutoMapper;
using KUSYS_Demo.Application.Dtos.Course;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MappingStudent();
            MappingCourse();
        }

        public void MappingStudent()
        {
            CreateMap<Student, StudentSimpleDto>().ForMember(dest=>dest.Courses,opt=>opt.MapFrom(src=>src.Courses));
            CreateMap<Student, StudentWithDetailsDto>().ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));
            CreateMap<CreateUpdateStudentDto, Student>();
            //CreateMap<UpdateStudentDto, Student>();
            CreateMap<StudentDto, Student>().ReverseMap();
            CreateMap<StudentDto, CreateUpdateStudentDto>().ReverseMap();

            CreateMap<StudentCourse, StudentCourseDto>()
                .ForMember(d => d.CourseId, d => d.MapFrom(s => s.CourseId))
                .ForMember(d => d.CourseName, d => d.MapFrom(s => $"{s.Course.CourseCode} - {s.Course.CourseName}"));
        }

        public void MappingCourse()
        {
            CreateMap<Course,CourseDto>()
                .ForMember(d => d.CourseId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CourseName, opt => opt.MapFrom(s => $"{s.CourseCode} - {s.CourseName}"));
        }
    }
}
