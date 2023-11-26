using AutoMapper;

namespace Contractor.Subscriptions
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<SubscriptionViewModel, SubscriptionDto>();
            CreateMap<Subscription, SubscriptionDto>();
        }
    }
}
