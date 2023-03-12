using KUSYS_Demo.Application.DataTables;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Pagination;
using KUSYS_Demo.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Services
{
    public class StudentService : IStudentService
    {
        public Task<StudentDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentDto>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<StudentSimpleDto>> GetPagedListAsync(DataTablesRequest dataTablesRequest)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateStudentDto updateStudentDto)
        {
            throw new NotImplementedException();
        }
    }
}
