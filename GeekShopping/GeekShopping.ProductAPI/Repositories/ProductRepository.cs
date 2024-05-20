using GeekShopping.ProductAPI.Context;
using GeekShopping.ProductAPI.DTOs;
using GeekShopping.ProductAPI.Entities;
using GeekShopping.ProductAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly SystemDbContext _dbContext;

        public ProductRepository(SystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            List<Product> products = await _dbContext.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id)
                              ?? throw new ArgumentException("Product not found");

            return product;
        }

        public async Task<Product> InsertProductAsync(ProductInsertDTO dto)
        {
            Product entity = new Product();
            copyDTOToEntity(dto, entity);

            _dbContext.Products.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public async Task<Product> UpdateProductAsync(ProductInsertDTO dto, int id)
        {
            Product entity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id)
                              ?? throw new ArgumentException("Product not found");

            copyDTOToEntity(dto, entity);

            _dbContext.Products.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                Product product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id)
                                  ?? throw new ArgumentException("Product not found");

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void copyDTOToEntity(ProductInsertDTO dto, Product entity)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.ImageUrl = dto.ImageUrl;
            entity.CategoryName = dto.CategoryName;
        }
    }
}
