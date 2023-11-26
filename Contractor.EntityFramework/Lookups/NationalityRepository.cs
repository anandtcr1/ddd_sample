using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Lookups
{
    public class NationalityRepository : INationalityRepository
    {
        private readonly DatabaseContext _databaseContext;

        public NationalityRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public async Task<Nationality> CreateAsync(Nationality nationality)
        {
            await _databaseContext.Nationalities.AddAsync(nationality);

            return nationality;
        }

        public async Task<List<Nationality>> GetAllAsync(string? search)
        {
            search = !string.IsNullOrEmpty(search) ? search.ToLower() : null;
            return await _databaseContext.Nationalities
                .Where(x => string.IsNullOrEmpty(search) || x.ArabicName.ToLower().Contains(search) || x.EnglishName.ToLower().Contains(search))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<Nationality>> GetAllAsync(string? arabicName, string? englishName, int pageNumber, int pageSize)
        {
            arabicName = !string.IsNullOrEmpty(arabicName) ? arabicName.ToLower() : null;
            englishName = !string.IsNullOrEmpty(englishName) ? englishName.ToLower() : null;

            var query = _databaseContext.Nationalities
                .Where(x =>
                        (string.IsNullOrEmpty(arabicName) || x.ArabicName.Contains(arabicName)) &&
                        (string.IsNullOrEmpty(englishName) || x.EnglishName.Contains(englishName))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListRepositoryModel<Nationality>(totalRecords, list);
        }

        public async Task<Nationality> GetAsync(int id)
        {
            return await _databaseContext.Nationalities.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Nationality), id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
