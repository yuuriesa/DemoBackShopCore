using DemoBackShopCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

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
                .HasColumnName("FirstName")
                .HasField("_firstName");

                entity.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnName("LastName")
                .HasField("_lastName");

                entity.HasIndex(c => c.EmailAddress)
                .IsUnique();

                entity.Property(c => c.EmailAddress)
                .IsRequired()
                .HasColumnName("EmailAddress")
                .HasField("_emailAddress");

                entity.Property(c => c.DateOfBirth)
                .IsRequired()
                .HasColumnName("DateOfBirth")
                .HasField("_dateOfBirth")
                .HasDefaultValue(DateOnly.FromDateTime(dateTime: DateTime.Now));

                entity.HasMany(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Ignore(c => c.ErrorMessageIfIsNotValid);
            });

            modelBuilder.Entity<Address>(entity => 
                {
                    entity.HasKey(a => a.AddressId);

                    entity.Property(a => a.ZipCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ZipCode");

                    entity.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Street");

                    entity.Property(a => a.Number)
                    .IsRequired()
                    .HasColumnName("Number");

                    entity.Property(a => a.Neighborhood)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Neighborhood");

                    entity.Property(a => a.AddressComplement)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("AddressComplement");

                    entity.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("City");

                    entity.Property(a => a.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("State");

                    entity.Property(a => a.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Country");

                    entity.HasOne(a => a.Customer)
                    .WithMany(c => c.Addresses)
                    .HasForeignKey(c => c.CustomerId);
                }
            );
        }
    }
}