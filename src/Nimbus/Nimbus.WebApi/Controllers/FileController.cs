using Microsoft.AspNetCore.Mvc;
using Nimbus.Business.Models;
using Nimbus.Business.Services;

namespace Nimbus.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{apiVersion}[controller]")]
    public class FileController(IFileService fileService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;


        [HttpGet("list")]
        public async Task<IActionResult> GetFileListAsync([FromQuery] Guid? folderId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileService.GetFileListAsync(folderId ?? Guid.Empty, cancellationToken);
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout, "The request was canceled.");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm] FileUploadRequest request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }


            await _fileService.SaveFileAsync(request, cancellationToken);
            return Ok("File uploaded successfully");

        }
    }
}
