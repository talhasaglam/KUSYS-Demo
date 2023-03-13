using KUSYS_Demo.Application.Dtos.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetListAsync();
    }
}
