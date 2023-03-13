using KUSYS_Demo.DataAccess.Contexts;
using KUSYS_Demo.DataAccess.Repositories.Interfaces;
using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KUSYS_Demo.DataAccess.Repositories
{
    public class StudentRepository : EfCoreRepository<Student,BaseDbContext>, IStudentRepository
    {
        public StudentRepository(BaseDbContext context): base(context)
        {
            
        }

        //For Paginitaion and filtering
        public async Task<(int totalCount, int filteredCount)> CountAsync(string filterText, int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
        {
            var quaryable = GetQueryable();
            var totalCount = await quaryable.CountAsync(cancellationToken);

            if (!string.IsNullOrEmpty(filterText))
            {
                //WhereIf eklenmeli.

                quaryable = quaryable.Where(s => s.FirstName.ToUpper().Contains(filterText)
                                             || s.LastName.ToUpper().Contains(filterText));
            }


            var filteredCount = await quaryable.Skip(skipCount).Take(maxResultCount).AsNoTracking().CountAsync(cancellationToken);

            return (totalCount, filteredCount);
        }

        //For Paginitaion and filtering
        public async Task<List<Student>> GetPagedListAsync(string filterText, int skipCount, int maxResultCount, Func<IQueryable<Student>, IIncludableQueryable<Student, object>>
                                                           include = null, CancellationToken cancellationToken = default)
        {
            var quaryable = GetQueryable();
            if (!string.IsNullOrEmpty(filterText))
            {
                //WhereIf eklenmeli.

                quaryable = quaryable.Where(s => s.FirstName.ToUpper().Contains(filterText)
                                             || s.LastName.ToUpper().Contains(filterText));
            }

            if(include != null) quaryable = include(quaryable);

            return await quaryable.Skip(skipCount).Take(maxResultCount).AsNoTracking().ToListAsync(cancellationToken);

        }
    }
}
