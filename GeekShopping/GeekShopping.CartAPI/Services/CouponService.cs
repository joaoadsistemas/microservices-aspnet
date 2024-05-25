using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Repositories.Interfaces;
using GeekShopping.CartAPI.Services.Interfaces;

namespace GeekShopping.CartAPI.Services
{
    public class CouponService : ICouponService
    {

        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<CouponDTO> GetCouponByCode(string couponCode, string token)
        {
            var coupon = await _couponRepository.GetCouponByCode(couponCode, token);
            return coupon;
        }
    }
}
