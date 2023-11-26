using AutoMapper;

namespace Contractor.Identities
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleViewModel, RoleDto>();
            CreateMap<RoleClaim, RoleClaimDto>();
        }
    }
}
