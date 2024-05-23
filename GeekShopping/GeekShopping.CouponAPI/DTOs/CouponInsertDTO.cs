using GeekShopping.CouponAPI.Entities;

namespace GeekShopping.CouponAPI.DTOs
{
    public class CouponInsertDTO
    { 
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
    }
}
