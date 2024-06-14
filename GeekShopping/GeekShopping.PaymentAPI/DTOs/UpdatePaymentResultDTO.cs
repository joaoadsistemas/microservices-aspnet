using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.DTOs
{
    public class UpdatePaymentResultDTO : BaseMessage
    {
        public long OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
