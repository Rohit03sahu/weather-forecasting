using Newtonsoft.Json;
using System.Collections.Generic;
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
            Dictionary<string, List<WeatherForecast>> keyValuePairs = new Dictionary<string, List<WeatherForecast>>();
            foreach (var loc in request.Locations)
            {

                var response = await _weatherRepository.FetchWeatherDetailsByLocations(new List<string>() { loc });
                var forecastString = await response.Content.ReadAsStringAsync();
                List<WeatherForecast> forecasts = new List<WeatherForecast>();
                if (response != null && !string.IsNullOrEmpty(forecastString))
                {
                    var forecastJson = JsonConvert.DeserializeObject<Weather>(forecastString);
                    
                    if (request.TimeLine.ToString()== WeatherTimeLineEnum.Minutely.ToString())
                        forecasts= WeatherForecastMapper.MapMinutelyForecast(forecastJson);

                    if (request.TimeLine.ToString()== WeatherTimeLineEnum.Hourly.ToString())
                        forecasts= WeatherForecastMapper.MapMinutelyForecast(forecastJson);

                    if (request.TimeLine.ToString()== WeatherTimeLineEnum.Daily.ToString())
                        forecasts= WeatherForecastMapper.MapMinutelyForecast(forecastJson);
                }
                keyValuePairs.Add(loc, forecasts);
            }

            var weatherforecasts= await getDeltaChanges(keyValuePairs[request.Locations[0]], keyValuePairs[request.Locations[1]]);

            if (weatherforecasts.Source == null && weatherforecasts.Source.Count<=0 )
            {
                weatherforecasts.IsSuccess= true;
                weatherforecasts.reason=new List<string>() { "No data found for source"};
            }
            if (weatherforecasts.Destination == null && weatherforecasts.Destination.Count<=0)
            {
                weatherforecasts.IsSuccess= true;
                weatherforecasts.reason=new List<string>() { "No data found for destination" };
            }

            return weatherforecasts;

        }

        public async Task<WeatherForecasts> getDeltaChanges(List<WeatherForecast> source, List<WeatherForecast> destination)
        {
            WeatherForecasts weatherforecasts = new WeatherForecasts() 
            { 
                    Source= source, 
                    Delta= new List<WeatherForecast>(),
                    Destination= destination, 
                    IsSuccess=true
            };

            var deltaChanges = from s in source
                               join r in destination on s.Location equals r.Location
                               where s.TimeStamp.Value == r.TimeStamp.Value
                               select new { loc= s.Location, timeStamp=s.TimeStamp, deltaC= s.TempratureInC - r.TempratureInC, deltaF= s.TempratureInF - r.TempratureInF };
            foreach (var s in deltaChanges ) {
                weatherforecasts.Delta.Add( new WeatherForecast() { Location= s.loc, TempratureInC=s.deltaC, TempratureInF= s.deltaF, TimeStamp=s.timeStamp });
            }
            return weatherforecasts;
        }
    }
}
