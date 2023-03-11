using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KUSYS_Demo.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TEntity> : IQuery<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);

        Task<int> CountAsync(CancellationToken cancellation = default);

        Task<List<TEntity>> GetPagedListAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int skipCount, int maxResultCount, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> GetDbSet();

    }
}
