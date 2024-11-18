using IldVann.Api;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
Globals.MongoDBConnectionString =
    isDocker
        ? "mongodb://user:pass@mongodb:27017/?directConnection=true"
        : "mongodb://user:pass@localhost:27019/?directConnection=true";
var client = new MongoClient(Globals.MongoDBConnectionString);
var database = client.GetDatabase("weather");
var collection = database.GetCollection<WeatherForecast>("forecasts");
collection.DeleteMany(Builders<WeatherForecast>.Filter.Empty);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
{
    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    TemperatureC = Random.Shared.Next(-20, 55),
    Summary = summaries[Random.Shared.Next(summaries.Length)]
});
collection.InsertMany(weatherForecasts);


app.Run();