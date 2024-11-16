namespace Nimbus.Business.Models
{
    public class FolderDto
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsFolder { get; set; }
        public List<FolderDto> Subfolders { get; set; } = [];
        public List<FileDto> Files { get; set; } = [];
    }
}
