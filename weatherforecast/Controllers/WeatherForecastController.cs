using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using weatherforecast.Enums;
using weatherforecast.Model.RequestDto;
using weatherforecast.Model.ResponseDto;
using weatherforecast.Provider;

namespace weatherforecast.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastProvider _weatherForecastProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastProvider weatherForecastProvider)
        {
            _logger = logger;
            _weatherForecastProvider= weatherForecastProvider;
        }

        [HttpGet]
        [Route("forecastbytimeline")]
        public async Task<ActionResult<WeatherForecasts>> GetWeatherForecast(string location, WeatherTimeLineEnum timeline)
        {
            WeatherForecasts weatherForecasts = new WeatherForecasts() { IsSuccess=true, reason= new List<string>() };
            if (string.IsNullOrEmpty(location)) { weatherForecasts.IsSuccess=false; weatherForecasts.reason.Add("Invalid Location"); }

            if (weatherForecasts.IsSuccess)
            {                
                weatherForecasts= await _weatherForecastProvider.FetchWeatherForecast(new WeatherForecastDto() { Locations= new List<string>() { location }, TimeLine=timeline });
            }
            return Ok(weatherForecasts);
        }
    }
}