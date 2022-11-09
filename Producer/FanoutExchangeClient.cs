using System;
using RabbitMQ.Client;

namespace Producer
{
    public class FanoutExchangeClient : IExchangeClient
    {
        public void SendMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = " guest",
                Password = "guest",
                VirtualHost = "/",
                Port = 5672
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("fanout-exchange-name", ExchangeType.Fanout);

            var message = "Hello from RabbitMQ";

            var body = System.Text.Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("fanout-exchange-name", "", null, body);
            Console.WriteLine($"Sending message to exchange: fanout-exchange-name without routing key");
        }
    }
}

