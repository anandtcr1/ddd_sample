using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Correspondences
{
    public class IncomeTypeRepository : IIncomeTypeRepository
    {
        private readonly DatabaseContext _databaseContext;

        public IncomeTypeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public async Task<IncomeType> CreateAsync(IncomeType incomeType)
        {
            await _databaseContext.IncomeTypes.AddAsync(incomeType);

            return incomeType;
        }

        public async Task<List<IncomeType>> GetAllAsync(string? search)
        {
            search = !string.IsNullOrWhiteSpace(search) ? search.ToLower() : null;
            return await _databaseContext.IncomeTypes
                .Where(x => (string.IsNullOrEmpty(search) || x.ArabicDescription.Contains(search) || x.EnglishDescription.Contains(search)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<IncomeType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize)
        {
            arabicDescription = !string.IsNullOrEmpty(arabicDescription) ? arabicDescription.ToLower() : null;
            englishDescription = !string.IsNullOrEmpty(englishDescription) ? englishDescription.ToLower() : null;

            var query = _databaseContext.IncomeTypes
                .Where(x =>
                        (string.IsNullOrEmpty(arabicDescription) || x.ArabicDescription.Contains(arabicDescription)) &&
                        (string.IsNullOrEmpty(englishDescription) || x.EnglishDescription.Contains(englishDescription))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListRepositoryModel<IncomeType>(totalRecords, list);

        }

        public async Task<IncomeType> GetAsync(int id)
        {
            return await _databaseContext.IncomeTypes.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(IncomeType), id);
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _databaseContext.SaveChangesAsync();
        }
    }
}
