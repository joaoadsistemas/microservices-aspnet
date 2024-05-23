using GeekShopping.CouponAPI.Entities;

namespace GeekShopping.CouponAPI.DTOs
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }

        public CouponDTO(Coupon entity)
        {
            this.Id = entity.Id;
            this.CouponCode = entity.CouponCode;
            this.DiscountAmount = entity.DiscountAmount;
        }
    }
}
