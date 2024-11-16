namespace Nimbus.Business.Models
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
