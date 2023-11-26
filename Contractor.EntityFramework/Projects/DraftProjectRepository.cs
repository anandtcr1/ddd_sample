using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class DraftProjectRepository : IDraftProjectRepository
    {
        private readonly DatabaseContext _databaseContext;

        public DraftProjectRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<DraftProject> CreateAsync(DraftProject draftProject)
        {
            await _databaseContext.DraftProjects.AddAsync(draftProject);
            return draftProject;
        }

        public async Task<ListRepositoryModel<DraftProject>> GetAllAsync(string consultantId, string? ownerName, DraftProjectStatus? statusId, DateTime? createdDate, int pageNumber, int pageSize)
        {
            ownerName = !string.IsNullOrEmpty(ownerName) ? ownerName.ToLower() : null;
            statusId = (statusId == null) ? null : statusId;
            createdDate = createdDate.HasValue ? createdDate.Value : null;

            var query = _databaseContext.DraftProjects
                            .Where(x =>
                                (x.ConsultantId == consultantId) &&
                                (string.IsNullOrEmpty(ownerName) || (x.Owner.DisplayName.ToLower().Contains(ownerName.ToLower()))) &&
                                (statusId == null || x.StatusId == statusId) &&
                                (!createdDate.HasValue || x.CreatedDate.Date == createdDate.Value.Date)
                                )
                            .Include(x => x.Owner)
                            .Include(x => x.Consultant)
                            .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new ListRepositoryModel<DraftProject>(totalRecords, list);
        }

        public async Task<DraftProject> GetAsync(int id)
        {
            return await _databaseContext.DraftProjects.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(DraftProject), id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
