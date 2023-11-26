using Contractor.Tools;
using Microsoft.AspNetCore.Http;

namespace Contractor.Correspondences
{
    public interface ICorrespondenceAppService
    {
        Task<CorrespondenceDto> CreateNewAsync(CorrespondenceDto correspondenceDto, string userId, List<string> toRecipients, List<string> cCRecipients, List<IFormFile>? correspondenceFiles);

        Task<CorrespondenceDto> CreateReplayAsync(string? referenceNumber,
            string subject,
            string content,
            int originalId,
            string userId,
            List<IFormFile>? correspondenceFiles);

        Task<CorrespondenceDto> GetAsync(int id);

        Task<List<CorrespondenceDto>> GetByThreadIdAsync(int threadId);

        Task<ListServiceModel<GetCorrespondenceForListDto>> GetIncomingAsync(string userId, string? search, int pageNumber, int pageSize);

        Task<ListServiceModel<GetCorrespondenceForListDto>> GetOutgoingAsync(string userId, string? search, int pageNumber, int pageSize);
    }
}
