using RabbitMQ.Client;

namespace Producer
{
    public class DirectExchangeClient : IExchangeClient
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

            channel.ExchangeDeclare("direct-exchange-name", ExchangeType.Direct);

            var message = "Hello from RabbitMQ";

            var body = System.Text.Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("direct-exchange-name", "student", null, body);
            Console.WriteLine($"Sending message to exchange: direct-exchange-name and routing key: student");
        }
    }
}

