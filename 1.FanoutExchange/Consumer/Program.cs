using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
//channel.QueueDeclare("hello-queue", true, false, false);

// string message = "hello world";
// var messageBody = Encoding.UTF8.GetBytes(message);
// channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

var randomeQueueName = channel.QueueDeclare().QueueName;
channel.QueueBind(randomeQueueName, "logs-fanout", "", null);

var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(randomeQueueName, false, consumer);
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine($"Message: {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();