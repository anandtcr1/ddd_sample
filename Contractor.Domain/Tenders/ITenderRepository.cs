using Contractor.Identities;
using Contractor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public interface ITenderRepository
    {
        Task<ListRepositoryModel<Tender>> GetAllAsync(string userId,
            UserTypes loggedInUserTypes,
            string? tenderNumber,
            DateTime? tenderDate, 
            DateTime? openDate,
            DateTime? endDate,
            string? projectNumber,
            string? tenderTitle,
            string? winner,
            TenderStatus? status,
            int pageNumber, int pageSize);
        Task<Tender> GetInvitationListAsync(int tenderId);
        
        Task<Tender> CreateAsync(Tender tender);
        
        Task<Tender> GetAsync(int id);
        
        Task<Tender> GetByProjectIdAsync(int projectId);

        Task<int> GetNextIdAsync();

        Task<int> SaveChangesAsync();
    }
}
