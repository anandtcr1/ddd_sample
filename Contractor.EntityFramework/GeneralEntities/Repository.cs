using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Lookups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.GeneralEntities
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _entities;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id.Equals(id)) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(T), id.ToString());
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetListByCondition(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }
    }
}
