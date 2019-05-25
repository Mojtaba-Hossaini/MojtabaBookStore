using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models.Repository
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity> where TEntity: class where TContext : DbContext
    {
        private readonly TContext context;
        private DbSet<TEntity> dbSet;

        public BaseRepository(TContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync() => await dbSet.AsNoTracking().ToListAsync();

        public IEnumerable<TEntity> FindAll() => dbSet.AsNoTracking().ToList();


        public async Task<TEntity> FindByID(int? id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity,bool>> filter = null,Func<IQueryable<TEntity>,IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity,object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;
            

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            if (filter != null)
                query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task Create(TEntity entity) => await dbSet.AddAsync(entity);

        public void Update(TEntity entity) => dbSet.Update(entity);

        public void Delete(TEntity entity) => dbSet.Remove(entity);

        public async Task CreateRange(IEnumerable<TEntity> entities) => await dbSet.AddRangeAsync(entities);

        public void UpdateRange(IEnumerable<TEntity> entities) => dbSet.UpdateRange(entities);

        public void DeleteRange(IEnumerable<TEntity> entities) => dbSet.RemoveRange(entities);

        public async Task<List<TEntity>> GetPaginateResultAsync(int currentPage = 1, int pageSize = 5)
        {
            var entities = await FindAllAsync();
            return entities.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetCount() => dbSet.Count();
    }
}
