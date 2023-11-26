using Contractor.Tools;

namespace Contractor.Lookups
{
    public interface IAddressRepository
    {
        Task<City> GetCityAsync(int id);

        Task<City> CreateCityAsync(City city);

        Task<List<City>> GetAllCityAsync(string? search);

        Task<ListRepositoryModel<City>> GetAllCityAsync(string? arabicName, string? englishName, int pageNumber, int pageSize);

        Task<Area> GetAreaAsync(int id);

        Task<Area> CreateAreaAsync(Area area);

        Task<List<Area>> GetAllAreaAsync(int cityId, string? search);

        Task<ListRepositoryModel<Area>> GetAllAreaAsync(int cityId, string? arabicName, string? englishName, int pageNumber, int pageSize);

        Task<int> SaveChangesAsync();
    }
}
