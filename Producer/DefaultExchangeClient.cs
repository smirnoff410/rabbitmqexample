using System;
using RabbitMQ.Client;

namespace Producer
{
    public class DefaultExchangeClient : IExchangeClient
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

            channel.QueueDeclare("my-queue-name", false, false, false, null);

            var message = "Hello from RabbitMQ";

            var body = System.Text.Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "my-queue-name", null, body);
            Console.WriteLine("Sending message to my-queue-name");
        }
    }
}

