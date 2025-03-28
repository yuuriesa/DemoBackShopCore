using DemoBackShopCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSetEntity;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSetEntity = _dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}