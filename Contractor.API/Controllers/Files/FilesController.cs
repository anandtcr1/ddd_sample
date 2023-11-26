using AutoMapper;
using Contractor.Files;
using Contractor.Identities;
using Contractor.ViewModels.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Contractor.Controllers.Files
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileAppService _fileAppService;
        private readonly IMapper _mapper;


        public FilesController(IFileAppService fileAppService, IMapper mapper)
        {
            _fileAppService = fileAppService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult<FileDto>> UploadAsync([FromForm] AccessDefinitionViewModel view)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var respose = await _fileAppService.UploadAsync(userId!.Value, view.ParentId, view.FormFile);

            return Ok(respose);
        }

        [HttpPost]
        [Route("CreateFolder/{parentId}")]
        public async Task<ActionResult<FileDto>> CreateFolderAsync(int parentId, [Required] string folderName)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var respose = await _fileAppService.CreateFolderAsync(userId!.Value, parentId, folderName);

            return Ok(respose);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _fileAppService.DeleteAsync(id, userId!.Value);

            return Ok();
        }

        [HttpGet]
        [Route("Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _fileAppService.Download(id);

            return File(file.Content, file.ContentType);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<ActionResult<FileDto>> GetAsync(int id)
        {
            var file = await _fileAppService.GetAsync(id);

            return Ok(file);
        }

        [HttpPost]
        [Route("ShareAccessDefinition")]
        public async Task<ActionResult<FileDto>> ShareAccessDefinition(ShareAccessDefinitionViewModel viewModel)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            await _fileAppService.ShareAccessDefinition(viewModel.Id, viewModel.SharedWithIdList, userId!.Value);

            return Ok();
        }

        [HttpGet]
        [Route("GetRoute")]
        public async Task<ActionResult<List<AccessDefinitionDto>>> GetRoute()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            
            var list = await _fileAppService.GetRouteAsync(userId!.Value);

            return Ok(list);
        }

        [HttpPost]
        [Route("GetAccessDefinitions")]
        public async Task<ActionResult<List<AccessDefinitionDto>>> GetAccessDefinitionsAsync(List<int> accessDefinitionIds)
        {
            var list = await _fileAppService.GetAccessDefinitionsAsync(accessDefinitionIds);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetByProjectId/{projectId}")]
        public async Task<ActionResult<List<AccessDefinitionDto>>> GetByProjectId(int projectId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _fileAppService.GetByProjectIdAsync(userId!.Value, projectId);

            return Ok(list);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("GetAccessDefinitionUsers/{accessDefinitionId}")]
        public async Task<ActionResult<List<UserListDto>>> GetAccessDefinitionUsers(int accessDefinitionId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _fileAppService.GetAccessDefinitionUsersAsync(accessDefinitionId, userId.Value);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetByParentId/{parentId}")]
        public async Task<ActionResult<List<AccessDefinitionDto>>> GetByParentIdAsync(int parentId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var list = await _fileAppService.GetByParentIdAsync(userId!.Value, parentId);

            return Ok(list);
        }
    }
}
