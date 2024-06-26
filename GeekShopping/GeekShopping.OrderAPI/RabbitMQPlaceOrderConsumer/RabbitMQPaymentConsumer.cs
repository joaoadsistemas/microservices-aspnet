﻿using GeekShopping.OrderAPI.Repositories.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeekShopping.OrderAPI.DTOs;
using GeekShopping.OrderAPI.Entities;
using GeekShopping.OrderAPI.RabbitMQSender.Interfaces;
using GeekShopping.OrderAPI.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeekShopping.OrderAPI.RabbitMQMessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;
        private const string _exchangeFanout = "DirectPaymentUpdateExchange";
        private const string _paymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQPaymentConsumer(OrderRepository repository)
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

            // utilizando exchange do tipo fanout
            _channel.ExchangeDeclare(_exchangeFanout, ExchangeType.Direct);

            // criando a fila 
            _channel.QueueDeclare(queue: _paymentOrderUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);


            // bind na fila
            _channel.QueueBind(queue: _paymentOrderUpdateQueueName, exchange: _exchangeFanout, routingKey: "PaymentOrder");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResultDTO updatePayment = JsonSerializer.Deserialize<UpdatePaymentResultDTO>(content);
                UpdatePaymentStatus(updatePayment).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(_paymentOrderUpdateQueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResultDTO updatePayment)
        {
            

            try
            {

                await _orderRepository.UpdateOrderPaymentStatus(updatePayment.OrderId, updatePayment.Status);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        }
    }

