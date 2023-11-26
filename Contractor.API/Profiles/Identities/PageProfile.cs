using AutoMapper;

namespace Contractor.Identities
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Page, PageDto>();
        }
    }
}
