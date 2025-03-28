using DemoBackShopCore.Data;
using DemoBackShopCore.Utils;
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

        public void Add(TEntity entity)
        {
            _dbSetEntity.Add(entity: entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSetEntity.AddRange(entities: entities);
        }

        public IQueryable<TEntity> GetAll(PaginationFilter paginationFilter)
        {
            return _dbSetEntity
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize);
        }

        public TEntity GetById(int id)
        {
            return _dbSetEntity.Find(keyValues: id);
        }

        public void Remove(int id)
        {
            _dbSetEntity.Remove(entity: _dbSetEntity.Find(keyValues: id));
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Update(int id, TEntity entity)
        {
            _dbSetEntity.Update(entity: entity);
        }
    }
}