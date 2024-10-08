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
            SharedDocuments = Set<SharedDocument>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DriversLicense> DriversLicenses { get; set; }
        public DbSet<BirthCertificate> BirthCertificates { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<SharedDocument> SharedDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        UserId = 1,
                        Username = "user1",
                        Password = "user1",
                        Name = "User 1",
                        Email = "test@testuser.com",
                        PhoneNumber = "4169973041",
                        PublicKey = "",
                    },
                    new User
                    {
                        UserId = 2,
                        Username = "user2",
                        Password = "user2",
                        Name = "User 2",
                        Email = "user2@user2.com",
                        PhoneNumber = "6473310099",
                        PublicKey = "",
                    }
                );

            modelBuilder.Entity<SharedDocument>().ToTable("SharedDocuments");
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
