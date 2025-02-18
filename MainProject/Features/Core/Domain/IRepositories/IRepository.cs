﻿
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Features.Core.Domain.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetOneAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
