using AutoMapper;
using Contractor.Filters;
using Contractor.Identities;
using Contractor.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Subscriptions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionAppService _appService;
        private readonly IMapper _mapper;

        public SubscriptionsController(IMapper mapper, 
            ISubscriptionAppService appService)
        {
            _mapper = mapper;
            _appService = appService;
        }

        [HttpPost("Create")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> CreateAsync(SubscriptionViewModel viewModel)
        {
            var request = _mapper.Map<SubscriptionDto>(viewModel);

            var subscriptionDto = await _appService.CreateAsync(request);

            return Ok(subscriptionDto);
        }

        [HttpPost("GetList")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> GetAllAsync(GetSubScriptionListViewModel viewModel)
        {
            var list = await _appService.GetAllAsync(viewModel.Name,viewModel.PageNumber, viewModel.PageSize);

            return Ok(list);
        }

        [HttpGet("Get/{id}")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var subscriptionDto = await _appService.GetAsync(id);

            return Ok(subscriptionDto);
        }

        [HttpPost("Update")]
        [ClaimRequirement(PageNames.SubscriptionManagment)]
        public async Task<IActionResult> UpdateAsync(SubscriptionViewModel viewModel)
        {
            var request = _mapper.Map<SubscriptionDto>(viewModel);

            var subscriptionDto = await _appService.UpdateAsync(request);

            return Ok(subscriptionDto);
        }
    }
}
