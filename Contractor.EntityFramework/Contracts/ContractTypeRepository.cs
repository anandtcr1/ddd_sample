using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Contracts
{
    public class ContractTypeRepository : IContractTypeRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ContractTypeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ContractType> CreateAsync(ContractType contractType)
        {
            await _databaseContext.ContractTypes.AddAsync(contractType);
            return contractType;
        }

        public async Task<List<ContractType>> GetAllAsync(string? search)
        {
            search = !string.IsNullOrEmpty(search) ? search.ToLower() : null;
            return await _databaseContext.ContractTypes
                .Where(x => (string.IsNullOrEmpty(search) || x.ArabicDescription.ToLower().Contains(search) || x.EnglishDescription.ToLower().Contains(search)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<ContractType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize)
        {
            arabicDescription = !string.IsNullOrEmpty(arabicDescription) ? arabicDescription.ToLower() : null;
            englishDescription = !string.IsNullOrEmpty(englishDescription) ? englishDescription.ToLower() : null;

            var query = _databaseContext.ContractTypes
                .Where(x =>
                        (string.IsNullOrEmpty(arabicDescription) || x.ArabicDescription.Contains(arabicDescription)) &&
                        (string.IsNullOrEmpty(englishDescription) || x.EnglishDescription.Contains(englishDescription))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new ListRepositoryModel<ContractType>(totalRecords, list);
        }

        public async Task<ContractType> GetAsync(int id)
        {
            return await _databaseContext.ContractTypes.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(ContractType), id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
