using Microsoft.EntityFrameworkCore;
using Web_Hulk.Data.Entities;

namespace Web_Hulk.Data
{
    public class HulkDbContext : DbContext
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
        }

    }
}
