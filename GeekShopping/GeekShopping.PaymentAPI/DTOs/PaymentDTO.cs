using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.DTOs
{
    public class PaymentDTO : BaseMessage
    {
        public long OrderId { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public string ExpirationDate { get; set; }
        public double Amount { get; set; }
        public string Email { get; set; }
    }
}
