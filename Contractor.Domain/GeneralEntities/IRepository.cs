using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.GeneralEntities
{
    public interface IRepository<T,TKey> where T : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> GetByIdAsync(TKey id);
        
        Task AddAsync(T entity);
        
        Task<bool> SaveChangesAsync();
        
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetListByCondition(Expression<Func<T, bool>> predicate);
    }
}
