using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Subscriptions
{
    public interface ISubscriptionAppService
    {
        Task<SubscriptionDto> CreateAsync(SubscriptionDto subscription);
        
        Task<SubscriptionDto> UpdateAsync(SubscriptionDto subscription);

        Task<SubscriptionDto> GetAsync(int id);

        Task<ListServiceModel<SubscriptionDto>> GetAllAsync(string? name, int pageNumber, int pageSize);
    }
}
