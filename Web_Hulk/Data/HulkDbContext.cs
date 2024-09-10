using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Hulk.Data.Entities;
using Web_Hulk.Data.Entities.Identity;

namespace Web_Hulk.Data
{
    public class HulkDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        public HulkDbContext(DbContextOptions<HulkDbContext> options) : base(options) { 
        }

        public DbSet<CategoryEntity>  Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductImage>()
                .HasOne(x=> x.Product)
                .WithMany(x=> x.ProductImages)
                .HasForeignKey(x => x.ProductId);

            //identity Builder
            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(x => x.User)
                .WithMany(r => r.uSerRoles)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(x => x.Role)
                .WithMany(r => r.Roles)
                .HasForeignKey(r => r.RoleId);
        }

    }
}
