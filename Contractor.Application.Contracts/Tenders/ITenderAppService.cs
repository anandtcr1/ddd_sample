using Contractor.Tools;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tenders
{
    public interface ITenderAppService
    {
        Task<ListServiceModel<TendersListDto>> GetAllAsync(string userId, 
            string? tenderNumber,
            DateTime? tenderDate,
            DateTime? openDate,
            DateTime? endDate,
            string? projectNumber,
            string? tenderTitle, 
            string? winner,
            TenderStatus? status, 
            int pageNumber, int pageSize);

        Task<TenderDto> CreateAsync(TenderDto dto, string userId, List<IFormFile> tenderFiles);
        
        Task<TenderDto> UpdateAsync(TenderDto dto, string userId, List<IFormFile> tenderFiles);

        Task DeleteTenderAccessDefinitionAsync(int tenderId, int accessDefinitionId, string userId);

        Task<TenderDto> GetAsync(int id);
           
        Task InviteContractors(int tenderId, List<string> userIdList);
            
        Task<List<TenderUserInvitationsDto>> GetInvitationListAsync(int tenderId, string userId);
        
        Task<TenderDto> GetByProjectIdAsync(int projectId);
        
        Task<ContractorTenderDto> GetForContractorAsync(int id, string userId);
        
        Task AcceptTenderInvitationAsync(int id, string userId);
        
        Task DeclineTenderInvitationAsync(int id, string userId);
        
        Task SubmitTenderProposalAsync(int tenderId, string note, List<IFormFile> files, string userId);
        
        Task SelectTenderProposalAsync(int tenderId, int invitationId, string userId);
    }
}
