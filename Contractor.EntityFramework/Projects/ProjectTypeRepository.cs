using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectTypeRepository : IProjectTypeRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProjectTypeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ProjectType> CreateAsync(ProjectType projectType)
        {
            await _databaseContext.ProjectTypes.AddAsync(projectType);
            return projectType;
        }

        public async Task<List<ProjectType>> GetAllAsync(string? search)
        {
            search = !string.IsNullOrEmpty(search) ? search.ToLower() : null;
            return await _databaseContext.ProjectTypes
                .Where(x => (string.IsNullOrEmpty(search) || x.ArabicDescription.ToLower().Contains(search) || x.EnglishDescription.ToLower().Contains(search)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<ProjectType>> GetAllAsync(string? arabicDescription, string? englishDescription, int pageNumber, int pageSize)
        {
            arabicDescription = !string.IsNullOrEmpty(arabicDescription) ? arabicDescription.ToLower() : null;
            englishDescription = !string.IsNullOrEmpty(englishDescription) ? englishDescription.ToLower() : null;

            var query = _databaseContext.ProjectTypes
                .Where(x =>
                        (string.IsNullOrEmpty(arabicDescription) || x.ArabicDescription.Contains(arabicDescription)) &&
                        (string.IsNullOrEmpty(englishDescription) || x.EnglishDescription.Contains(englishDescription))
                      ).AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new ListRepositoryModel<ProjectType>(totalRecords, list);
        }

        public async Task<ProjectType> GetAsync(int id)
        {
            return await _databaseContext.ProjectTypes.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(ProjectType), id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
