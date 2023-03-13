using AutoMapper;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.WebApp.Pages.Students;

namespace KUSYS_Demo.WebApp.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUpdateStudentViewModal, StudentDto>().ReverseMap();
        }
    }
}
