using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeekShopping.Email.DTOs;
using GeekShopping.Email.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeekShopping.Email.RabbitMQMessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly EmailRepository _emailRepository;
        private IConnection _connection;
        private IModel _channel;
        private const string _exchangeFanout = "DirectPaymentUpdateExchange";
        private const string _paymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";

        public RabbitMQPaymentConsumer(EmailRepository repository)
        {
            _emailRepository = repository;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // utilizando exchange do tipo fanout
            _channel.ExchangeDeclare(_exchangeFanout, ExchangeType.Direct);

            // criando a fila 
            _channel.QueueDeclare(queue: _paymentEmailUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);


            // bind na fila
            _channel.QueueBind(queue: _paymentEmailUpdateQueueName, exchange: _exchangeFanout, routingKey: "PaymentEmail");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                ProcessLogsDTOs updatePayment = JsonSerializer.Deserialize<ProcessLogsDTOs>(content);
                ProcessLogs(updatePayment).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(_paymentEmailUpdateQueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessLogs(ProcessLogsDTOs message)
        {
            try
            {

                await _emailRepository.SendEmailAsync(message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        }
    }

