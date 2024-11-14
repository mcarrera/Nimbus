namespace Nimbus.Business.Models
{

    public class FileDescriptor
    {
        public Guid Id { get; set; }
        public Guid? ParentFolderId { get; set; }
        public bool IsFolder { get; set; }
        public required string FileName { get; set; }
        public long? FileSize { get; set; }
        public string? MimeType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public static FileDescriptor FromEntity(Persistance.Entities.File file)
        {
            return new FileDescriptor
            {
                Id = file.Id,
                ParentFolderId = file.ParentFolderId,
                IsFolder = file.IsFolder,
                FileName = file.FileName,
                FileSize = file.FileSize,
                MimeType = file.MimeType,
                CreatedDateTime = file.CreatedDateTime,
                ModifiedDate = file.ModifiedDate
            };
        }
    }



}

