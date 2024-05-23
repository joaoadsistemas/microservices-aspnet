using GeekShopping.CouponAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Context
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
            
        }


        public DbSet<Coupon> Coupons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 1,
                CouponCode = "SILVEIRA_2024_10",
                DiscountAmount = 10
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 2,
                CouponCode = "SILVEIRA_2024_15",
                DiscountAmount = 15
            });

        }
    }
}
