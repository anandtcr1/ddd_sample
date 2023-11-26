using AutoMapper;
using Contractor.Tenders;

namespace Contractor.Profiles.Tenders
{
    public class TenderProfile : Profile
    {
        public TenderProfile()
        {
            CreateMap<TenderViewModel, TenderDto>();
            CreateMap<Tender, TenderDto>();
            CreateMap<Tender, ContractorTenderDto>();
            CreateMap<TenderInvitation, TenderInvitationDto>();
            CreateMap<TenderInvitation, TenderUserInvitationsDto>()
                .ForMember(dest => dest.ContractorName, opt => opt.MapFrom(src => src.Contractor == null ? "" : src.Contractor.DisplayName));
            CreateMap<TenderAccessDefinition, TenderAccessDefinitionDto>();
            CreateMap<InvitationAccessDefinition, InvitationAccessDefinitionDto>();
        }
    }
}
