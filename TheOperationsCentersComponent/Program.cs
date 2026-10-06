using DBCommandSystem.Service;
using MongoDB.Driver;



var mongoAddress = Environment.GetEnvironmentVariable("MONGO_CONNECTION") ?? "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoAddress);
var mongoDatabase = mongoClient.GetDatabase("Alerts");

Console.WriteLine("Start to send Task Service (Running continuously)...");

var service = new TaskService(mongoDatabase);
await service.HandleAlerts();



