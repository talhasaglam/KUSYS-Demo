using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Pagination;

namespace KUSYS_Demo.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentSimpleDto>> GetListAsync();

        Task<PagedResultDto<StudentSimpleDto>> GetPagedListAsync(GetPagePropDto getPagePropDto);

        Task<StudentDto> GetAsync(int id);

        Task UpdateAsync(StudentDto updateStudentDto,int id);

        Task CreateAsync(StudentDto createStudentDto);

        Task DeleteAsync(int id);

        Task<StudentWithDetailsDto> GetWithDetailsAsync(int id);

        Task SetCoruseAsync(int sutdentId,int courseId);
    }
}
