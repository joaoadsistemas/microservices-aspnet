using GeekShopping.ProductAPI.DTOs;
using GeekShopping.ProductAPI.Entities;
using GeekShopping.ProductAPI.Repositories.Interfaces;
using GeekShopping.ProductAPI.Services.Interfaces;

namespace GeekShopping.ProductAPI.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        {
            IEnumerable<Product> list = await _productRepository.GetProductsAsync();
            return list.Select(p => new ProductDTO(p)).ToList();
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            Product product = await _productRepository.GetProductByIdAsync(id);
            return new ProductDTO(product);
        }

        public async Task<ProductDTO> InsertProductAsync(ProductInsertDTO dto)
        {
            Product product = await _productRepository.InsertProductAsync(dto);
            return new ProductDTO(product);
        }

        public async Task<ProductDTO> UpdateProductAsync(ProductInsertDTO dto, int id)
        {
            Product product = await _productRepository.UpdateProductAsync(dto, id);
            return new ProductDTO(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            bool result = await _productRepository.DeleteProductAsync(id);
            return result;
        }

       
    }
}
