namespace GeekShopping.CartAPI.DTOs
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }

        public CouponDTO()
        {
            
        }
    }
}
