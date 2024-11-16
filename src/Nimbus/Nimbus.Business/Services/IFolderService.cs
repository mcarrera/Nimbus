using Nimbus.Business.Models;

namespace Nimbus.Business.Services
{
    public interface IFolderService
    {
        Task CreateFolderAsync(CreateFolderRequest request, CancellationToken cancellationToken);
        Task SoftDeleteFolderAsync(Guid folderId, CancellationToken cancellationToken);
        Task<FolderDto> GetFolderTreeAsync(Guid folderId, CancellationToken cancellationToken);
    }
}
