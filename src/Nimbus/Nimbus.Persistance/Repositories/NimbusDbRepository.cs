
using Microsoft.EntityFrameworkCore;
using Nimbus.Persistance.Data;

namespace Nimbus.Persistance.Repositories
{
    public class NimbusDbRepository : INimbusDbRepository
    {
        private readonly NimbusDbContext _dbContext;
        public NimbusDbRepository(NimbusDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Entities.File>> GetFileList(Guid folderId)
        {
            return await _dbContext.Files.Where(x => x.ParentFolderId == folderId).ToListAsync();
        }
    }
}
