using AutoMapper;
using Contractor.Exceptions;
using Contractor.Files;
using Contractor.Identities;
using Contractor.Tools;
using Contractor.Tools.Email;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Projects
{
    public class ProjectAppService : IProjectAppService
    {
        private readonly IProjectRepository _repository;
        private readonly IDraftProjectRepository _draftProjectRepository;
        private readonly IMapper _mapper;
        private readonly IBlobManager _blobManager;
        private readonly IEmailManager _emailManager;
        private readonly IUserRepository _userRepository;

        public ProjectAppService(IProjectRepository repository,
            IMapper mapper,
            IDraftProjectRepository draftProjectRepository,
            IBlobManager blobManager,
            IEmailManager emailManager,
            IUserRepository userRepository)
        {
            _repository = repository;
            _draftProjectRepository = draftProjectRepository;
            _mapper = mapper;
            _blobManager = blobManager;
            _emailManager = emailManager;
            _userRepository = userRepository;
        }

        public async Task<ProjectDto> CreateAsync(ProjectDto projectDto, string consultantId, string consultantName, string? ownerId)
        {
            if(await _repository.CheckProjectNumberExists(consultantId, projectDto.ProjectNumber))
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.DuplicateProjectNumber);
            }

            var project = Project.Create(projectDto.ProjectTypdId, projectDto.ProjectNumber, projectDto.ProjectTitle, projectDto.StartDate, projectDto.EndDate, projectDto.Description);

            if(!string.IsNullOrEmpty(ownerId))
                project.AddOwner(ownerId);
            
            project.AddConsultant(consultantId);

            if (projectDto.DraftProjectId.HasValue)
            {
                var draftProject = await _draftProjectRepository.GetAsync(projectDto.DraftProjectId.Value);
                draftProject.UpdateProjectCreatedStatus();

                project.SetDraftProjectId(draftProject.Id);
            }

            await _repository.CreateAsync(project);

            await _blobManager.CreateProjectFolders(consultantId, consultantName, project.ProjectNumber);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateAsync(ProjectDto projectDto)
        {
            var project = await _repository.GetAsync(projectDto.Id);

            project.Update(projectDto.ProjectTypdId, projectDto.ProjectTitle, projectDto.StartDate, projectDto.EndDate, projectDto.Description, projectDto.Status);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ListServiceModel<ProjectsListDto>> GetAllAsync(string userId,
            string? projectNumber,
            int? projectTypdId,
            string? ownerName,
            string? projectTitle,
            DateTime? startDate,
            DateTime? endDate,
            string? description,
            ProjectStatus? status,
            int pageNumber, int pageSize)
        {
            var list = await _repository.GetAllAsync(userId, projectNumber, projectTypdId, ownerName, projectTitle, startDate, endDate, description, status, pageNumber, pageSize);
            
            return new ListServiceModel<ProjectsListDto>(list.TotalCount, _mapper.Map<List<ProjectsListDto>>(list.List));
        }

        public async Task<ProjectDto> GetAsync(int id, string userId)
        {
            var project = await _repository.GetAsync(id);
            
            var users = FilterProjectUsers(project, userId);

            var model = _mapper.Map<ProjectDto>(project);            

            model.ProjectUsers = _mapper.Map<List<ProjectUserDto>>(users);

            return model;
        }

        public async Task AddProjectSubConsultant(int projectId, string userId, List<string> subConsultantIdList)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            var companyUsers = await _userRepository.GetCompanyUsersAsync(user.CompanyId);

            var subUsers = companyUsers.Where(x => !x.SubscriptionPlanId.HasValue).Select(x => x.Id).ToList();

            project.AddSubConsultant(userId, subConsultantIdList, subUsers);

            await _repository.SaveChangesAsync();
        }

        public async Task AddProjectContractor(int projectId, string userId, List<string> contractorIdList)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            var companyUsers = await _userRepository.GetCompanyUsersAsync(user.CompanyId);

            var subUsers = companyUsers.Where(x => !x.SubscriptionPlanId.HasValue).Select(x => x.Id).ToList();

            project.AddContractor(userId, contractorIdList, subUsers);

            await _repository.SaveChangesAsync();
        }

        public async Task AddProjectSubContractor(int projectId, string userId, List<string> subContractorIdList)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            var companyUsers = await _userRepository.GetCompanyUsersAsync(user.CompanyId);

            var subUsers = companyUsers.Where(x => !x.SubscriptionPlanId.HasValue).Select(x => x.Id).ToList();

            project.AddSubContractor(userId, subContractorIdList, subUsers);

            await _repository.SaveChangesAsync();
        }

        public async Task AddSubUser(int projectId, string userId, List<string> subUserIdList)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            var companyUsers = await _userRepository.GetCompanyUsersAsync(user.CompanyId);

            var subUsers = companyUsers.Where(x => !x.SubscriptionPlanId.HasValue).Select(x => x.Id).ToList();

            project.AddSubUser(userId, subUserIdList, subUsers);

            await _repository.SaveChangesAsync();
        }

        public async Task<ProjectInvitationResponse> InviteProjectOwner(int projectId, string userId, string email, string redirectUrl)
        {
            var project = await _repository.GetAsync(projectId);

            project.AddInvitation(userId, email, ProjectUserType.Owner);

            var link = redirectUrl + "?email=" + email + "&projectId=" + projectId;
            var header = string.Format("Project invitation");

            var emailBody = string.Format("Hi {0}, <br /> You received a project invitation. Please check the <a href=\"{1}\">link</a>", email, link);

            var message = Message.Create(new string[] { email }, header, emailBody);

            await _emailManager.SendEmail(message);
            await _repository.SaveChangesAsync();

            return new ProjectInvitationResponse { Link = link };
        }

        public async Task<ProjectDto> CheckProjectInvitationAsync(int projectId, string email, string userId)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            var projectInvitation = project.ProjectInvitations.FirstOrDefault(x => x.Email == email && x.Status == ProjectInvitationStatus.Pending) ?? throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);

            if(user.Email != email)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task AcceptProjectInvitationAsync(int projectId, string userId)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            project.AcceptInvitation(userId, user.Email);

            await _repository.SaveChangesAsync();
        }

        public async Task DeclineProjectInvitationAsync(int projectId, string userId)
        {
            var project = await _repository.GetAsync(projectId);

            var user = await _userRepository.GetByIdAsync(userId);

            project.DeclineInvitation(userId, user.Email);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<ProjectUserListDto>> GetProjectUsersAsync(int id, string userId)
        {
            var project = await _repository.GetAsync(id);
           
            var users =  FilterProjectUsers(project, userId);

            return _mapper.Map<List<ProjectUserListDto>>(users);
        }

        private List<ProjectUser> FilterProjectUsers(Project project, string userId)
        {
            var projectUser = project.ProjectUsers.FirstOrDefault(x => x.UserId == userId);

            if (projectUser == null)
            {
                throw new BusinessRuleException(BusinessRuleExceptionConstants.AccessDenied);
            }

            switch (projectUser.UserType)
            {
                case ProjectUserType.Contractor:
                case ProjectUserType.SubContractor:
                    return project.ProjectUsers.Where(x => x.UserType == ProjectUserType.Contractor || x.UserType == ProjectUserType.SubContractor || x.UserType == ProjectUserType.Owner).ToList();
                case ProjectUserType.Consultant:
                case ProjectUserType.SubConsultant:
                    return project.ProjectUsers.Where(x => x.UserType == ProjectUserType.Consultant || x.UserType == ProjectUserType.SubConsultant || x.UserType == ProjectUserType.Owner).ToList();
                case ProjectUserType.Owner:
                case ProjectUserType.SubOwner:
                    return project.ProjectUsers.Where(x => x.UserType == ProjectUserType.Owner || x.UserType == ProjectUserType.SubOwner).ToList();

                default:
                    return project.ProjectUsers.ToList();
            }

        }
    }
}
