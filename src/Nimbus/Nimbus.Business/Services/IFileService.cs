
using Nimbus.Business.Models;

namespace Nimbus.Business.Services
{
    public interface IFileService
    {
        Task<Result<IEnumerable<FileDescriptor>>> GetFileListAsync(Guid? folderId, CancellationToken cancellationToken);
        Task SaveFileAsync(FileUploadRequest request, CancellationToken cancellationToken);
    }
}
