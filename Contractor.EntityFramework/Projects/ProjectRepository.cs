using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DatabaseContext _databaseContext;
        
        public ProjectRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _databaseContext.Projects.AddAsync(project);
            return project;
        }

        public async Task<List<Project>> GetAllLiteAsync(string userId,string? projectNumber)
        {
            projectNumber = !string.IsNullOrEmpty(projectNumber) ? projectNumber.ToLower() : null;

            return await _databaseContext.Projects
                .Where(x => x.ProjectUsers.Any(y => y.UserId == userId) && (string.IsNullOrEmpty(projectNumber) || x.ProjectNumber.Contains(projectNumber)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<Project>> GetAllAsync(string userId, string? projectNumber, int? projectTypdId, string? ownerName, string? projectTitle, DateTime? startDate, DateTime? endDate, string? description, ProjectStatus? status, int pageNumber, int pageSize)
        {
            projectNumber = !string.IsNullOrEmpty(projectNumber) ? projectNumber.ToLower() : null;
            ownerName = !string.IsNullOrEmpty(ownerName) ? ownerName.ToLower() : null;
            projectTitle = !string.IsNullOrEmpty(projectTitle) ? projectTitle.ToLower() : null;
            startDate = startDate.HasValue ? startDate.Value : null;
            endDate = endDate.HasValue ? endDate.Value : null;
            description = !string.IsNullOrEmpty(description) ? description.ToLower() : null;

            var query = _databaseContext.Projects
                            .Where(x =>
                                    x.ProjectUsers.Any(y => y.UserId == userId) &&
                                    (!projectTypdId.HasValue || x.ProjectTypdId == projectTypdId.Value) &&
                                    (!status.HasValue || x.Status == status.Value) &&
                                    (string.IsNullOrEmpty(projectNumber) || x.ProjectNumber.Contains(projectNumber)) &&
                                    (string.IsNullOrEmpty(ownerName) || x.ProjectUsers.Any(y => y.User.DisplayName.Contains(ownerName) && y.UserType == ProjectUserType.Owner)) &&
                                    (string.IsNullOrEmpty(projectTitle) || x.ProjectTitle.Contains(projectTitle)) &&
                                    (!startDate.HasValue || x.StartDate.Date >= startDate.Value.Date) &&
                                    (!endDate.HasValue || x.EndDate.Date <= endDate.Value.Date) &&
                                    (string.IsNullOrEmpty(description) || x.Description.Contains(description))
                                  )
                            .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.ProjectUsers)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectType)
                .ToListAsync();

            return new ListRepositoryModel<Project>(totalRecords, list);
        }

        public async Task<Project> GetAsync(int id)
        {
            return await _databaseContext.Projects
                .Where(x => x.Id == id)
                .Include(x => x.ProjectUsers)
                    .ThenInclude(x => x.User)
                .Include(x => x.ProjectInvitations)
                .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Project), id);
        }

        public async Task<bool> CheckProjectNumberExists(string consultantId,  string projectNumber)
        {
            var query = _databaseContext.Projects
                            .Where(x => x.ProjectNumber == projectNumber &&
                                        x.ProjectUsers.Any(y => y.UserId == consultantId && y.UserType == ProjectUserType.Consultant))
                            .AsNoTracking();

           return await query.AnyAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
