using GeekShopping.OrderAPI.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeekShopping.OrderAPI.DTOs;
using GeekShopping.OrderAPI.Entities;
using GeekShopping.OrderAPI.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeekShopping.OrderAPI.RabbitMQMessageConsumer
{
    public class RabbitMQPlaceOrderConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQPlaceOrderConsumer(OrderRepository repository)
        {
            _orderRepository = repository;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PlaceOrderDTO placeOrder = JsonSerializer.Deserialize<PlaceOrderDTO>(content);
                ProcessOrder(placeOrder).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(PlaceOrderDTO placeOrder)
        {
            var orderHeader = new OrderHeader
            {
                UserId = placeOrder.UserId,
                FirstName = placeOrder.FirstName,
                LastName = placeOrder.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = placeOrder.CardNumber,
                CouponCode = placeOrder.CouponCode,
                SecurityCode = placeOrder.SecurityCode,
                DiscountAmount = placeOrder.DiscountAmount,
                Email = placeOrder.Email,
                ExpirationDate = placeOrder.ExpirationDate,
                OrderTime = DateTime.Now,
                PurchaseAmount = placeOrder.PurchaseAmount,
                PaymentStatus = false,
                Phone = placeOrder.Phone,
                DateTime = placeOrder.DateTime
            };

            if (placeOrder.CartDetails != null)
            {
                foreach (var detail in placeOrder.CartDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.Product.Name,
                        Price = detail.Product.Price,
                        Count = detail.Count
                    };

                    orderHeader.CartTotalItens += detail.Count;
                    orderHeader.OrderDetails.Add(orderDetail);
                }
            }

            await _orderRepository.AddOrder(orderHeader);
        }
    }
}
