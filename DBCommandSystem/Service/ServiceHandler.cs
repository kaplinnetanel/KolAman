using DBCommandSystem.Data;
using DBCommandSystem.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NotificationGate.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DBCommandSystem.Service;

public class ServiceHandler
{
    private readonly IMongoDatabase _mongoDatabase;
    private string _queueName = "";
    public ServiceHandler(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;    }
    public async Task<bool> ProcessEventAsync(string message,string queueName)
    {
        _queueName = queueName;
        var collection = _mongoDatabase.GetCollection<Alert>(_queueName);
        try
        { 
            var alert = JsonSerializer.Deserialize<Alert>(message);
            if (alert == null)
            {
                return false;
            } 
            if (string.IsNullOrEmpty(alert.Alert_id)
                || string.IsNullOrEmpty(alert.Source) 
                || string.IsNullOrEmpty(alert.Title) 
                || string.IsNullOrEmpty(alert.Content)
                || string.IsNullOrEmpty(alert.Priority) 
                || string.IsNullOrEmpty(alert.Classification)
                || string.IsNullOrEmpty(alert.Status)) 
            {
                return false; 
            }
            if (alert.Lat < -90 || alert.Lat > 90)
              return false;

            if (alert.Lon < -180 || alert.Lon > 180) 
                return false;

            //var alertDB = new AlertDB
            //{
            //     Alert_id = alert.Alert_id,
            //    Source = alert.Source,
            //    Title  = alert.Title,
            //    Content = alert.Content,
            //    Priority =alert.Priority,
            //    Classification = alert.Classification,
            //    Lat =alert.Lat,
            //    Lon = alert.Lon,
            //    Timestamp = alert.Timestamp,
            //    Status = alert.Status,
            //    Command = _queueName
            //};
            await collection.InsertOneAsync(alert);
            //_context.alertDbs.Add(alertDB);
            //await _context.SaveChangesAsync(); 

            return true; } 
        catch
        {
            return false;
        }
    }
}



