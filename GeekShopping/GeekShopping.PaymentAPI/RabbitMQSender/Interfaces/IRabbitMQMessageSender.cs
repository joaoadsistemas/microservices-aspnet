using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.RabbitMQSender.Interfaces
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage);
    }
}
