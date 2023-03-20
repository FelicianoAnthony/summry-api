using LbAutomationPortalApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StarterApi.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected StarterApiContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(StarterApiContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }


        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> AddMany(List<T> entity)
        { 
            await dbSet.AddRangeAsync(entity);
            return true;
        }

        public virtual bool Delete(T entity)
        { 
            dbSet.Remove(entity);
            return true;
        }

        public bool DeleteMany(ICollection<T> entity)
        {
            dbSet.RemoveRange(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> FindManyWithRelated(Expression<Func<T, bool>> predicate, List<string> relatedTables)
        {
            IQueryable<T> query = dbSet.AsQueryable();
            relatedTables.ForEach(table => query = query.Include(table));
            return await query.Where(predicate).ToListAsync();   
        }


        public virtual async Task<T> FindOneWithRelated(Expression<Func<T, bool>> predicate, List<string> relatedTables)
        {
            IQueryable<T> query = dbSet.AsQueryable();
            relatedTables.ForEach(table => query = query.Include(table));
            return await query.Where(predicate).FirstOrDefaultAsync();
        }


        public virtual bool Update(T entity)
        {
            dbSet.Update(entity);
            return true;
        }
    }
}
