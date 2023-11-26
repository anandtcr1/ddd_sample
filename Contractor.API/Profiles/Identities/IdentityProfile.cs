using AutoMapper;

namespace Contractor.Identities
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<UserProfileViewModel, UserProfileDto>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());

            CreateMap<ProfileAccessDefinition, ProfileAccessDefinitionDto>();

            CreateMap<UserProfile, UserProfileDto>();

            CreateMap<User, UserListDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => GetRoles(src)));
            
            CreateMap<User, SubUserListDto>();
            CreateMap<UserClaim, UserClaimDto>();

            CreateMap<User, UserDto>();
            CreateMap<SubUserCreateViewModel, SubUserDto>();
            CreateMap<User, SubUserDto>();
            CreateMap<UserCreateViewModel, UserDto>();
            CreateMap<UserUpdateViewModel, UserDto>();
            CreateMap<SubUserUpdateViewModel, SubUserDto>();
        }

        private List<string> GetRoles(User user)
        {
            var roles = new List<string>();

            if(user.Roles != null)
            {
                var userRole = user.Roles.Select(x => x.Role);

                if(userRole.Any())
                {
                    return userRole.Select(x=>x.Name).ToList();
                }
            }



            return roles;
        }
    }
}
