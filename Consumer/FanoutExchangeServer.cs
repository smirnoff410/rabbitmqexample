using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class FanoutExchangeServer : IExchangeServer
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

            channel.ExchangeDeclare("fanout-exchange-name", ExchangeType.Fanout);

            #region first queue

            var queueFirstName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueFirstName,
                exchange: "fanout-exchange-name",
                routingKey: "");

            var consumerFirst = new EventingBasicConsumer(channel);

            consumerFirst.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message from first queue: {message}");
            };

            channel.BasicConsume(queueFirstName, true, consumerFirst);

            #endregion

            #region second queue
            var queueSecondName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueSecondName,
                exchange: "fanout-exchange-name",
                routingKey: "");

            var consumerSecond = new EventingBasicConsumer(channel);

            consumerSecond.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message from second queue: {message}");
            };

            channel.BasicConsume(queueSecondName, true, consumerSecond);
            #endregion

            Console.WriteLine("Subscribe to fanout exchange");

            Console.ReadLine();
        }
    }
}

