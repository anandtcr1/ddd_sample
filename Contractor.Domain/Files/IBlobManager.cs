using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public interface IBlobManager
    {
        Task<AccessDefinition> UploadAsync(string userId, IFormFile formFile, int parentId, FileType fileType);

        Task<AccessDefinition> CreateFolder(string folderName, string userId, bool deletable, int? parentId);

        Task<string> GetProjectFolderPath(int projectId, string? subFolder = null);

        Task<List<AccessDefinition>> UploadTenderFiles(int projectId, string userId, List<IFormFile> tenderFiles);

        Task<List<AccessDefinition>> UploadTenderSubmissionProposalFiles(int projectId, string userId, string userDisplayName, List<IFormFile> submissionProposalFiles);

        Task<List<AccessDefinition>> UploadCorrespondenceFiles(int projectId, string userId, List<IFormFile> correspondenceFiles);

        Task<AccessDefinition> UploadProfileMediaFiles(string userId, string username, IFormFile mediaFile);

        Task<AccessDefinition> UploadProfileChatFiles(string userId, string username, IFormFile chatFile); 

        Task<long> GetFolderSize(string path);

        Task<(Stream, string)> Download(string path);

        Task CreateProjectFolders(string userId, string username, string projectNumber);

        Task CreateProfileMediaFolders(string userId, string username);
        Task CreateProfileChatFolders(string userId, string username); 
        Task Delete(int id, string userId, bool forceDelete = false, bool isMedia = false);
    }
}
