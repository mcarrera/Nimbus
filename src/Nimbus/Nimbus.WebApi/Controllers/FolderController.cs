using Microsoft.AspNetCore.Mvc;
using Nimbus.Business.Models;
using Nimbus.Business.Services;

namespace Nimbus.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{apiVersion}[controller]")]
    public class FolderController(IFolderService folderService) : ControllerBase
    {
        private readonly IFolderService _folderService = folderService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateFolderAsync([FromForm] CreateFolderRequest request, CancellationToken cancellationToken)
        {
            await _folderService.CreateFolderAsync(request, cancellationToken);
            return Ok(new { Message = "Folder created." });
        }

        [HttpDelete("{folderId}")]
        public async Task<IActionResult> DeleteFolder(Guid folderId, CancellationToken cancellationToken)
        {
            await _folderService.SoftDeleteFolderAsync(folderId, cancellationToken);
            return Ok(new { Message = "Folder and its contents successfully deleted." });
        }

        [HttpGet("folders/{folderId}/tree")]
        public async Task<IActionResult> GetFolderTree(Guid folderId, CancellationToken cancellationToken)
        {
            var folderTree = await _folderService.GetFolderTreeAsync(folderId, cancellationToken);
            if (folderTree == null)
            {
                return NotFound();
            }
            return Ok(folderTree);
        }

    }
}
