

namespace Nimbus.Persistance.Repositories
{
    public interface INimbusDbRepository
    {
        Task AddFile(Entities.File fileEntity, CancellationToken cancellationToken);
        Task<IEnumerable<Entities.File>> GetFileList(Guid folderId, CancellationToken cancellationToken);
        Task<Entities.File?> GetFileByIdAsync(Guid fileId, CancellationToken cancellationToken);
    }
}
