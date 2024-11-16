using Nimbus.Business.Models;

namespace Nimbus.Business.Services
{
    public interface IFolderService
    {
        Task CreateFolderAsync(CreateFolderRequest request, CancellationToken cancellationToken);
        Task SoftDeleteFolderAsync(Guid folderId, CancellationToken cancellationToken);

        //Task<IEnumerable<Folder>> GetFoldersAsync(Guid? parentFolderId, CancellationToken cancellationToken);
    }
}
