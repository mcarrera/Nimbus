using Nimbus.Persistance.Repositories;

namespace Nimbus.Persistance.Data
{
    public class UnitOfWork(NimbusDbContext context, INimbusDbRepository nimbusDbRepository) : IUnitOfWork
    {
        private readonly NimbusDbContext _context = context;
        private readonly INimbusDbRepository _nimbusDbRepository = nimbusDbRepository;

        public async Task<IEnumerable<Entities.File>> GetFileList(Guid folderId)
        {
            return await _nimbusDbRepository.GetFileList(folderId);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }


    }

}
