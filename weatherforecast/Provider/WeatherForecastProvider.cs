using Newtonsoft.Json;
using weatherforecast.Enums;
using weatherforecast.Mapper;
using weatherforecast.Model.Common;
using weatherforecast.Model.RequestDto;
using weatherforecast.Model.ResponseDto;
using weatherforecast.Repository;

namespace weatherforecast.Provider
{
    public class WeatherForecastProvider : IWeatherForecastProvider
    {
        private readonly IWeatherRepository _weatherRepository;
        public WeatherForecastProvider(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;

        }
        public async Task<WeatherForecasts> FetchWeatherForecast(WeatherForecastDto request)
        {
            WeatherForecasts weatherforecasts = new WeatherForecasts() { WeatherForecast= new List<WeatherForecast>(), IsSuccess=true };

            var response = await _weatherRepository.FetchWeatherDetailsByLocations(request.Locations);
            var forecastString = await response.Content.ReadAsStringAsync();
            if (response != null && !string.IsNullOrEmpty(forecastString))
            {
                var forecastJson = JsonConvert.DeserializeObject<Weather>(forecastString);
                
                if(request.TimeLine.ToString()== WeatherTimeLineEnum.Minutely.ToString())
                    weatherforecasts= WeatherForecastMapper.MapMinutelyForecast(forecastJson);
                
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Hourly.ToString())
                    weatherforecasts= WeatherForecastMapper.MapMinutelyForecast(forecastJson);

                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Daily.ToString())
                    weatherforecasts= WeatherForecastMapper.MapMinutelyForecast(forecastJson);
            }

            if (weatherforecasts.WeatherForecast == null && weatherforecasts.WeatherForecast.Count<=0)
            {
                weatherforecasts.IsSuccess= true;
                weatherforecasts.reason=new List<string>() { "No data found"};
            }

            return weatherforecasts;

        }
    }
}
