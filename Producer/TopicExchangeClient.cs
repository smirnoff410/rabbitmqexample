using System;
using RabbitMQ.Client;

namespace Producer
{
    public class TopicExchangeClient : IExchangeClient
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

            var exchangeName = "topic-exchange-name";
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            var message1 = "Hello from RabbitMQ. I'm woman and student of VSTU";
            var body1 = System.Text.Encoding.UTF8.GetBytes(message1);

            channel.BasicPublish(exchangeName, "student.woman.vstu", null, body1);
            Console.WriteLine($"Sending message to exchange: {exchangeName} and routing key: student.woman.vstu");

            var message2 = "Hello from RabbitMQ. I'm man and student of VSTU";
            var body2 = System.Text.Encoding.UTF8.GetBytes(message2);

            channel.BasicPublish(exchangeName, "student.man.vstu", null, body2);
            Console.WriteLine($"Sending message to exchange: {exchangeName} and routing key: student.man.vstu");

            var message3 = "Hello from RabbitMQ. I'm man and еуфсрук of VSU";
            var body3 = System.Text.Encoding.UTF8.GetBytes(message3);

            channel.BasicPublish(exchangeName, "teacher.man.vsu", null, body3);
            Console.WriteLine($"Sending message to exchange: {exchangeName} and routing key: teacher.man.vsu");
        }
    }
}

