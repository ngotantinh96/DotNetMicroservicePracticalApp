
using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IEventProcessor eventProcessor;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        this.eventProcessor = eventProcessor;

        InitializeRabbitMQ();
    }

    public void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"],
            Port = int.Parse(_configuration["RabbitMQ:Port"])
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

        Console.WriteLine("---> Listening of the Message Bus...");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (moduleHandle, eventArgs) => 
        {
            Console.WriteLine("---> Event Received!");
            
            var body = eventArgs.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        Console.WriteLine("---> MessageBus disposed");
        if(_connection.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("---> RabbitMQ Connection Shutdown");
    }
}