using DBCommandSystem.Service;
using MongoDB.Driver;

var mongoAddress =
Environment.GetEnvironmentVariable("MONGO_CONNECTION")
?? "mongodb://localhost:27017";

var mongoClient = new MongoClient(mongoAddress);

var mongoDatabase = mongoClient.GetDatabase("Alerts");

var serviceHandler = new ServiceHandler(mongoDatabase);

var north = new RabbitMqService(serviceHandler);
var south = new RabbitMqService(serviceHandler);
var center = new RabbitMqService(serviceHandler);
var overseas = new RabbitMqService(serviceHandler);

await north.InitializeAsync("NORTH");
await south.InitializeAsync("SOUTH");
await center.InitializeAsync("CENTER");
await overseas.InitializeAsync("OVERSEAS");

await north.StartConsumingAsync();
await south.StartConsumingAsync();
await center.StartConsumingAsync();
await overseas.StartConsumingAsync();

Console.WriteLine("All command systems are running.");

await Task.Delay(Timeout.Infinite);


      