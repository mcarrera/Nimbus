using Nimbus.Persistance.Repositories;

namespace Nimbus.Persistance.Data
{
    public class UnitOfWork(NimbusDbContext context, INimbusDbRepository nimbusDbRepository) : IUnitOfWork
    {
        private readonly NimbusDbContext _context = context;
        private readonly INimbusDbRepository _nimbusDbRepository = nimbusDbRepository;

        public async Task<IEnumerable<Entities.File>> GetFileList(Guid folderId, CancellationToken cancellationToken)
        {
            return await _nimbusDbRepository.GetFileList(folderId, cancellationToken);
        }


        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task PersistFileAsync(Entities.File fileEntity, CancellationToken cancellationToken)
        {
            // todo: in future implementations, we will persist the file to a cloud BLOB or a filesystem
            // so the UOW will first persist the file, and upon success it will insert the file metadata in the datase

            await _nimbusDbRepository.AddFile(fileEntity, cancellationToken);
        }

        public async Task<Entities.File?> GetFileByIdAsync(Guid fileId, CancellationToken cancellationToken)
        {
            return await _nimbusDbRepository.GetFileByIdAsync(fileId, cancellationToken);
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }

}
