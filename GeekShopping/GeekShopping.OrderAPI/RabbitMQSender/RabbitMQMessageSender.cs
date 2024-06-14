using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.OrderAPI.DTOs;
using GeekShopping.OrderAPI.RabbitMQSender.Interfaces;
using RabbitMQ.Client;

namespace GeekShopping.OrderAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {

        private readonly string _hostName;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username
            };

            _connection = factory.CreateConnection();


            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            byte[] body = GetMessageAsByteArray(message);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            // serializa a classe na qual extende do BaseMessage, no caso o PaymentDTO
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            //

            var json = JsonSerializer.Serialize<PaymentDTO>((PaymentDTO) message, options);

            var body = Encoding.UTF8.GetBytes(json);
            return body;

        }
    }
}
