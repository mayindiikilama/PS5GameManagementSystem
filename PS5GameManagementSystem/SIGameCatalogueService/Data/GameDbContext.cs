using Microsoft.EntityFrameworkCore;
using SIGameCatalogueService.Models;

namespace SIGameCatalogueService.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games => Set<Game>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(10, 2);
        }
    }
}
