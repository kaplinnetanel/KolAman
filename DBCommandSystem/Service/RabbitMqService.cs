using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCommandSystem.Service;




public class RabbitMqService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private AsyncEventingBasicConsumer? _consumer;
    private readonly ServiceHandler _serviceHandler;
    private string _queueName = "";

    public RabbitMqService(ServiceHandler serviceHandler)
    {
        _serviceHandler = serviceHandler;
    }
    public async Task InitializeAsync(string queueName)
    {
        _queueName = queueName;
        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(_queueName, true, false, false);

        _consumer = new AsyncEventingBasicConsumer(_channel);

        _consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var success = await _serviceHandler.ProcessEventAsync(message);

            if (success)
            {
                await _channel.BasicAckAsync(
                    eventArgs.DeliveryTag,
                    false);
            }
        };
    }

    public async Task StartConsumingAsync()
    {
        if (_channel == null || _consumer == null)
        {
            throw new InvalidOperationException("RabbitMQ has not been initialized.");
        }
        await _channel.BasicConsumeAsync(_queueName, false, _consumer);
    }
}



