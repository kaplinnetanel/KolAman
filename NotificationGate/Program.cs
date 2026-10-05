using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationGate.Service;
using System;
using System.IO;
using System.Text.Json;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsetings.json", optional: false)
    .Build();

string bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
while (true)
{
    var dataLoder = new SystemFileWatcher(bootstrapServers);
    dataLoder.Load_Data();

}




