using DemoBackShopCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options: options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Database=DemoBackShopCore;User=SA;Password=Password123!;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString: connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity => 
            {
                entity.HasKey(c => c.CustomerId);

                entity.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("FirstName");

                entity.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("LastName");

                entity.HasIndex(c => c.EmailAddress)
                .IsUnique();

                entity.Property(c => c.EmailAddress)
                .IsRequired()
                .HasColumnName("EmailAddress");

                entity.Property(c => c.DateOfBirth)
                .IsRequired()
                .HasColumnName("DateOfBirth")
                .HasDefaultValue(DateOnly.FromDateTime(dateTime: DateTime.Now));
            });
        }
    }
}