using GeekShopping.ProductAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Context
{
    public class SystemDbContext : DbContext
    {

        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) { }
        
        public DbSet<Product> Products { get; set; }


    }
}
