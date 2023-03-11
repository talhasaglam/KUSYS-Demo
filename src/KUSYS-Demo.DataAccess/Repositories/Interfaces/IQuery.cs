
namespace KUSYS_Demo.DataAccess.Repositories.Interfaces
{
    public interface IQuery<TEntity>
    {
        IQueryable<TEntity> GetQueryable();
    }
}
