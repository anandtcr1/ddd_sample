using Contractor.Identities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public interface IFileAppService
    {
        Task<FileDto> GetAsync(int id);
        
        Task ShareAccessDefinition(int accessDefinitionId, List<string> sharedWithIdList, string ownerId);

        Task<List<UserListDto>> GetAccessDefinitionUsersAsync(int accessDefinitionId, string userId);

        Task<List<AccessDefinitionDto>> GetRouteAsync(string userId);
        
        Task<List<AccessDefinitionDto>> GetAccessDefinitionsAsync(List<int> accessDefinitionIds);
        
        Task<List<AccessDefinitionDto>> GetByProjectIdAsync(string userId, int projectId);

        Task<List<AccessDefinitionDto>> GetByParentIdAsync(string userId, int parentId);

        Task<FileDownloadResponse> Download(int id);

        Task<long> GetFolderSize(string path);

        Task<AccessDefinitionDto> UploadAsync(string userId, int parentId, IFormFile formFile);
        
        Task<AccessDefinitionDto> CreateFolderAsync(string userId, int parentId, string folderName);

        Task DeleteAsync(int id, string userId);
    }
}
