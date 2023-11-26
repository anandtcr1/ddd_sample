using Contractor.Tools;

namespace Contractor.Subscriptions
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> CreateAsync(Subscription subscription);
        
        Task<Subscription> GetAsync(int id);

        Task<Subscription> GetDefaultAsync();

        Task<ListRepositoryModel<Subscription>> GetAllAsync(string? name, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
