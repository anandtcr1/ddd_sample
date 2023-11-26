using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Contractor.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DraftProjectController : ControllerBase
    {
        private readonly IDraftProjectAppService _appService;
        private readonly IMapper _mapper;

        public DraftProjectController(IDraftProjectAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.DraftList)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var draftProject = await _appService.GetAsync(id);

            return Ok(draftProject);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.DraftList)]
        public async Task<IActionResult> GetListAsync(GetDraftProjectListViewModel viewModel)
        {
            var consultantId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var list = await _appService.GetAllAsync(consultantId.Value, viewModel.OwnerName, viewModel.statusId, viewModel.createdDate, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.SendInvitationForDraft)]
        public async Task<IActionResult> CreateAsync(DraftProjectViewModel viewModel)
        {
            var request = _mapper.Map<DraftProjectDto>(viewModel);
            
            var ownerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            request.OwnerId = ownerId.Value;
            
            var contractType = await _appService.CreateAsync(request);

            return Ok(contractType);
        }        

        [HttpPost("AcceptRequest")]
        [ClaimRequirement(PageNames.DraftList)]
        public async Task<IActionResult> AcceptRequestAsync(int id)
        {
            var contractType = await _appService.ChangeProjectStatusAsync(id, DraftProjectStatus.Accept);

            return Ok(contractType);
        }

        [HttpPost("RejectRequest")]
        [ClaimRequirement(PageNames.DraftList)]
        public async Task<IActionResult> RejectRequestAsync(int id)
        {
            var contractType = await _appService.ChangeProjectStatusAsync(id, DraftProjectStatus.Reject);

            return Ok(contractType);
        }
    }
}
