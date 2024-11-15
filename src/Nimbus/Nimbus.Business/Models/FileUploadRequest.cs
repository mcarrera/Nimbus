using Microsoft.AspNetCore.Http;

namespace Nimbus.Business.Models
{
    public class FileUploadRequest
    {
        public string FileName { get; set; } = string.Empty;

        public required IFormFile File { get; set; }
        public Guid? FolderId { get; set; }
    }
}
