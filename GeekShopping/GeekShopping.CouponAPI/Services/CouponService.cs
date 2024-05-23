using GeekShopping.CouponAPI.DTOs;
using GeekShopping.CouponAPI.Repositories.Interfaces;
using GeekShopping.CouponAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace GeekShopping.CouponAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<CouponDTO> GetCouponByCode(string couponCode)
        {
            var coupon = await _couponRepository.GetCouponByCode(couponCode);
            return new CouponDTO(coupon);
        }
    }
}