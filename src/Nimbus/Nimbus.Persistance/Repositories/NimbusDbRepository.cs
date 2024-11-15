
using Microsoft.EntityFrameworkCore;
using Nimbus.Persistance.Data;

namespace Nimbus.Persistance.Repositories
{
    public class NimbusDbRepository(NimbusDbContext dbContext) : INimbusDbRepository
    {
        private readonly NimbusDbContext _dbContext = dbContext;

        public async Task AddFile(Entities.File fileEntity, CancellationToken cancellationToken)
        {
            await _dbContext.Files.AddAsync(fileEntity, cancellationToken);
        }

        public async Task<IEnumerable<Entities.File>> GetFileList(Guid folderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Files.Where(x => x.ParentFolderId == folderId).ToListAsync(cancellationToken);
        }
        public async Task<Entities.File?> GetFileByIdAsync(Guid fileId, CancellationToken cancellationToken)
        {
            return await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId, cancellationToken);
        }
    }
}
