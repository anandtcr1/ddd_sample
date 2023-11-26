using AutoMapper;

namespace Contractor.Lookups
{
    public class LookupProfile: Profile
    {
        public LookupProfile()
        {
            CreateMap<CityViewModel, CityDto>();
            CreateMap<City, CityDto>();

            CreateMap<AreaViewModel, AreaDto>();
            CreateMap<Area, AreaDto>();

            CreateMap<NationalityDto, NationalityViewModel>();
            CreateMap<Nationality, NationalityDto>();
        }
    }
}
