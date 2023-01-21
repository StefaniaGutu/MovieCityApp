using MovieCity.Common;
using MovieCity.WebApp;
using System.Linq;


namespace MovieCity.DataAccess
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly MovieAppDBContext Context;

        public BaseRepository(MovieAppDBContext context)
        {
            this.Context = context;
        }

        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public TEntity Insert(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
        
        public void DeleteRange(List<TEntity> entity)
        {
            Context.Set<TEntity>().RemoveRange(entity);
        }
    }
}
