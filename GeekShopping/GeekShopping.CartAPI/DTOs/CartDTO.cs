using GeekShopping.CartAPI.Entities;

namespace GeekShopping.CartAPI.DTOs
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeader { get; set; }
        public IEnumerable<CartDetailDTO> CartDetails { get; set; }

       
    }
}
