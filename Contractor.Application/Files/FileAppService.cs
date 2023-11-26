using AutoMapper;
using Contractor.Exceptions;
using Contractor.Identities;
using Contractor.Projects;
using Contractor.Subscriptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class FileAppService: IFileAppService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IBlobManager _blobManager;
        private readonly IAccessDefinitionRepository _accessDefinitionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public FileAppService(IFileRepository repository,
            IMapper mapper,
            IBlobManager blobManager,
            IAccessDefinitionRepository accessDefinitionRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _fileRepository = repository;
            _mapper = mapper;
            _blobManager = blobManager;
            _accessDefinitionRepository = accessDefinitionRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<AccessDefinitionDto> UploadAsync(string userId, int parentId, IFormFile formFile)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user.UserType == UserTypes.Owner || user.UserType == UserTypes.SubOwner)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            var accessDefinition = await _blobManager.UploadAsync(userId,
                formFile,
                parentId,
                FileType.ProjectFiles);

            return _mapper.Map<AccessDefinitionDto>(accessDefinition);
        }
        public async Task<AccessDefinitionDto> CreateFolderAsync(string userId, int parentId, string folderName)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if(user.UserType == UserTypes.Owner || user.UserType == UserTypes.SubOwner)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            var accessDefinition = await _blobManager.CreateFolder(folderName, userId, true, parentId);

            await _accessDefinitionRepository.SaveChangesAsync();

            return _mapper.Map<AccessDefinitionDto>(accessDefinition);
        }

        public async Task DeleteAsync(int id, string userId)
        {
            await _blobManager.Delete(id, userId);
        }

        public async Task<FileDownloadResponse> Download(int id)
        {
            var accessDefinition = await _accessDefinitionRepository.GetAsync(id);

            var download = await _blobManager.Download(accessDefinition.Path);
            var response = new FileDownloadResponse
            {
                Content = download.Item1,
                ContentType = download.Item2,
            };

            return response;
        }

        public async Task<long> GetFolderSize(string path)
        {
            return await _blobManager.GetFolderSize(path);
        }

        public async Task<FileDto> GetAsync(int id)
        {
            var file = await _fileRepository.GetAsync(id);

            return _mapper.Map<FileDto>(file);
        }

        public async Task<List<AccessDefinitionDto>> GetRouteAsync(string userId)
        {
            var list = await _accessDefinitionRepository.GetRouteAsync(userId);
            
            return _mapper.Map<List<AccessDefinitionDto>>(list);
        }

        public async Task<List<AccessDefinitionDto>> GetAccessDefinitionsAsync(List<int> accessDefinitionIds)
        {
            var list = await _accessDefinitionRepository.GetAsync(accessDefinitionIds);

            return _mapper.Map<List<AccessDefinitionDto>>(list);
        }

        public async Task<List<AccessDefinitionDto>> GetByParentIdAsync(string userId, int parentId)
        {
            var accessDefinition = await _accessDefinitionRepository.GetAsync(parentId);

            if(!accessDefinition.IsOriginal)
            {
                parentId = accessDefinition.OriginalId!.Value;
            }

            var list = await _accessDefinitionRepository.GetByParentIdAsync(parentId);

            return _mapper.Map<List<AccessDefinitionDto>>(list);
        }

        public async Task ShareAccessDefinition(int accessDefinitionId, List<string> sharedWithIdList, string ownerId)
        {
            AccessDefinition access = await _accessDefinitionRepository.GetAsync(accessDefinitionId);

            if (!access.IsOriginal)
            {
                access = await _accessDefinitionRepository.GetAsync(access.OriginalId!.Value);
            }

            var user = await _userRepository.GetByIdAsync(ownerId);            

            if (user.UserType == UserTypes.Owner || user.UserType == UserTypes.SubOwner)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }
            if (user.UserType == UserTypes.SubContractor || user.UserType == UserTypes.Contractor)
            {
                var sharedWithUsers = await _userRepository.GetByIdAsync(sharedWithIdList);

                if(sharedWithUsers.Any(x=>x.UserType != UserTypes.Contractor && x.UserType != UserTypes.SubContractor))
                {
                    throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
                }
            }

            var shared = access.Share(sharedWithIdList);

            await _accessDefinitionRepository.CreateAsync(shared);

            await _accessDefinitionRepository.SaveChangesAsync();
        }

        public async Task<List<UserListDto>> GetAccessDefinitionUsersAsync(int accessDefinitionId, string userId)
        {
            var access = await _accessDefinitionRepository.GetAsync(accessDefinitionId);

            var userIdList = new List<string>();

            if(!access.IsOriginal)
            {
                access = await _accessDefinitionRepository.GetAsync(access.OriginalId!.Value);
            }

            userIdList.Add(access.UserId);
            userIdList.AddRange(access.Copies.Select(x => x.UserId));

            var users = await _userRepository.GetByIdAsync(userIdList);

            var user = await _userRepository.GetByIdAsync(userId);

            switch (user.UserType)
            {
                case UserTypes.Owner:
                case UserTypes.SubOwner:
                    users = users.Where(x => x.UserType == UserTypes.Owner || x.UserType == UserTypes.SubOwner).ToList();
                    break;
                case UserTypes.Consultant:
                case UserTypes.SubConsultant:
                    users = users.Where(x => x.UserType != UserTypes.SubOwner && x.UserType != UserTypes.SubContractor).ToList();
                    break;
                case UserTypes.Contractor:
                case UserTypes.SubContractor:
                    users = users.Where(x => x.UserType == UserTypes.Contractor || x.UserType == UserTypes.SubContractor).ToList();
                    break;
            }

            return _mapper.Map<List<UserListDto>>(users);
        }

        public async Task<List<AccessDefinitionDto>> GetByProjectIdAsync(string userId, int projectId)
        {
            var path = await _blobManager.GetProjectFolderPath(projectId);

            var list = await _accessDefinitionRepository.GetByPathAsync(userId, path);

            var returnedList = _mapper.Map<List<AccessDefinitionDto>>(list);

            foreach (var item in returnedList)
            {
                item.Path = item.Path.Split('/').LastOrDefault() ?? "";
            }

            return returnedList;
        }
    }
}
