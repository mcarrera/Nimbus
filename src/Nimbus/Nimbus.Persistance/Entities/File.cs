namespace Nimbus.Persistance.Entities;

public partial class File
{
    public Guid Id { get; set; }

    public Guid? ParentFolderId { get; set; }

    public bool IsFolder { get; set; }

    public string FileName { get; set; } = null!;

    public long? FileSize { get; set; }

    public byte[]? FileContent { get; set; } = null!;

    public string? MimeType { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? DeletedDateTime { get; set; }

    public DateTime? ModifiedDate { get; set; }

    // Navigation properties
    public virtual File? ParentFolder { get; set; }

    public virtual ICollection<File>? ChildFiles { get; set; }
}
