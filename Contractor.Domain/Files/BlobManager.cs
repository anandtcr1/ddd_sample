using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Projects;
using Contractor.Subscriptions;
using Contractor.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Contractor.Files
{
    public class BlobManager : IBlobManager
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly AzureUrls _azureUrls;
        private readonly IAccessDefinitionRepository _accessDefinitionRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;


        public BlobManager(IOptions<AzureUrls> options,
            BlobServiceClient blobServiceClient,
            IAccessDefinitionRepository accessDefinitionRepository,
            ISubscriptionRepository subscriptionRepository,
            IFileRepository fileRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _azureUrls = options.Value;
            _blobServiceClient = blobServiceClient;
            _containerClient = _blobServiceClient.GetBlobContainerClient(_azureUrls.BlobStorage.ContainerName);
            _accessDefinitionRepository = accessDefinitionRepository;
            _subscriptionRepository = subscriptionRepository;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task CreateProjectFolders(string userId, string username, string projectNumber)
        {
            var defaultSubscription = await _subscriptionRepository.GetDefaultAsync();

            var paths = defaultSubscription.ProjectFolderTemplates!.Select(x => string.Format(FilePathConstants.ProjectFoldersPath, username, defaultSubscription.Name, projectNumber, x.Name));

            var tasks = paths.Select(x => CreateFolder(x, userId, false, null));

            await Task.WhenAll(tasks);

        }

        public async Task CreateProfileMediaFolders(string userId, string username)
        {
            var defaultSubscription = await _subscriptionRepository.GetDefaultAsync();

            var path = string.Format(FilePathConstants.ProfileMediaPath, username, defaultSubscription.Name);
            
            await CreateFolder(path, userId, false, null);

        }

        public async Task CreateProfileChatFolders(string userId, string username)
        {
            var defaultSubscription = await _subscriptionRepository.GetDefaultAsync();

            var path = string.Format(FilePathConstants.ProfileChatPath, username, defaultSubscription.Name);

            await CreateFolder(path, userId, false, null);

        }

        public async Task Delete(int id, string userId, bool forceDelete = false, bool isMedia = false)
        {
            var userFileAccess = await _accessDefinitionRepository.GetAsync(id);

            var user = await _userRepository.GetByIdAsync(userId);

            if ((!forceDelete && !userFileAccess.Deletable) || (user.UserType != UserTypes.Consultant && user.UserType != UserTypes.SubConsultant && !isMedia))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            if (userFileAccess.Type == AccessDefinitionType.File)
            {
                if (userFileAccess.IsOriginal)
                {

                    var blobClient = _containerClient.GetBlobClient(userFileAccess.Path);

                    await blobClient.DeleteIfExistsAsync();
                }
            }
            else
            {
                if (userFileAccess.IsOriginal)
                {
                    var blobItems = _containerClient.GetBlobsAsync(prefix: userFileAccess.Path);
                    await foreach (BlobItem blobItem in blobItems)
                    {
                        BlobClient blobClient = _containerClient.GetBlobClient(blobItem.Name);
                        await blobClient.DeleteIfExistsAsync();
                    }
                }                
            }
            await _accessDefinitionRepository.DeleteAsync(userFileAccess.Id);

            await _accessDefinitionRepository.SaveChangesAsync();
            

        }

        public async Task<long> GetFolderSize(string path)
        {
            long size = 0;

            await foreach (var blob in _containerClient.GetBlobsAsync(prefix: path))
            {
                size += blob.Properties.ContentLength!.Value;
            }

            return size;
        }

        public async Task<(Stream, string)> Download(string path)
        {
            var blobClient = _containerClient.GetBlobClient(path);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return (blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<AccessDefinition> UploadProfileMediaFiles(string userId, string username, IFormFile mediaFile)
        {
            var path = await GetMediaFolderPath(username);

            var mediaFolder = await _accessDefinitionRepository.GetByPathAsync(path);

            var file = File.Create(mediaFile.FileName, mediaFile.Length, FileType.ProfileMedia, path, Path.GetExtension(mediaFile.FileName));

            await _fileRepository.CreateAsync(file);

            var blobClient = _containerClient.GetBlobClient(file.Path);

            using (var memoryStream = new MemoryStream())
            {
                mediaFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.Path.GetContentType() });

                var accessDefinition = await CreateAccessDefinition(file.Path, userId, true, type: AccessDefinitionType.File, null, file);

                mediaFolder.AddChild(accessDefinition);

                return accessDefinition;
            }
        }


        public async Task<AccessDefinition> UploadProfileChatFiles(string userId, string username, IFormFile chatFile)
        {
            var path = await GetChatFolderPath(username);

            var chatFolder = await _accessDefinitionRepository.GetByPathAsync(path);

            var file = File.Create(chatFile.FileName, chatFile.Length, FileType.ProfileChat, path, Path.GetExtension(chatFile.FileName));

            await _fileRepository.CreateAsync(file);

            var blobClient = _containerClient.GetBlobClient(file.Path);

            using (var memoryStream = new MemoryStream())
            {
                chatFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.Path.GetContentType() });

                var accessDefinition = await CreateAccessDefinition(file.Path, userId, true, type: AccessDefinitionType.File, null, file);

                chatFolder.AddChild(accessDefinition);

                return accessDefinition;
            }

            
        }

        public async Task<List<AccessDefinition>> UploadTenderFiles(int projectId, string userId, List<IFormFile> tenderFiles)
        {
            List<AccessDefinition> accessDefinitions = new List<AccessDefinition>();

            var path = await GetProjectFolderPath(projectId, SubscriptionConstants.Tenders);

            var tenderFolder = await _accessDefinitionRepository.GetByPathAsync(path);

            foreach (IFormFile formFile in tenderFiles)
            {
                var file = File.Create(formFile.FileName, formFile.Length, FileType.TenderFiles, path, Path.GetExtension(formFile.FileName));

                await _fileRepository.CreateAsync(file);

                var blobClient = _containerClient.GetBlobClient(file.Path);

                using (var memoryStream = new MemoryStream())
                {
                    formFile.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.Path.GetContentType() });

                    var accessDefinition = await CreateAccessDefinition(file.Path, userId, false, type: AccessDefinitionType.File, null, file);

                    tenderFolder.AddChild(accessDefinition);

                    accessDefinitions.Add(accessDefinition);
                }
            }
            
            return accessDefinitions;
        }

        public async Task<List<AccessDefinition>> UploadCorrespondenceFiles(int projectId, string userId, List<IFormFile> correspondenceFiles)
        {
            List<AccessDefinition> accessDefinitions = new List<AccessDefinition>();

            var path = await GetProjectFolderPath(projectId, SubscriptionConstants.Correspondence);

            var correspondenceFolder = await _accessDefinitionRepository.GetByPathAsync(path);

            foreach (IFormFile formFile in correspondenceFiles)
            {
                var file = File.Create(formFile.FileName, formFile.Length, FileType.TenderFiles, path, Path.GetExtension(formFile.FileName));

                await _fileRepository.CreateAsync(file);

                var blobClient = _containerClient.GetBlobClient(file.Path);

                using (var memoryStream = new MemoryStream())
                {
                    formFile.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.Path.GetContentType() });

                    var accessDefinition = await CreateAccessDefinition(file.Path, userId, false, type: AccessDefinitionType.File, null, file);

                    correspondenceFolder.AddChild(accessDefinition);

                    accessDefinitions.Add(accessDefinition);
                }
            }

            return accessDefinitions;
        }

        public async Task<List<AccessDefinition>> UploadTenderSubmissionProposalFiles(int projectId, string userId, string userDisplayName, List<IFormFile> submissionProposalFiles)
        {
            List<AccessDefinition> accessDefinitions = new List<AccessDefinition>();

            var path = await GetProjectFolderPath(projectId, SubscriptionConstants.Tenders);

            var tenderFolder = await _accessDefinitionRepository.GetByPathAsync(path);

            var submissionFolder = await CreateFolder(userDisplayName, userId, false, tenderFolder.Id);

            tenderFolder.AddChild(submissionFolder);

            foreach (IFormFile formFile in submissionProposalFiles)
            {
                var file = File.Create(formFile.FileName, formFile.Length, FileType.TenderSubmissionFiles, path, Path.GetExtension(formFile.FileName));

                await _fileRepository.CreateAsync(file);

                var blobClient = _containerClient.GetBlobClient(file.Path);

                using (var memoryStream = new MemoryStream())
                {
                    formFile.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.Path.GetContentType() });

                    var accessDefinition = await CreateAccessDefinition(file.Path, userId, false, type: AccessDefinitionType.File, null, file);

                    submissionFolder.AddChild(accessDefinition);

                    accessDefinitions.Add(accessDefinition);
                }
            }

            return accessDefinitions;
        }

        public async Task<AccessDefinition> UploadAsync(string userId, IFormFile formFile, int parentId, FileType fileType)
        {
            var userFileAccess = await _accessDefinitionRepository.GetAsync(parentId);

            var path = userFileAccess.Path;

            if (!userFileAccess.IsOriginal)
            {
                var originalUserFileAccess = await _accessDefinitionRepository.GetAsync(userFileAccess.OriginalId!.Value);

                path = originalUserFileAccess.Path;

                parentId = originalUserFileAccess.Id;
            }

            var file = File.Create(formFile.FileName, formFile.Length, fileType, path, Path.GetExtension(formFile.FileName));

            await _fileRepository.CreateAsync(file);

            var blobClient = _containerClient.GetBlobClient(file.Path);

            using(var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = file.Path.GetContentType() });

                var accessDefinition =  await CreateAccessDefinition(file.Path, userId, true, type: AccessDefinitionType.File, parentId, file);

                await _fileRepository.SaveChangesAsync();

                return accessDefinition;
            }
        }

        public async Task<AccessDefinition> CreateFolder(string folderName, string userId, bool deletable, int? parentId)
        {
            if (parentId.HasValue)
            {
                var parent = await _accessDefinitionRepository.GetAsync(parentId!.Value);

                folderName =  parent.Path + "/" + folderName;
            }

            var accessDefinition = await CreateAccessDefinition(folderName, userId, deletable, type: AccessDefinitionType.Folder, parentId, null);

            folderName += FilePathConstants.EmptyFolderExtension;

            var blobClient = _containerClient.GetBlobClient(folderName);
            byte[] bytes = Array.Empty<byte>();

            using (var memoryStream = new MemoryStream(bytes, writable: false))
            {
                await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = folderName.GetContentType() });
            }

            return accessDefinition;

        }

        public async Task<string> GetProjectFolderPath(int projectId, string? subFolder = null)
        {
            var project = await _projectRepository.GetAsync(projectId);

            var projectConsultant = project.ProjectUsers.FirstOrDefault(x => x.UserType == ProjectUserType.Consultant);

            var consultant = await _userRepository.GetByIdAsync(projectConsultant!.UserId);

            var defaultSubscription = await _subscriptionRepository.GetDefaultAsync();

            if(string.IsNullOrEmpty(subFolder))
            {
                return string.Format(FilePathConstants.ProjectFoldersRoutePath, consultant.Email, defaultSubscription.Name, project.ProjectNumber);
            }
            else
            {
                return string.Format(FilePathConstants.ProjectFoldersPath, consultant.Email, defaultSubscription.Name, project.ProjectNumber, subFolder);
            }

            
        }

        private async Task<string> GetMediaFolderPath(string username)
        {
            var defaultSubscription = await _subscriptionRepository.GetDefaultAsync();

            return string.Format(FilePathConstants.ProfileMediaPath, username, defaultSubscription.Name);
        }

        private async Task<string> GetChatFolderPath(string username)
        {
            var defaultSubscription = await _subscriptionRepository.GetDefaultAsync();

            return string.Format(FilePathConstants.ProfileChatPath, username, defaultSubscription.Name);
        }

        private async Task<AccessDefinition> CreateAccessDefinition(string path, string userId, bool deletable, AccessDefinitionType type, int? parentId, File? file)
        {
            var accessDefinition = AccessDefinition.Create(path, userId, isOriginal: true, status: AccessDefinitionStatus.Active, deletable, type, parentId, file, null);

            await _accessDefinitionRepository.CreateAsync(accessDefinition);

            return accessDefinition;
        }
    }
}
