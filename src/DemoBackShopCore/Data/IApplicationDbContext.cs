using DemoBackShopCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public int SaveChanges();
    }
}