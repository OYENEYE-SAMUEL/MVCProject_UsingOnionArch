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
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<OrderFish> OrderFishes => Set<OrderFish>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var adminUserId = Guid.NewGuid();
            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();

            modelBuilder.Entity<OrderItem>()
                .HasOne(f => f.Order)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(f => f.OrderId);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = adminUserId,
                    Email = "superadmin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("superadmin"),
                    ConfirmedPassword = "superadmin",
                    FullName = "Super Admin",
                    CreatedBy = "Super Admin",
                    IsDeleted = false,
                });
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                   Id = adminRoleId,
                   Name = "Admin"

                });

            modelBuilder.Entity<UserRole>()
               .HasData(new UserRole
               {
                   Id = userRoleId,
                   RoleId = adminRoleId,
                   UserId = adminUserId,

               });


        }

    }
}
