using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

// string message = "hello world";
// var messageBody = Encoding.UTF8.GetBytes(message);
// channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new Dictionary<string, object>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "all"); //all - any | any not match all properties (header)

channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

channel.BasicConsume(queueName, false, consumer);
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine($"Message: {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();