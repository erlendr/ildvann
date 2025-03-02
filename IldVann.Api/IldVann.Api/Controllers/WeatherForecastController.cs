using System.Net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IldVann.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    
    private readonly IMongoDatabase _database;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        
        var client = new MongoClient(Globals.MongoDBConnectionString);
        _database = client.GetDatabase("weather");
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetWeatherForecasts()
    {
        var collection = _database.GetCollection<WeatherForecast>("forecasts");
        return new JsonResult(collection.Find(Builders<WeatherForecast>.Filter.Empty).ToList());
    }
    
    [HttpGet]
    [Route("{id:length(24)}")]
    public IActionResult GetWeatherForecastById(string id)
    {
        ObjectId objectId;
        try {
            objectId = ObjectId.Parse(id);
        } catch (FormatException) {
            return new BadRequestObjectResult("Invalid id");
        }
        
        var collection = _database.GetCollection<WeatherForecast>("forecasts");
        var weatherForecasts = collection.Find(Builders<WeatherForecast>.Filter.Eq("_id", objectId)).ToList();
        return new JsonResult(weatherForecasts);
    }
}