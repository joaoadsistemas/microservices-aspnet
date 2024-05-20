using GeekShopping.ProductAPI.DTOs;
using GeekShopping.ProductAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsAsync()
        {
            return Ok(await _productService.GetProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(int id)
        {
            try
            {
                return Ok(await _productService.GetProductByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> InsertProductAsync([FromBody] ProductInsertDTO dto)
        {
            try
            {
                return Ok(await _productService.InsertProductAsync(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProductAsync([FromBody] ProductInsertDTO dto, int id)
        {
            try
            {
                return Ok(await _productService.UpdateProductAsync(dto, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProductAsync(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (result)
                {
                    return Ok(new
                    {
                        message = "Product has been deleted"
                    });
                }

                return BadRequest(new
                {
                    message = "Something went wrong"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
