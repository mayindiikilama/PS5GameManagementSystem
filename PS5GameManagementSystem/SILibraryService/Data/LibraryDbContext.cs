using Microsoft.EntityFrameworkCore;
using SILibraryService.Models;

namespace SILibraryService.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<GameLibrary> GameLibraries => Set<GameLibrary>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameLibrary>()
                .Property(g => g.Price)
                .HasPrecision(10, 2);
        }
    }
}
