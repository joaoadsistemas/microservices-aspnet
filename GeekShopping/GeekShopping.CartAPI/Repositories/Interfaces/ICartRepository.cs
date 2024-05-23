using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Entities;

namespace GeekShopping.CartAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> FindCartByUserId(string userId);
        Task<Cart> SaveOrUpdateCart(CartDTO dto);
        Task<bool> RemoveFromCart(int id);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);

    }
}
