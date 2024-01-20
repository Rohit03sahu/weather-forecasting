using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using weatherforecast.Enums;
using weatherforecast.Model.Common;
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
        public async Task<ActionResult<WeatherForecasts>> GetWeatherForecast(string Location, WeatherTimeLineEnum timeline)
        {
            WeatherForecasts weatherForecasts = new WeatherForecasts() { IsSuccess=true, reason= new List<string>() };
            if (string.IsNullOrEmpty(Location) ) { weatherForecasts.IsSuccess=false; weatherForecasts.reason.Add("Invalid Location / atleast two location should be there for delta changes"); }

            if (weatherForecasts.IsSuccess)
            {                
                weatherForecasts= await _weatherForecastProvider.FetchWeatherForecast(new WeatherForecastDto() { Location= Location, TimeLine=timeline });
            }
            return Ok(weatherForecasts);
        }



        [HttpPost]
        [Route("multilocforecastbytimeline")]
        public async Task<ActionResult<MultiLocWeatherForecast>> GetWeatherForecastWithDelta(WeatherForecastDeltaDto weatherForecastDeltaDto)
        {
            MultiLocWeatherForecast weatherForecasts = new MultiLocWeatherForecast() { IsSuccess=true, reason= new List<string>() };
            var errors = weatherForecastDeltaDto.Validate();
            if (errors == null && errors?.Count <=2) { weatherForecasts.IsSuccess=false; weatherForecasts.reason.Add("Invalid Location / atleast two location should be there for delta changes"); }

            if (weatherForecasts.IsSuccess)
            {
                weatherForecasts= await _weatherForecastProvider.FetchMultiLocWeatherForecast(weatherForecastDeltaDto);
            }
            return Ok(weatherForecasts);
        }

        [HttpGet]
        [Route("location")]
        public async Task<ActionResult<LocationResponse>> GetLocation()
        {
            var locations = await _weatherForecastProvider.GetLocations();
            return Ok(locations);
        }

    }
}