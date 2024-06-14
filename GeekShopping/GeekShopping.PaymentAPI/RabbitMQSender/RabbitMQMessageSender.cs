using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.PaymentAPI.DTOs;
using GeekShopping.PaymentAPI.RabbitMQSender.Interfaces;
using RabbitMQ.Client;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {

        private readonly string _hostName;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private const string _exchangeDirect = "DirectPaymentUpdateExchange";
        private const string _paymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
        private const string _paymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public void SendMessage(BaseMessage message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username
            };

            _connection = factory.CreateConnection();


            using var channel = _connection.CreateModel();

            // utilizando exchange do tipo fanout
            channel.ExchangeDeclare(_exchangeDirect, ExchangeType.Direct, false);


            // CONFIGURANDO AS FILAS PARA EMAIL E ORDER
            channel.QueueDeclare(queue: _paymentEmailUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: _paymentOrderUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // bind na fila
            channel.QueueBind(queue: _paymentEmailUpdateQueueName, exchange: _exchangeDirect, routingKey: "PaymentEmail");
            channel.QueueBind(queue: _paymentOrderUpdateQueueName, exchange: _exchangeDirect, routingKey: "PaymentOrder");

            byte[] body = GetMessageAsByteArray(message);

            // publica a mensagem no exchange
            channel.BasicPublish(exchange: _exchangeDirect, routingKey: "PaymentEmail", basicProperties: null, body: body);
            channel.BasicPublish(exchange: _exchangeDirect, routingKey: "PaymentOrder", basicProperties: null, body: body);

        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            // serializa a classe na qual extende do BaseMessage, no caso o PaymentDTO
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            //

            var json = JsonSerializer.Serialize<UpdatePaymentResultDTO>((UpdatePaymentResultDTO) message, options);

            var body = Encoding.UTF8.GetBytes(json);
            return body;

        }
    }
}
