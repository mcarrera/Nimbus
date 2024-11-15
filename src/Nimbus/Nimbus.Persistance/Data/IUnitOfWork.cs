﻿
namespace Nimbus.Persistance.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Entities.File>> GetFileList(Guid folderId, CancellationToken cancellationToken);
        Task PersistFileAsync(Entities.File fileEntity, CancellationToken cancellationToken);
    }

}
