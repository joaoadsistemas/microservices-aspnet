using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Entities
{
   
    public class OrderHeader
    {
        public int Id { get; set; }


        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double PurchaseAmount { get; set; }
        public double DiscountAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? OrderTime { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }


        public int? CartTotalItens { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }



        public bool PaymentStatus { get; set; }
    }
}
