using GeekShopping.CouponAPI.DTOs;
using GeekShopping.CouponAPI.Entities;

namespace GeekShopping.CouponAPI.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> GetCouponByCode(string couponCode);
    }
}
