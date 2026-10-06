using MatzpehControlComponentAPI.models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MatzpehControlComponent​​API.Repositories;

public class Reservoir : IReservoir
{

    private readonly IMongoDatabase _mongoDatabase;
    public Reservoir(IMongoDatabase db)
    {
        _mongoDatabase = db;
    }
    string[] regions =
   {
        "NORTH", "SOUTH", "CENTER", "OVERSEAS"
    };

    public  async Task<Dictionary<string, long>> Count()
    {
        var result = new Dictionary<string, long>();

        foreach (var region in regions)
        {
            var collection =  _mongoDatabase.GetCollection<Alert>(region);
            result[region] =  await collection.CountDocumentsAsync(_ => true);
        }

        return result;
    }
}