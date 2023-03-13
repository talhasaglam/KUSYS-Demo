using AutoMapper;
using KUSYS_Demo.Application.Dtos.Course;
using KUSYS_Demo.Application.Services.Interfaces;
using KUSYS_Demo.DataAccess.Repositories.İnterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }
        public async Task<List<CourseDto>> GetListAsync()
        {
            var courses = await _courseRepository.GetListAsync();
            return _mapper.Map<List<CourseDto>>(courses);
        }
    }
}
