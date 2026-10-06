using Confluent.Kafka;
using NotificationGate.models;
using Serilog;
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

        watcher.Created += OnCreated;
        //ready
        watcher.Filter = "*.ready";
        //watcher.Filter = "*.json";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;


        Log.Information("Press enter to exit.");
        Console.ReadLine();
    }
    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        var _kafkaService = new ProducerService(BootstrapServers);
        var exist = File.Exists(e.FullPath);
        if (exist)
        {
            string fullPath = e.FullPath.Replace("ready", "json");
            string fileText = File.ReadAllText(fullPath);
            await _kafkaService.SendMessageAsync("Warning", null, fileText);
            Log.Information("Created send to kafka");
        }
        Log.Error("the file not ready ");

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





