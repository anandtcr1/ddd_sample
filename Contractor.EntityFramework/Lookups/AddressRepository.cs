using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AddressRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<City> CreateCityAsync(City city)
        {
            await _databaseContext.Cities.AddAsync(city);

            return city;
        }

        public async Task<List<City>> GetAllCityAsync(string? search)
        {
            search = !string.IsNullOrWhiteSpace(search) ? search.ToLower() : null;
            return await _databaseContext.Cities
                .Where(x => string.IsNullOrEmpty(search) || x.ArabicName.ToLower().Contains(search) || x.EnglishName.ToLower().Contains(search))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<City>> GetAllCityAsync(string? arabicName, string? englishName, int pageNumber, int pageSize)
        {
            arabicName = !string.IsNullOrEmpty(arabicName) ? arabicName.ToLower() : null;
            englishName = !string.IsNullOrEmpty(englishName) ? englishName.ToLower() : null;

            var query = _databaseContext.Cities
                .Where(x =>
                        (string.IsNullOrEmpty(arabicName) || x.ArabicName.Contains(arabicName)) &&
                        (string.IsNullOrEmpty(englishName) || x.EnglishName.Contains(englishName))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListRepositoryModel<City>(totalRecords, list);
        }

        public async Task<City> GetCityAsync(int id)
        {
            return await _databaseContext.Cities.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(City), id);
        }

        public async Task<Area> CreateAreaAsync(Area area)
        {
            await _databaseContext.Areas.AddAsync(area);

            return area;
        }

        public async Task<List<Area>> GetAllAreaAsync(int cityId,string? search)
        {
            search = !string.IsNullOrWhiteSpace(search) ? search.ToLower() : null;
            return await _databaseContext.Areas
                .Where(x => x.CityId == cityId && (string.IsNullOrEmpty(search) || x.ArabicName.ToLower().Contains(search) || x.EnglishName.ToLower().Contains(search)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<Area>> GetAllAreaAsync(int cityId, string? arabicName, string? englishName, int pageNumber, int pageSize)
        {
            arabicName = !string.IsNullOrEmpty(arabicName) ? arabicName.ToLower() : null;
            englishName = !string.IsNullOrEmpty(englishName) ? englishName.ToLower() : null;

            var query = _databaseContext.Areas
                .Where(x =>
                         x.CityId == cityId &&
                        (string.IsNullOrEmpty(arabicName) || x.ArabicName.Contains(arabicName)) &&
                        (string.IsNullOrEmpty(englishName) || x.EnglishName.Contains(englishName))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListRepositoryModel<Area>(totalRecords, list);
        }

        public async Task<Area> GetAreaAsync(int id)
        {
            return await _databaseContext.Areas.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Area), id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
