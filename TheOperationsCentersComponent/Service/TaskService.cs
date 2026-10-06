
using MongoDB.Driver;
using TheOperationsCentersComponent.models;

namespace DBCommandSystem.Service;

public class TaskService
{
    private readonly IMongoDatabase _database;

    public TaskService(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task HandleAlerts()
    {
        string[] regions =
        {
            "NORTH", "SOUTH", "CENTER", "OVERSEAS"
        };

        while (true)
        {
            try
            {
                foreach (var region in regions)
                {
                    var collection = _database.GetCollection<Alert>(region);

                    var alerts = await collection
                        .Find(x => x.Status == "WAITING")
                        .ToListAsync();

                    foreach (var alert in alerts)
                    {
                        if (alert.Priority != "CRITICAL")
                        {
                            alert.Status = "CANCELED";

                            await collection.ReplaceOneAsync(
                                x => x.Alert_id == alert.Alert_id,
                                alert);
                        }
                        else
                        {
                            alert.Status = "INPROGRESS";

                            await collection.ReplaceOneAsync(
                                x => x.Alert_id == alert.Alert_id,
                                alert);

                            await Task.Delay(5000);

                            alert.Status = "DONE";

                            await collection.ReplaceOneAsync(
                                x => x.Alert_id == alert.Alert_id,
                                alert);
                        }

                        Console.WriteLine($"{region} - {alert.Alert_id} - {alert.Status}");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error occurred: {ex.Message}"); }
            await Task.Delay(10000);
        }
    }
}

