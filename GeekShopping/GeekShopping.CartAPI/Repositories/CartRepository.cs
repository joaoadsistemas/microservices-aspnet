using AutoMapper;
using GeekShopping.CartAPI.Config;
using GeekShopping.CartAPI.Context;
using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Entities;
using GeekShopping.CartAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {

        private readonly SystemDbContext _dbContext;
        private readonly IMapper _mapper;

        public CartRepository(SystemDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<Cart> FindCartByUserId(string userId)
        {
            Cart cart = new Cart();

            cart.CartHeader = await _dbContext.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart.CartHeader != null)
            {
                // Eagerly load related CartDetails and Product
                cart.CartDetails = await _dbContext.CartDetails
                    .Include(c => c.Product)
                    .ToListAsync();
            }

            return cart;
        }




        public async Task<Cart> SaveOrUpdateCart(CartDTO dto)
        {
            Cart cart = _mapper.Map<Cart>(dto);
            //Checks if the product is already saved in the database if it does not exist then save
            var product = await _dbContext.Products.FirstOrDefaultAsync(    
                p => p.Id == dto.CartDetails.FirstOrDefault().ProductId);

            if (product == null)
            {
                _dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _dbContext.SaveChangesAsync();
            }

            //Check if CartHeader is null

            var cartHeader = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(
                c => c.UserId == cart.CartHeader.UserId);

            if (cartHeader == null)
            {
                //Create CartHeader and CartDetails
                _dbContext.CartHeaders.Add(cart.CartHeader);
                await _dbContext.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                //If CartHeader is not null
                //Check if CartDetails has same product
                var cartDetail = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    p => p.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    p.CartHeaderId == cartHeader.Id);

                if (cartDetail == null)
                {
                    //Create CartDetails
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    //Update product count and CartDetails
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                    _dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _dbContext.SaveChangesAsync();
                }
            }
            return cart;
        }




        public async Task<bool> RemoveFromCart(int id)
        {
            try
            {
                var cartDetail = await _dbContext.CartDetails.FirstOrDefaultAsync(c => c.Id == id);
                if (cartDetail == null)
                {
                    return false;
                }

                int total = _dbContext.CartDetails.Count(c => c.CartHeaderId == cartDetail.CartHeaderId);

                _dbContext.CartDetails.Remove(cartDetail);
                if (total == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeaders
                        .FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                    _dbContext.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            var header = await _dbContext.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (header != null)
            {
                header.CouponCode = couponCode;
                _dbContext.CartHeaders.Update(header);
                await _dbContext.SaveChangesAsync();
                return true;
            }


            return false;
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var header = await _dbContext.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (header != null)
            {
                header.CouponCode = "";
                _dbContext.CartHeaders.Update(header);
                await _dbContext.SaveChangesAsync();
                return true;
            }


            return false;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeader != null)
            {
                _dbContext.CartDetails.RemoveRange(_dbContext.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id));
                _dbContext.CartHeaders.Remove(cartHeader);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


    }




}



