using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Projects;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Tenders
{
    public class TenderRepository: ITenderRepository
    {
        private readonly DatabaseContext _databaseContext;

        public TenderRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ListRepositoryModel<Tender>> GetAllAsync(string userId,
            UserTypes loggedInUserTypes,
            string? tenderNumber, 
            DateTime? tenderDate,
            DateTime? openDate, 
            DateTime? endDate, 
            string? projectNumber, 
            string? tenderTitle, 
            string? winner, 
            TenderStatus? status, 
            int pageNumber, int pageSize)
        {

            projectNumber = !string.IsNullOrEmpty(projectNumber) ? projectNumber.ToLower() : null;
            tenderNumber = !string.IsNullOrEmpty(tenderNumber) ? tenderNumber.ToLower() : null;
            tenderTitle = !string.IsNullOrEmpty(tenderTitle) ? tenderTitle.ToLower() : null;
            winner = !string.IsNullOrEmpty(winner) ? winner.ToLower() : null;
            tenderDate = tenderDate.HasValue ? tenderDate.Value : null;
            endDate = endDate.HasValue ? endDate.Value : null;
            openDate = openDate.HasValue ? openDate.Value : null;

             
            if (loggedInUserTypes == UserTypes.Consultant || loggedInUserTypes == UserTypes.SubConsultant)
            {  
                var query = _databaseContext.Tenders
                                .Where(x =>
                                           x.Project.ProjectUsers.Any(y => y.UserId == userId) &&
                                           (string.IsNullOrEmpty(projectNumber) || x.Project.ProjectNumber.Contains(projectNumber)) &&
                                         (string.IsNullOrEmpty(tenderNumber) || x.Number.Contains(tenderNumber)) &&
                                        (!tenderDate.HasValue || x.IssueDate.Date >= tenderDate.Value.Date) &&
                                         (!openDate.HasValue || x.OpenDate.Date >= openDate.Value.Date) &&
                                        (!endDate.HasValue || x.CloseDate.Date <= endDate.Value.Date)&& 
                                        (!status.HasValue || x.Status == status.Value) &&
                                         (string.IsNullOrEmpty(tenderTitle) || x.Title.Contains(tenderTitle))  &&
                                          (string.IsNullOrEmpty(winner) || x.TenderInvitations.Any(y=>y.Status == TenderInvitationStatus.Winner && y.Contractor.DisplayName.Contains(winner)))
                                      )
                                .AsNoTracking();

                var totalCount = await query.CountAsync();
                var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.Project)
                    .Include(x=>x.TenderInvitations)
                        .ThenInclude(x=>x.Contractor)
                    .ToListAsync();

                return new ListRepositoryModel<Tender>(totalCount, list);
            }
            else if(loggedInUserTypes == UserTypes.Contractor || loggedInUserTypes == UserTypes.SubContractor)
            {
                var query = _databaseContext.Tenders
                                .Where(x =>
                                           x.TenderInvitations.Any(y=>y.ContractorId == userId) &&
                                           (string.IsNullOrEmpty(projectNumber) || x.Project.ProjectNumber.Contains(projectNumber)) &&
                                         (string.IsNullOrEmpty(tenderNumber) || x.Number.Contains(tenderNumber)) &&
                                        (!tenderDate.HasValue || x.IssueDate.Date >= tenderDate.Value.Date) &&
                                         (!openDate.HasValue || x.OpenDate.Date >= openDate.Value.Date) &&
                                        (!endDate.HasValue || x.CloseDate.Date <= endDate.Value.Date) &&
                                        (!status.HasValue || x.Status == status.Value) &&
                                         (string.IsNullOrEmpty(tenderTitle) || x.Title.Contains(tenderTitle))
                                      )
                                .AsNoTracking();

                var totalCount = await query.CountAsync();
                var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.Project)
                    .Include(x => x.TenderInvitations)
                        .ThenInclude(x => x.Contractor)
                    .ToListAsync();
                return new ListRepositoryModel<Tender>(totalCount, list);
            }
            else
            {
                return new ListRepositoryModel<Tender>(0, new List<Tender>());
            }
        }

        public async Task<Tender> CreateAsync(Tender tender)
        {
            await _databaseContext.Tenders.AddAsync(tender);

            return tender;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<int> GetNextIdAsync()
        {
            return await _databaseContext.Tenders.MaxAsync(x=> x.Id) + 1;
        }

        public async Task<Tender> GetAsync(int id)
        {
            return await _databaseContext.Tenders
                .Where(x => x.Id == id)
                .Include(x => x.TenderAccessDefinitions)
                .Include(x => x.TenderInvitations)
                    .ThenInclude(x => x.InvitationAccessDefinitions)
                .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Tender), id);
        }

        public async Task<Tender> GetInvitationListAsync(int tenderId)
        {
            return await _databaseContext.Tenders
                .Where(x => x.Id == tenderId)
                .Include(x => x.TenderInvitations)
                    .ThenInclude(x => x.InvitationAccessDefinitions)
                .Include(x => x.TenderInvitations)
                    .ThenInclude(x => x.Contractor) 
                .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Tender),  tenderId);
        }

        public async Task<Tender> GetByProjectIdAsync(int projectId)
        {
            return await _databaseContext.Tenders
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.TenderAccessDefinitions)
                .Include(x => x.TenderInvitations)
                    .ThenInclude(x => x.InvitationAccessDefinitions)
                .FirstOrDefaultAsync();
        }
    }
}
