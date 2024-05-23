using GeekShopping.CouponAPI.Context;
using GeekShopping.CouponAPI.Entities;
using GeekShopping.CouponAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GeekShopping.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly SystemDbContext _dbContext;

        public CouponRepository(SystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Coupon> GetCouponByCode(string couponCode)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            if (coupon == null)
            {
                throw new ArgumentException("Invalid coupon code");
            }
            return coupon;
        }
    }
}