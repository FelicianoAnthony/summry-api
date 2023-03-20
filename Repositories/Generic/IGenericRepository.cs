using System.Linq.Expressions;

namespace StarterApi.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> Add(T entity);

        Task<bool> AddMany(List<T> entity);

        bool Delete(T entity);

        bool DeleteMany(ICollection<T> entity);

        Task<IEnumerable<T>> FindManyWithRelated(Expression<Func<T, bool>> predicate, List<string> relatedTables);

        Task<T> FindOneWithRelated(Expression<Func<T, bool>> predicate, List<string> relatedTables);

        bool Update(T entity);

    }
}
