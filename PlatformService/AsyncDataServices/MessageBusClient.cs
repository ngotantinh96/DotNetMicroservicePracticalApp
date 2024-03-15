using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:Host"],
            Port = int.Parse(_configuration["RabbitMQ:Port"])
        };

        try 
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            Console.WriteLine("---> Connected to MessageBus");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"---> Could not connect to the MessageBus: {ex.Message}");
        }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("---> RabbitMQ Connection Shutdown");
    }

    public void Dispose()
    {
        Console.WriteLine("---> MessageBus disposed");
        if(_connection.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        if(_connection.IsOpen)
        {
            Console.WriteLine("---> RabbitMQ Connection is Open, sending message");
            var message = JsonSerializer.Serialize(platformPublishedDto);
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("---> RabbitMQ Connection is Closed, not send message");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body);
        Console.WriteLine($"---> We have sent {message}");
    }
}