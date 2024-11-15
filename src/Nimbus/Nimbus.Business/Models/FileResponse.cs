namespace Nimbus.Business.Models
{
    public class FileResponse
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public string? Base64Content { get; set; }
    }
}