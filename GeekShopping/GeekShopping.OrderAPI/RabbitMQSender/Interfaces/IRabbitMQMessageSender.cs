using GeekShopping.MessageBus;

namespace GeekShopping.OrderAPI.RabbitMQSender.Interfaces
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
