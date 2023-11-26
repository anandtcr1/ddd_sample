using Contractor.Chathub;
using Contractor.ViewModels.Chathub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contractor.Controllers.Chathub
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatMessageAppService _chatAppService;
        public ChatController(IChatMessageAppService chatAppService)
        {
            _chatAppService = chatAppService;
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(ChatUserDto model)
        {
            _chatAppService.AddUserToList(model);
            return NoContent();
        }

        [HttpPost("GetChat")]
        public async Task<IActionResult> GetChatAsync(GetChatMessageViewModel request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var chats = await _chatAppService.GetAllChatsAsync(userId!.Value, request.ToUserId, request.PageNumber, request.PageSize);
            return Ok(chats);
        }

        [HttpPost("UploadChatMedia")]
        public async Task<IActionResult> UploadChatMediaAsync([FromForm] ChatMediaViewModel chatMediaViewModel)
        {
            var accessD = await _chatAppService.UploadChatFile(chatMediaViewModel.FromUserId, chatMediaViewModel.Email, chatMediaViewModel.MediaFile);
            return Ok(accessD);
        }
    }
}
