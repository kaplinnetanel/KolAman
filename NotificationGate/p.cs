//using Microsoft.Extensions.Configuration;
//using NotificationGate.Service;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace NotificationGate
//{
//    internal class p
//    {
//    }
//}

//var configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsetings.json", optional: false)
//    .Build();

//string bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
//while (true)
//{
//    var dataLoder = new SystemFileWatcher();
//    var WarningMessage = dataLoder.Load_Data();
//    Console.WriteLine("load jeson");

//    var kafkaService = new ProducerService(bootstrapServers);

//    string json = JsonSerializer.Serialize(WarningMessage);
//    await kafkaService.SendMessageAsync("Warning", null, json);

//}




//var _kafkaService = new ProducerService(BootstrapServers);
//string fileText = File.ReadAllText(e.FullPath);
//await _kafkaService.SendMessageAsync("Warning", null, fileText);
//Console.WriteLine("Created send to kafka");