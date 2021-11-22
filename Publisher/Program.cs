using System;
using RabbitMQ.Client;
using System.Text;

class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

        using var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare("hello-queue", true, false, false);

        string message = "hello world 10";
        for (int i = 100; i < 200; i++)
        {
            message = i.ToString();
            var messageBody = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
            Thread.Sleep(100);
        }


        Console.WriteLine("Message: Done");
        Console.ReadLine();
    }
}
