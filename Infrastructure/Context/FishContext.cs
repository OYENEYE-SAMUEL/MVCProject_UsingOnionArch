using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class FishContext : DbContext
    {
        public FishContext(DbContextOptions<FishContext> opt) : base(opt) 
        {
               
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<Pond> Ponds => Set<Pond>();
        public DbSet<Fish> Fishes => Set<Fish>();
        public DbSet<FishPond> FishPonds => Set<FishPond>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderFish> OrderFishItems => Set<OrderFish>();
        public DbSet<Notification> Notifications => Set<Notification>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderFish>()
                .HasOne(f => f.Order)
                .WithMany(d => d.OrderFishItems)
                .HasForeignKey(f => f.OrderId);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Email = "superadmin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("superadmin"),
                    ConfirmedPassword = BCrypt.Net.BCrypt.HashPassword("superadmin"),
                    FullName = "Super Admin",
                    CreatedBy = "Super Admin",
                });
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Name = "Admin",

                });
           
        }

    }
}
