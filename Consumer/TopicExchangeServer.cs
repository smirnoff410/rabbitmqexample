using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class TopicExchangeServer : IExchangeServer
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

            var exchangeName = "topic-exchange-name";
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            #region student routing key
            var queueStudentName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueStudentName,
                exchange: exchangeName,
                routingKey: "student.#");

            var consumerStudent = new EventingBasicConsumer(channel);

            consumerStudent.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message from student: {message}");
            };

            channel.BasicConsume(queueStudentName, true, consumerStudent);
            #endregion

            #region woman routing key

            var queueWomanName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueWomanName,
                exchange: exchangeName,
                routingKey: "*.woman.#");

            var consumerWoman = new EventingBasicConsumer(channel);

            consumerWoman.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message from woman: {message}");
            };

            channel.BasicConsume(queueWomanName, true, consumerWoman);

            #endregion

            #region vstu roution key

            var queueVSTUName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueVSTUName,
                exchange: exchangeName,
                routingKey: "#.vstu");

            var consumerVSTU = new EventingBasicConsumer(channel);

            consumerVSTU.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Receive message from VSTU: {message}");
            };

            channel.BasicConsume(queueVSTUName, true, consumerVSTU);
            #endregion

            Console.WriteLine("Subscribe to direct exchange, routing key: info-key");

            Console.ReadLine();
        }
    }
}

