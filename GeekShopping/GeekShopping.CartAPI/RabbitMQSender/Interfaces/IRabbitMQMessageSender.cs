using GeekShopping.MessageBus;

namespace GeekShopping.CartAPI.RabbitMQSender.Interfaces
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
