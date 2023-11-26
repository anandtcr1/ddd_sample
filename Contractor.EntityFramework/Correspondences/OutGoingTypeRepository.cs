using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Correspondences
{
    public class OutGoingTypeRepository : IOutGoingTypeRepository
    {
        private readonly DatabaseContext _databaseContext;

        public OutGoingTypeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public async Task<OutGoingType> CreateAsync(OutGoingType outGoingType)
        {
            await _databaseContext.OutGoingTypes.AddAsync(outGoingType);

            return outGoingType;
        }

        public async Task<List<OutGoingType>> GetAllAsync(string? search)
        {
            search = !string.IsNullOrWhiteSpace(search) ? search.ToLower() : null;
            return await _databaseContext.OutGoingTypes
                .Where(x => (string.IsNullOrEmpty(search) || x.ArabicDescription.Contains(search) || x.EnglishDescription.Contains(search)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<OutGoingType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize)
        {
            arabicDescription = !string.IsNullOrEmpty(arabicDescription) ? arabicDescription.ToLower() : null;
            englishDescription = !string.IsNullOrEmpty(englishDescription) ? englishDescription.ToLower() : null;

            var query = _databaseContext.OutGoingTypes
                .Where(x =>
                        (string.IsNullOrEmpty(arabicDescription) || x.ArabicDescription.Contains(arabicDescription)) &&
                        (string.IsNullOrEmpty(englishDescription) || x.EnglishDescription.Contains(englishDescription))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListRepositoryModel<OutGoingType>(totalRecords, list);

        }

        public async Task<OutGoingType> GetAsync(int id)
        {
            return await _databaseContext.OutGoingTypes.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(OutGoingType), id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
