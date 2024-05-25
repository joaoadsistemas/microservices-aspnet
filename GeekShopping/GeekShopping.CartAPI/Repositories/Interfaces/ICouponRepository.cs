using GeekShopping.CartAPI.DTOs;

namespace GeekShopping.CartAPI.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string couponCode, string token);
    }
}
