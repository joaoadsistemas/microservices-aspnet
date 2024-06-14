using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GeekShopping.CartAPI.RabbitMQSender.Interfaces;
using GeekShopping.CartAPI.Services.Interfaces;

namespace GeekShopping.CartAPI.Controllers
{
    [Route("carts")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;
        private IRabbitMQMessageSender _rabbitMQSender;

        public CartController(ICartService cartService, IRabbitMQMessageSender rabbitMqSender, ICouponService couponService)
        {
            _cartService = cartService;
            _rabbitMQSender = rabbitMqSender;
            _couponService = couponService;
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


        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartDTO>> ApplyCoupon([FromBody] CartDTO dto)
        {
            var status = await _cartService.ApplyCoupon(dto.CartHeader.UserId, dto.CartHeader.CouponCode);
            if (!status)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpDelete("remove-coupon")]
        public async Task<ActionResult<CartDTO>> RemoveCoupon()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var status = await _cartService.RemoveCoupon(userId);
            if (!status)
            {
                return NotFound();
            }
            return Ok(status);
        }


        //RABBITMQ
        [HttpPost("checkout")]
        public async Task<ActionResult<PlaceOrderDTO>> Checkout([FromBody] PlaceOrderDTO dto)
        {
            // RECUPERANDO O TOKEN DA REQUEST
            string token = Request.Headers["Authorization"];

            if (dto?.UserId == null) return BadRequest();
            var cart = await _cartService.FindCartByUserId(dto.UserId);
            if (cart == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(dto.CouponCode))
            {
                //entrando no microservices de coupon
                CouponDTO coupon = await _couponService.GetCouponByCode(dto.CouponCode, token);

                // verificando se mudou o calor do coupon
                if (dto.DiscountAmount != coupon.DiscountAmount)
                {
                    // esse status code 412 quer dizer que mudou as condições desde que o cliente mandou a request
                    return StatusCode(412);
                }
            }

            dto.Id = Guid.NewGuid().ToString(); // Atribuindo um novo Guid a dto.Id
            dto.MessageCreated = DateTime.Now;

            dto.CartDetails = cart.CartDetails;

            // MANDANDO O PLACEORDER PARA O RABBITMQ
            _rabbitMQSender.SendMessage(dto, "checkoutqueue");

            // limpando o carrinho apos o checkout
            await _cartService.ClearCart(dto.UserId);

            return Ok(dto);
        }


    }
}
