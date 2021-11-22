//TODO: Drirect Exchange (warning-route | info-route) -> Queue -> Consume
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//Exchange: Direct
channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

LogNames logs = (LogNames)new Random().Next(1, 5);
Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    //TODO: routeKey
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);

    channel.QueueBind(queueName, "logs-direct", routeKey, null);
});

string message = "hello world 10";
for (int i = 100; i < 200; i++)
{
    message = i.ToString() + logs;
    var messageBody = Encoding.UTF8.GetBytes(message);
    var routeKey = $"route-{logs}";
    channel.BasicPublish("logs-direct", routeKey, null, messageBody);
    Thread.Sleep(100);
}

Console.WriteLine("Message: Done");
Console.ReadLine();

public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Infor = 4
}