using Confluent.Kafka;
using NotificationGate.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationGate.Service;

public class SystemFileWatcher
{
    public string BootstrapServers;

    public SystemFileWatcher(string bootstrapServers)
    {

        BootstrapServers = bootstrapServers;
     }
       
    public void Load_Data()

    {
        using var watcher = new FileSystemWatcher(@"C:\Users\User\Desktop\KolAman\alert-simulator\alerts");

        watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Filter = "*.json";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Press enter to exit.");
        var jsonString = Console.Read();
    }
    private async void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }
        var kafkaService = new ProducerService(BootstrapServers);
        await kafkaService.SendMessageAsync("Warning", null, e.FullPath);
        Console.WriteLine("Changed: send to kafka");
    }
    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        var kafkaService = new ProducerService(BootstrapServers);
        await kafkaService.SendMessageAsync("Warning", null,  e.FullPath);
        Console.WriteLine("Created send to kafka");

    }
    private static void PrintException(Exception? ex)
    {
        if (ex != null)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }
}




