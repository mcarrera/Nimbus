namespace Nimbus.Business.Models
{
    public class CreateFolderRequest
    {
        public required string Name { get; set; }
        public required Guid ParentFolderId { get; set; }


        public Persistance.Entities.File ToEntity()
        {
            return new Persistance.Entities.File
            {
                Id = Guid.NewGuid(),
                FileName = Name,
                ParentFolderId = this.ParentFolderId,
                IsFolder = true,
                CreatedDateTime = DateTime.UtcNow
            };
        }
    }
}
