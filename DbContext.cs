using Microsoft.EntityFrameworkCore;
using IdVaultServer.Models;

namespace IdVaultServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>? options)
                    : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
        }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
        }
    }
}
