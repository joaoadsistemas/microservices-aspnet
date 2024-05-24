using GeekShopping.OrderAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Context
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
            
        }

        public DbSet<OrderDetail> Details { get; set; }
        public DbSet<OrderHeader> Headers { get; set; }
    }
}
