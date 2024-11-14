using Microsoft.AspNetCore.Mvc;
using Nimbus.Business.Services;

namespace Nimbus.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{apiVersion}[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetFileListAsync([FromQuery] Guid folderId)
        {
            var result = await _fileService.GetFileListAsync(folderId);
            return Ok(result);
        }
    }
}
