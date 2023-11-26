using Contractor.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Contractor.Identities
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageAppService _pageAppService;

        public PageController(IPageAppService pageAppService)
        {
            _pageAppService = pageAppService;
        }

        [HttpPost("GetPages")]
        [ClaimRequirement(PageNames.PageManagment)]
        public async Task<IActionResult> GetPages(PageViewModel request)
        {
            var response = await _pageAppService.GetListAsync(request.Name, request.PageNumber, request.PageSize);
            return Ok(response);
        }
    }
}
