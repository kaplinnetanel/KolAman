using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationGate.Service;
using System;
using System.IO;
using System.Text.Json;
using Serilog;
using Elastic.Serilog.Sinks;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/app-.log",
        rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch(
        new[]
        {
            new Uri(
                Environment.GetEnvironmentVariable("ELASTICSEARCH_URL")
                ?? "http://localhost:9200")
        },
        options =>
        {
            options.DataStream =
                new Elastic.Ingest.Elasticsearch.DataStreams.DataStreamName(
                    "logs",
                    "NotificationGate",
                    "default");
        })
    .CreateLogger();




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









