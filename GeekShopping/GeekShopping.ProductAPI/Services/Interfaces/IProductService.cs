using GeekShopping.ProductAPI.DTOs;

namespace GeekShopping.ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<ProductDTO> InsertProductAsync(ProductInsertDTO dto);
        Task<ProductDTO> UpdateProductAsync(ProductInsertDTO dto, int id);
        Task<bool> DeleteProductAsync(int id);
    }
}
