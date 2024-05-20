using GeekShopping.ProductAPI.DTOs;
using GeekShopping.ProductAPI.Entities;

namespace GeekShopping.ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> InsertProductAsync(ProductInsertDTO dto);
        Task<Product> UpdateProductAsync(ProductInsertDTO dto, int id);
        Task<bool> DeleteProductAsync(int id);
    }
}
