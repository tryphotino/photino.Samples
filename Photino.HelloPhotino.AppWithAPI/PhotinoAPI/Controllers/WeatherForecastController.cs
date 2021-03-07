using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PhotinoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly Dictionary<string, string> Forecasts = new Dictionary<string, string>
        {
            {"Thunderstorms", "fas fa-bolt"},
            {"Cloudy", "fas fa-cloud"},
            {"Meatballs", "fas fa-cloud-meatball"},
            {"Overcast", "fas fa-cloud-sun"},
            {"Heavy Rain", "fas fa-cloud-showers-heavy"},
            {"Showers", "fas fa-cloud-sun-rain"},
            {"Apocalypse", "fas fa-meteor"},
            {"Sunny", "fas fa-sun"},
            {"Snow", "fas fa-snowflake"},
            {"Freezing", "fas fa-temperature-low"},
            {"Floods", "fas fa-water"}
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            return Enumerable
                .Range(1, 5)
                .Select(index => new {
                    Index = index,
                    Random = rng.Next(Forecasts.Count)
                })
                .Select(numbers => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(numbers.Index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Forecasts.ElementAt(numbers.Random).Key,
                    Icon = Forecasts.ElementAt(numbers.Random).Value
                })
                .ToArray();
        }
    }
}
