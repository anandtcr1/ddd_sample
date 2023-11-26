using AutoMapper;

namespace Contractor.Correspondences
{
    public class CorrespondenceProfile:Profile
    {
        public CorrespondenceProfile()
        {
            CreateMap<IncomeTypeViewModel, IncomeTypeDto>();
            CreateMap<IncomeType, IncomeTypeDto>();

            CreateMap<OutGoingTypeViewModel, OutGoingTypeDto>();
            CreateMap<OutGoingType, OutGoingTypeDto>();

            CreateMap<CorrespondenceViewModel, CorrespondenceDto>();
            CreateMap<Correspondence, CorrespondenceDto>();

            CreateMap<CorrespondenceAccessDefinition, CorrespondenceAccessDefinitionDto>();

            CreateMap<CorrespondenceRecipient, CorrespondenceRecipientDto>()
                .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient != null ? src.Recipient.Email : ""));
            CreateMap<Correspondence, GetCorrespondenceForListDto>()
                .ForMember(dest=>dest.SenderName,opt=>opt.MapFrom(src=>GetSenderName(src)));
        }

        public static string GetSenderName(Correspondence correspondence)
        {
            if(correspondence.CorrespondenceRecipients ==  null)
            {
                return "";
            }

            var sender = correspondence.CorrespondenceRecipients.FirstOrDefault(x => x.RecipientType == CorrespondenceRecipientType.Sender);

            if(sender == null || sender.Recipient == null)
            {
                return "";
            }

            return sender.Recipient.DisplayName;
        }
    }
}
