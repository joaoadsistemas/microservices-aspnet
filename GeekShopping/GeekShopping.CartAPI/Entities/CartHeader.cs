using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Entities
{
   
    public class CartHeader
    {
        public int Id { get; set; }

        public string UserId { get; set; }


        public string CouponCode { get; set; }
    }
}
