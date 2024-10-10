using StoreApi.Data.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace StoreApi.Data
{
    public class ApiStoreDbContext : IdentityDbContext<UserEntity, RoleEntity, int> //DbContext
    {
        public ApiStoreDbContext(DbContextOptions<ApiStoreDbContext> options)
            : base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRoleEntity>()
               .HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

        }
    }
}