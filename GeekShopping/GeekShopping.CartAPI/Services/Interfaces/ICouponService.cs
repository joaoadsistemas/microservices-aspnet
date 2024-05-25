using GeekShopping.CartAPI.DTOs;

namespace GeekShopping.CartAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCouponByCode(string couponCode, string token);
    }
}
