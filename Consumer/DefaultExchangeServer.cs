using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class DefaultExchangeServer : IExchangeServer
    {
        public void ReceiveMessage()
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

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message: {message}");
            };

            channel.BasicConsume("my-queue-name", true, consumer);

            Console.WriteLine("Receiving messages from producer");
            Console.ReadLine();
        }
    }
}

