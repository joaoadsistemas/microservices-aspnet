using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeekShopping.CartAPI.Controllers
{
    [Route("carts")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet("find-cart")]
        public async Task<ActionResult<CartDTO>> FindCartByUserId()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _cartService.FindCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);

        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartDTO>> SaveCart([FromBody] CartDTO dto)
        {
            var cart = await _cartService.SaveOrUpdateCart(dto);
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartDTO>> UpdateCart([FromBody] CartDTO dto)
        {
            var cart = await _cartService.SaveOrUpdateCart(dto);
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<bool>> RemoveCart(int id)
        {
            var status = await _cartService.RemoveFromCart(id);
            if (!status)
            {
                return BadRequest();
            }
            return Ok(status);
        }

    }
}
