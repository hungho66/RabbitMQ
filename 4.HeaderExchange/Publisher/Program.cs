using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//Exchange: Header: format = pdf, x-match = all or x-match = any 
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("Header done"));

Console.WriteLine("Message: Done");
Console.ReadLine();