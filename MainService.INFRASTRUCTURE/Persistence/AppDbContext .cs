using MainService.CORE.Entities;
using MainService.INFRASTRUCTURE.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MainService.INFRASTRUCTURE.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Define DB sets (tables)
        public DbSet<User> Users { get; set; }
        public DbSet<Form> Forms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FormConfiguration());

        }
    }
}