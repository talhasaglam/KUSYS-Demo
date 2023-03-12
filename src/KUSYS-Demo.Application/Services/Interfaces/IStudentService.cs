using KUSYS_Demo.Application.DataTables;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Pagination;
using KUSYS_Demo.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentSimpleDto>> GetListAsync();

        Task<PagedResultDto<StudentSimpleDto>> GetPagedListAsync(DataTablesRequest dataTablesRequest);

        Task<StudentDto> GetAsync(int id);

        Task UpdateAsync(CreateUpdateStudentDto updateStudentDto);

        Task CreateAsync(CreateUpdateStudentDto createStudentDto);
    }
}
