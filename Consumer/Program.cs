using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

class Consumer
{
    public static void Main()
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

        using var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        //channel.QueueDeclare("hello-queue", true, false, false);

        // string message = "hello world";
        // var messageBody = Encoding.UTF8.GetBytes(message);
        // channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

        channel.BasicQos(0, 5, false);
        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume("hello-queue", false, consumer);
        consumer.Received += (object sender, BasicDeliverEventArgs e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            Console.WriteLine($"Message: {message}");
            channel.BasicAck(e.DeliveryTag, false);
        };

        Console.ReadLine();
    }
}
