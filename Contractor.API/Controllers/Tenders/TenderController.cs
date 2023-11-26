using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Contractor.ViewModels.Tenders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Tenders
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TenderController : ControllerBase
    {
        private readonly ITenderAppService _appService;
        private readonly IMapper _mapper;

        public TenderController(ITenderAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }
 
      
        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> GetListAsync(GetTenderListViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _appService.GetAllAsync(userId!.Value, 
                viewModel.TenderNumber,
                viewModel.TenderDate,
                viewModel.OpenDate,
                viewModel.EndDate,
                viewModel.ProjectNumber,
                viewModel.TenderTitle,
                viewModel.Winner,
                viewModel.Status,
                viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        [ClaimRequirement(FunctionalityNames.ViewProjectTender)]
        public async Task<IActionResult> CreateAsync([FromForm]TenderViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var dto = _mapper.Map<TenderDto>(viewModel);

            var tender = await _appService.CreateAsync(dto, userId!.Value, viewModel.Files);

            return Ok(tender);
        }

        [HttpPost("Update")]
        [ClaimRequirement(FunctionalityNames.ViewProjectTender)]
        public async Task<IActionResult> UpdateAsync([FromForm] TenderViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var dto = _mapper.Map<TenderDto>(viewModel);

            var tender = await _appService.UpdateAsync(dto, userId!.Value, viewModel.Files);

            return Ok(tender);
        }

        [HttpDelete("DeleteTenderAccessDefinition/{tenderId}/{accessDefinitionId}")]
        [ClaimRequirement(FunctionalityNames.ViewProjectTender)]
        public async Task<IActionResult> DeleteTenderAccessDefinitionAsync(int tenderId, int accessDefinitionId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.DeleteTenderAccessDefinitionAsync(tenderId, accessDefinitionId, userId!.Value);

            return Ok();
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var tender = await _appService.GetAsync(id);

            return Ok(tender);
        }

        [HttpPost("InviteContractors")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> InviteContractors(TenderInvitationViewModel viewModel)
        {  
             await _appService.InviteContractors(viewModel.TenderId,  viewModel.UserIdList );
            return Ok();
        }

        [HttpGet("GetByProjectId/{projectId}")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> GetByProjectIdAsync(int projectId)
        {
            var tender = await _appService.GetByProjectIdAsync(projectId);

            return Ok(tender);
        }

        [HttpGet("GetForContractor/{id}")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> GetForContractorAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var tender = await _appService.GetForContractorAsync(id, userId!.Value);

            return Ok(tender);
        }

        [HttpPost("AcceptTenderInvitation/{id}")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> AcceptTenderInvitationAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.AcceptTenderInvitationAsync(id, userId!.Value);

            return Ok();
        }

        [HttpPost("DeclineTenderInvitation/{id}")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> DeclineTenderInvitationAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.DeclineTenderInvitationAsync(id, userId!.Value);

            return Ok();
        }

        [HttpPost("SubmitTenderProposal")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> SubmitTenderProposalAsync([FromForm] TenderInvitationSubmissionViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.SubmitTenderProposalAsync(viewModel.TenderId, viewModel.Note, viewModel.Files, userId!.Value);

            return Ok();
        }

        [HttpPost("SelectTenderProposal")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> SelectTenderProposalAsync([FromForm] SelectTenderSubmissionWinnerViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _appService.SelectTenderProposalAsync(viewModel.TenderId, viewModel.InvitationId, userId!.Value);

            return Ok();
        }

        [HttpGet("GetInvitationList/{tenderId}")]
        [ClaimRequirement(PageNames.TenderList)]
        public async Task<IActionResult> GetInvitationListAsync(int tenderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _appService.GetInvitationListAsync(tenderId, userId!.Value);

            return Ok(list);
        }


    }
}
