using System.Collections.Generic;
using E_Commerce.Data.Entities;
using E_Commerce.Data.Fakers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace E_Commerce.Data
{
    public class ApplicationDbContext : DbContext
    { 
        
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<User.Roles>());
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion(new EnumToStringConverter<Order.Statuses>());
            
            modelBuilder.Entity<User>()
                .HasData(new UserFaker().Generate(20));
            base.OnModelCreating(modelBuilder);
        }
    }
}