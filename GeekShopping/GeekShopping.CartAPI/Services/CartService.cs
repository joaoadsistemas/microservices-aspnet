using AutoMapper;
using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Repositories.Interfaces;

namespace GeekShopping.CartAPI.Services
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }


        public async Task<CartDTO> FindCartByUserId(string userId)
        {
            var result = await _cartRepository.FindCartByUserId(userId);
            return _mapper.Map<CartDTO>(result);
        }

        public async Task<CartDTO> SaveOrUpdateCart(CartDTO dto)
        {
            var result = await _cartRepository.SaveOrUpdateCart(dto);
            return _mapper.Map<CartDTO>(result); ;
        }

        public async Task<bool> RemoveFromCart(int id)
        {
            var result = await _cartRepository.RemoveFromCart(id);
            return result;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId)
        {
            var result = await _cartRepository.ClearCart(userId);
            return result;
        }
    }
}
