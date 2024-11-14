
namespace Nimbus.Persistance.Repositories
{
    public interface INimbusDbRepository
    {
        Task<IEnumerable<Entities.File>> GetFileList(Guid folderId);
    }
}
