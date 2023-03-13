using KUSYS_Demo.DataAccess.Repositories.Interfaces;
using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace KUSYS_Demo.DataAccess.Repositories
{
    public class EfCoreRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        protected TContext Context { get; }
        public EfCoreRepository(TContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, CancellationToken cancellationToken = default)
        {
            var query = GetQueryable();
            if (include != null) query = include(query);
            return await query.Where(predicate).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, CancellationToken cancellationToken = default)
        {
            var query = GetQueryable();
            if (include != null) query = include(query);
            return await query.Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>
                                                           include = null, CancellationToken cancellationToken = default)
        {
            var query = GetQueryable();
            if (include != null) query = include(query);
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default)
        {
            var query = GetQueryable();
            return await query.CountAsync(predicate, cancellation);
        }

        public async Task<int> CountAsync(CancellationToken cancellation = default)
        {
            var query = GetQueryable();
            return await query.CountAsync(cancellation);
        }

        public async Task<List<TEntity>> GetPagedListAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
        {
            return await orderBy(GetQueryable()).Skip(skipCount).Take(maxResultCount).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = (await GetDbSet().AddAsync(entity, cancellationToken)).Entity;

            if(autoSave)
                await Context.SaveChangesAsync(cancellationToken);

            return savedEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            GetDbSet().Attach(entity);

            var updatedEntity = Context.Update(entity).Entity;

            if(autoSave)
                await Context.SaveChangesAsync(cancellationToken);

            return updatedEntity;
        }

        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            GetDbSet().Remove(entity);

            if(autoSave)
                await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public DbSet<TEntity> GetDbSet()
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return GetDbSet().AsQueryable();
        }
    }
}
