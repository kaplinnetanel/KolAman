using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationGate.Service;

public class ProducerService
{


    private readonly IProducer<string, string> _producer;
    public ProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task SendMessageAsync(string topicName, string? key, string massegeValue)
    {
        var message = new Message<string, string>
        {
            Key = key,
            Value = massegeValue
        };
        var result = await _producer.ProduceAsync(topicName, message);
        Console.WriteLine("send  the  topic Warning");
    }
}



  
