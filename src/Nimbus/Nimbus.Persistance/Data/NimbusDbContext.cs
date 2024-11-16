using Microsoft.EntityFrameworkCore;

namespace Nimbus.Persistance.Data
{


    public class NimbusDbContext : DbContext
    {
        public NimbusDbContext(DbContextOptions<NimbusDbContext> options) : base(options)
        {
            Files = Set<Entities.File>();
        }

        public DbSet<Entities.File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.File>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CreatedDateTime).HasPrecision(0);
                entity.Property(e => e.DeletedDateTime).HasPrecision(0);
                entity.Property(e => e.FileName).HasMaxLength(256);
                entity.Property(e => e.MimeType).HasMaxLength(50);
                entity.Property(e => e.ModifiedDate).HasPrecision(0);
                entity.Property(e => e.FileSize).HasColumnType("bigint");
                entity.Property(e => e.FileContent).HasColumnType("varbinary(max)");

                // parent-child relationship between folders
                entity.HasMany(f => f.ChildFiles).WithOne(f => f.ParentFolder).HasForeignKey(f => f.ParentFolderId).OnDelete(DeleteBehavior.NoAction);

                // global filter to exclude soft-deleted entities from being retrieved
                entity.HasQueryFilter(f => f.DeletedDateTime == null);
            });
        }
    }
}
