using StandardTest;
using Microsoft.EntityFrameworkCore;

namespace Housing.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LandUse> LandUses { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<StreetView> StreetViews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Community> Communities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StreetView>()
                .HasNoKey();
        }
    }
}
