using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Subscriptions
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DatabaseContext _databaseContext;

        public SubscriptionRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Subscription> CreateAsync(Subscription subscription)
        {
            await _databaseContext.Subscriptions.AddAsync(subscription);

            return subscription;
        }

        public async Task<ListRepositoryModel<Subscription>> GetAllAsync(string? name, int pageNumber, int pageSize)
        {
            name = !string.IsNullOrEmpty(name) ? name.ToLower() : null;

            var query = _databaseContext.Subscriptions
                .Where(x =>
                        (string.IsNullOrEmpty(name) || x.Name.Contains(name))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new ListRepositoryModel<Subscription>(totalRecords, list);
        }

        public async Task<Subscription> GetAsync(int id)
        {
            return await _databaseContext.Subscriptions.Where(x => x.Id == id).Include(x => x.ProjectFolderTemplates).FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Subscription), id);
        }

        public async Task<Subscription> GetDefaultAsync()
        {
            return await _databaseContext.Subscriptions.Include(x => x.ProjectFolderTemplates).FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Subscription), "Default");
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
