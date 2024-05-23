using GeekShopping.CouponAPI.DTOs;
using GeekShopping.CouponAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers
{
    [Route("coupons")]
    [ApiController]
    public class CouponController : ControllerBase
    {

        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet("{couponCode}")]
        [Authorize(Policy = "ClientOnly")]
        public async Task<ActionResult<CouponDTO>> GetCouponByCode(string couponCode)
        {
            try
            {
                return Ok(_couponService.GetCouponByCode(couponCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
