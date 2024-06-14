using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeekShopping.PaymentAPI.DTOs;
using GeekShopping.PaymentAPI.RabbitMQSender.Interfaces;
using GeekShopping.PaymentProcessor;
using Microsoft.Extensions.DependencyInjection;

namespace GeekShopping.PaymentAPI.RabbitMQMessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private IRabbitMQMessageSender _rabbitMQMessageSender;
        private readonly IProcessPayment _processPayment;

        public RabbitMQPaymentConsumer(IProcessPayment processPayment, IRabbitMQMessageSender rabbitMqMessageSender)
        {
            _processPayment = processPayment;
            _rabbitMQMessageSender = rabbitMqMessageSender;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "orderpaymentprocessqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PaymentDTO payment = JsonSerializer.Deserialize<PaymentDTO>(content);
                ProcessPayment(payment).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("orderpaymentprocessqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessPayment(PaymentDTO payment)
        {

            var result = _processPayment.PaymentProcessor();
            UpdatePaymentResultDTO updatePaymentResult = new UpdatePaymentResultDTO
            {
                OrderId = payment.OrderId,
                Status = result,
                Email = payment.Email
            };


            try
            {
                // cria uma nova fila contendo os dados do UPDATE
                _rabbitMQMessageSender.SendMessage(updatePaymentResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        }
    }

