using AutoMapper;
using Contractor.Correspondences;
using Contractor.Filters;
using Contractor.Identities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Controllers.Correspondences
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrespondencesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICorrespondenceAppService _correspondenceAppService;

        public CorrespondencesController(ICorrespondenceAppService correspondenceAppService, IMapper mapper)
        {
            _correspondenceAppService = correspondenceAppService;
            _mapper = mapper;
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.CorrespondenceList)]
        public async Task<ActionResult<CorrespondenceDto>> GetAsync(int id)
        {
            var correspondence = await _correspondenceAppService.GetAsync(id);

            return Ok(correspondence);
        }

        [HttpGet("GetByThreadId/{threadId}")]
        [ClaimRequirement(PageNames.CorrespondenceList)]
        public async Task<ActionResult<List<CorrespondenceDto>>> GetByThreadIdAsync(int threadId)
        {
            var correspondenceList = await _correspondenceAppService.GetByThreadIdAsync(threadId);

            return Ok(correspondenceList);
        }

        [HttpPost("CreateNew")]
        [ClaimRequirement(PageNames.CorrespondenceList)]
        public async Task<ActionResult<CorrespondenceDto>> CreateNewAsync([FromForm] CorrespondenceViewModel viewModel)
        {
            var request = _mapper.Map<CorrespondenceDto>(viewModel);
            
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var correspondence = await _correspondenceAppService.CreateNewAsync(request, userId!.Value, viewModel.ToRecipients, viewModel.CCRecipients, viewModel.CorrespondenceFiles);

            return Ok(correspondence);
        }

        [HttpPost("CreateReplay")]
        [ClaimRequirement(PageNames.CorrespondenceList)]
        public async Task<ActionResult<CorrespondenceDto>> CreateReplayAsync([FromForm] CorrespondenceReplayViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var correspondence = await _correspondenceAppService.CreateReplayAsync(viewModel.ReferenceNumber,
                viewModel.Subject,
                viewModel.Content,
                viewModel.OriginalId,
                userId!.Value,
                viewModel.CorrespondenceFiles);

            return Ok(correspondence);
        }

        [HttpPost("GetIncoming")]
        [ClaimRequirement(PageNames.CorrespondenceList)]
        public async Task<IActionResult> GetIncomingAsync(GetCorrespondenceListViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _correspondenceAppService.GetIncomingAsync(userId!.Value, viewModel.Search, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpPost("GetOutgoing")]
        [ClaimRequirement(PageNames.CorrespondenceList)]
        public async Task<IActionResult> GetOutgoingAsync(GetCorrespondenceListViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _correspondenceAppService.GetOutgoingAsync(userId!.Value, viewModel.Search, viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }
    }
}
