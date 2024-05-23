using GeekShopping.CartAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Context
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }


    }
}