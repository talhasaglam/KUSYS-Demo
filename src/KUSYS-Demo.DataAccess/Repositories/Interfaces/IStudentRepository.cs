using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace KUSYS_Demo.DataAccess.Repositories.Interfaces
{
    public interface IStudentRepository: IRepository<Student>
    {
        Task<List<Student>> GetPagedListAsync(string filterText, int skipCount, int maxResultCount, Func<IQueryable<Student>, IIncludableQueryable<Student, object>>
                                                           include = null, CancellationToken cancellationToken = default);

        Task<(int totalCount, int filteredCount)> CountAsync(string filterText, int skipCount, int maxResultCount, CancellationToken cancellationToken = default);
    }
}
