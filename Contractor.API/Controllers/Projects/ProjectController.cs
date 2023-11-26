using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectAppService _appService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var project = await _appService.GetAsync(id, userId!.Value);

            return Ok(project);
        }

        [HttpGet("GetProjectUsers/{id}")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> GetProjectUsersAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var users = await _appService.GetProjectUsersAsync(id, userId!.Value);

            return Ok(users);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> GetListAsync(GetProjectListViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _appService.GetAllAsync(userId!.Value,
                viewModel.ProjectNumber,
                viewModel.ProjectTypdId,
                viewModel.OwnerName,
                viewModel.ProjectTitle,
                viewModel.StartDate,
                viewModel.EndDate,
                viewModel.Description,
                viewModel.Status,
                viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> CreateAsync(ProjectViewModel viewModel)
        {
            var projectDto = _mapper.Map<ProjectDto>(viewModel);

            var consultantId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var consultantName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            var contractType = await _appService.CreateAsync(projectDto, consultantId!.Value, consultantName!.Value, viewModel.OwnerId);

            return Ok(contractType);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> Update(ProjectViewModel viewModel)
        {
            var projectDto = _mapper.Map<ProjectDto>(viewModel);

            var response = await _appService.UpdateAsync(projectDto);

            return Ok(response);
        }

        [HttpPost("AddProjectSubConsultant")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> AddProjectSubConsultant(ProjectUserViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            
            await _appService.AddProjectSubConsultant(viewModel.ProjectId, userId!.Value, viewModel.UserIdList);

            return Ok();
        }

        [HttpPost("AddProjectContractor")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> AddProjectContractor(ProjectUserViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.AddProjectContractor(viewModel.ProjectId, userId!.Value, viewModel.UserIdList);

            return Ok();
        }

        [HttpPost("AddProjectSubContractor")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> AddProjectSubContractor(ProjectUserViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.AddProjectSubContractor(viewModel.ProjectId, userId!.Value, viewModel.UserIdList);

            return Ok();
        }

        [HttpPost("AddSubUser")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<IActionResult> AddSubUser(ProjectUserViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.AddSubUser(viewModel.ProjectId, userId!.Value, viewModel.UserIdList);

            return Ok();
        }

        [HttpPost("InviteProjectOwner")]
        [ClaimRequirement(PageNames.ProjectManagement)]
        public async Task<ActionResult<ProjectInvitationResponse>> InviteProjectOwner(ProjectInvitationViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var link = await _appService.InviteProjectOwner(viewModel.ProjectId, userId!.Value, viewModel.Email, viewModel.RedirectUrl);

            return Ok(link);
        }

        [HttpGet("CheckProjectInvitation")]
        public async Task<IActionResult> CheckProjectInvitationAsync(int projectId, string email)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var project = await _appService.CheckProjectInvitationAsync(projectId, email, userId!.Value);

            return Ok(project);
        }

        [HttpPost("AcceptProjectInvitation")]
        public async Task<IActionResult> AcceptProjectInvitationAsync(int projectId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.AcceptProjectInvitationAsync(projectId, userId!.Value);

            return Ok();
        }

        [HttpPost("DeclineProjectInvitation")]
        public async Task<IActionResult> DeclineProjectInvitationAsync(int projectId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.DeclineProjectInvitationAsync(projectId, userId!.Value);

            return Ok();
        }
    }
}
