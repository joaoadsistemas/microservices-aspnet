using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; }
        public int ProductId { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
