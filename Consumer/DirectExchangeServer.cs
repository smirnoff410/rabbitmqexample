using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class DirectExchangeServer : IExchangeServer
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

            channel.ExchangeDeclare("direct-exchange-name", ExchangeType.Direct);

            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                exchange: "direct-exchange-name",
                routingKey: "student");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message: {message}");
            };

            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine("Subscribe to direct exchange, routing key: student");

            Console.ReadLine();
        }
    }
}

