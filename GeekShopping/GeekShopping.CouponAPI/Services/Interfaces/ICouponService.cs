using GeekShopping.CouponAPI.DTOs;

namespace GeekShopping.CouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCouponByCode(string couponCode);
    }
}
