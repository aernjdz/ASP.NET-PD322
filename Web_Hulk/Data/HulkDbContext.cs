using Microsoft.EntityFrameworkCore;
using Web_Hulk.Data.Entities;

namespace Web_Hulk.Data
{
    public class HulkDbContext : DbContext
    {
        public HulkDbContext(DbContextOptions<HulkDbContext> options) : base(options) { 
        }

        public DbSet<CategoryEntity>  Categories { get; set; }

    }
}
