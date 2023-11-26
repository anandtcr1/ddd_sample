using AutoMapper;
using Contractor.Tools;

namespace Contractor.Subscriptions
{
    public class SubscriptionAppService : ISubscriptionAppService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public SubscriptionAppService(ISubscriptionRepository subscriptionRepository, 
            IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }


        public async Task<SubscriptionDto> CreateAsync(SubscriptionDto subscriptionDto)
        {
            var subscription = Subscription.Create(subscriptionDto.Name, subscriptionDto.StorageSpace);

            if(subscriptionDto.ProjectFolderTemplates  != null)
            {
                foreach (var projectFolderTemplates in subscriptionDto.ProjectFolderTemplates)
                {
                    subscription.AddProjectFolderTemplate(projectFolderTemplates.Name);
                }
            }

            await _subscriptionRepository.CreateAsync(subscription);

            await _subscriptionRepository.SaveChangesAsync();

            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<ListServiceModel<SubscriptionDto>> GetAllAsync(string? name, int pageNumber, int pageSize)
        {
            var list = await _subscriptionRepository.GetAllAsync(name, pageNumber, pageSize);

            return new ListServiceModel<SubscriptionDto>(list.TotalCount, _mapper.Map<List<SubscriptionDto>>(list.List));
        }

        public async Task<SubscriptionDto> GetAsync(int id)
        {
            var subscription = await _subscriptionRepository.GetAsync(id);

            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<SubscriptionDto> UpdateAsync(SubscriptionDto subscriptionDto)
        {
            var subscription = await _subscriptionRepository.GetAsync(subscriptionDto.Id);

            subscription.Update(subscriptionDto.Name, subscriptionDto.StorageSpace);

            await _subscriptionRepository.SaveChangesAsync();

            return _mapper.Map<SubscriptionDto>(subscription);
        }
    }
}
