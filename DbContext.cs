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
            Documents = Set<Document>();
            DriversLicenses = Set<DriversLicense>();
            BirthCertificates = Set<BirthCertificate>();
            Passports = Set<Passport>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DriversLicense> DriversLicenses { get; set; }
        public DbSet<BirthCertificate> BirthCertificates { get; set; }
        public DbSet<Passport> Passports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        UserId = 1,
                        Username = "arya",
                        Password = "password1",
                        Name = "Aryan Kothari",
                        Email = "aryan@aryankothari.dev",
                        PhoneNumber = "64777610177"
                    }
                );
            /* modelBuilder.Entity<DriversLicense>().HasBaseType<Document>(); */
            /* modelBuilder.Entity<BirthCertificate>().HasBaseType<Document>(); */
            /* modelBuilder.Entity<Passport>().HasBaseType<Document>(); */
            modelBuilder.Entity<Document>().ToTable("Documents");
            modelBuilder.Entity<DriversLicense>().ToTable("DriversLicenses");
            modelBuilder.Entity<BirthCertificate>().ToTable("BirthCertificates");
            modelBuilder.Entity<Passport>().ToTable("Passports");

            modelBuilder
                .Entity<Document>()
                .HasOne(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId);
        }
    }
}
