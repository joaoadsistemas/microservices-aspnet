using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Entities;

namespace GeekShopping.CartAPI.Repositories.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> FindCartByUserId(string userId);
        Task<CartDTO> SaveOrUpdateCart(CartDTO dto);
        Task<bool> RemoveFromCart(int id);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);

    }
}
