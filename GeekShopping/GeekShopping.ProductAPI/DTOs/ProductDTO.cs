using GeekShopping.ProductAPI.Entities;

namespace GeekShopping.ProductAPI.DTOs
{
    public class ProductDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }


        public ProductDTO(Product entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Price = entity.Price;
            this.Description = entity.Description;
            this.CategoryName = entity.CategoryName;
            this.ImageUrl = entity.ImageUrl;
        }

    }
}
