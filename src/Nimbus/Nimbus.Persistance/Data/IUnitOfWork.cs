namespace Nimbus.Persistance.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IEnumerable<Entities.File>> GetFileList(Guid folderId);
    }

}
