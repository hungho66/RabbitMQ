//TODO: Topic Exchange (routing key: Critical.Error.Warning)  -> Queue -> Consume
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//Exchange: Topic (routing key: Critical.Error.Warning) 
// Publish  (routing key: Critical.Error.Warning)  -> TopicExchange -> *.Error.* (Queue), #.Error -> Consume
channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

string message = "hello world 10";
for (int i = 100; i < 200; i++)
{
    LogNames log1 = (LogNames)new Random().Next(1, 5);
    LogNames log2 = (LogNames)new Random().Next(1, 5);
    LogNames log3 = (LogNames)new Random().Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";
    message = i.ToString() + log1 + "--" + log2 + "--" + log3;
    Console.WriteLine(message);
    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-topic", routeKey, null, messageBody);
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