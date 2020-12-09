using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E_Commerce.Data.Entities;
using E_Commerce.Extensions;
using E_Commerce.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace E_Commerce.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _entities;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            return (await _entities.AddAsync(entity)).Entity;
        }
        public async Task Create(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }
        
        public async Task<TEntity> Find(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            return await Task.FromResult(_entities.Update(entity).Entity);
        }

        public async Task Delete(TEntity entity)
        {
            await Task.Run(() => _entities.Remove(entity));
        }

        public async Task Delete(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _entities.RemoveRange(_entities));
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>> relation)
        {
            return _entities.Include(relation);
        }

        public IQueryable<TEntity> Include(string relation)
        {
            return _entities.Include(relation);
        }


        public async Task Save()
        {
           await _dbContext.SaveChangesAsync();
        }

        public async Task<PaginationList<TEntity>> Paginate(PaginationOptions options)
        {
            return await _entities.Paginate(options);
        }

        public IQueryable<TEntity> OrderBy(Func<TEntity, object> order, bool ascOrder = true)
        {
            return (ascOrder ? _entities.OrderBy(order) : _entities.OrderByDescending(order)).AsQueryable();
        }
        
    }
}