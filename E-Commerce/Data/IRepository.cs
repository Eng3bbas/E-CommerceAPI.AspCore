using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E_Commerce.Data.Entities;
using E_Commerce.Helpers.Pagination;
using Microsoft.EntityFrameworkCore.Query;

namespace E_Commerce.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<List<TEntity>> GetAll();
        public Task<TEntity> Create(TEntity entity);
        public Task Create(IEnumerable<TEntity> entities);
        
        public Task<TEntity> Find(Guid id);
        public Task<TEntity> Update(TEntity entity);
        public Task Delete(TEntity entity);
        public Task Delete(IEnumerable<TEntity> entities);
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        public IQueryable<TEntity> Include(Expression<Func<TEntity,object>> relation);
        public IQueryable<TEntity> Include(string relation);
        public Task Save();
        public Task<PaginationList<TEntity>> Paginate(PaginationOptions options);
        public IQueryable<TEntity> OrderBy(Func<TEntity, object> order, bool ascOrder = true);
    }
}