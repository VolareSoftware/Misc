using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly EntityFrameworkContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ICurrentUser _currentUser;

        public Repository(EntityFrameworkContext context, ICurrentUser currentUser)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _currentUser = currentUser;
        }

        public IQueryable<TEntity> All()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = All();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TEntity Find(int id)
        {
            return _dbSet.Find(id);
        }

        public void InsertOrUpdate(TEntity entity)
        {
            var isTrackedEntity = entity is TrackedEntity;

            if (entity.IsExisting)
            {
                // Update
                if (isTrackedEntity)
                {
                    //Check if entity was updated
                    var updated = _context.Entry(entity).State == EntityState.Modified;

                    //Track user/date if so
                    if (updated)
                    {
                        var trackedEntity = entity as TrackedEntity;
                        trackedEntity.UpdatedBy = _currentUser.FullName;
                        trackedEntity.UpdatedDate = DateTime.UtcNow;
                    }
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                // Insert
                if (isTrackedEntity)
                {
                    var trackedEntity = entity as TrackedEntity;
                    trackedEntity.CreatedBy = _currentUser.FullName;
                    trackedEntity.CreatedDate = DateTime.UtcNow;
                    trackedEntity.UpdatedBy = _currentUser.FullName;
                    trackedEntity.UpdatedDate = DateTime.UtcNow;
                }
                _dbSet.Add(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }
    }
}