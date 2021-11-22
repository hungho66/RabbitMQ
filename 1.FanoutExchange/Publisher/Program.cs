//TODO: Fanout Exchange -> All Queue (Not route) -> queue -> C
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//Exchange: Fanout
string exchange = "logs-fanout";
channel.ExchangeDeclare(exchange, durable: true, type: ExchangeType.Fanout);

string message = "hello world 10";
for (int i = 100; i < 200; i++)
{
    message = i.ToString() + exchange;
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange, "", null, messageBody);
    Thread.Sleep(100);
}

Console.WriteLine("Message: Done");
Console.ReadLine();