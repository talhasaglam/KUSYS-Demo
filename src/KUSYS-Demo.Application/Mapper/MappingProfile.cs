using AutoMapper;
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
        }

        public void MappingStudent()
        {
            CreateMap<Student, StudentSimpleDto>();
            CreateMap<CreateUpdateStudentDto, Student>();
            //CreateMap<UpdateStudentDto, Student>();
            CreateMap<StudentDto, Student>().ReverseMap();
            CreateMap<StudentDto, CreateUpdateStudentDto>().ReverseMap();
        }
    }
}
