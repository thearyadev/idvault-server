using IdVaultServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdVaultServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>? options)
            : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
            Users = Set<User>();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
        }
    }
}
