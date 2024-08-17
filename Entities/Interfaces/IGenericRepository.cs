using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> AddAsync(T Model);
        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> models);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllIncludeAsync(params Expression<Func<T, object>>[] includes);
        public Task<T>? GetByIDAsync(int id);
        public Task<T>? GetByIDAsync(string id);
        public Task<T?> FindFirst(Expression<Func<T, bool>> predicate);
        public Task<T>? GetByNameAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>>? Filter(Expression<Func<T, bool>> expression);
        public Task<T> UpdateAsync(T Model, int id);
        public void Delete(int id);
        public Task<T> IsExistAsync(Expression<Func<T, bool>> expression);
    }
}
