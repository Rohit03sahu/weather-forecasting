using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using weatherforecast.Enums;
using weatherforecast.Mapper;
using weatherforecast.Model.Common;
using weatherforecast.Model.RequestDto;
using weatherforecast.Model.ResponseDto;
using weatherforecast.Repository;
using weatherforecast.Seed;

namespace weatherforecast.Provider
{
    public class WeatherForecastProvider : IWeatherForecastProvider
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly IMemoryCache _memoryCache;
        public WeatherForecastProvider(IWeatherRepository weatherRepository, IMemoryCache memoryCache)
        {
            _weatherRepository = weatherRepository;
            _memoryCache = memoryCache;

        }
        public async Task<WeatherForecastWithDelta> FetchDeltaWeatherForecast(WeatherForecastWithDeltaDto request)
        {
            Dictionary<string, Weather> keyValuePairs = new Dictionary<string, Weather>();
            foreach (var loc in request.Locations)
            {
                string timeLineType = "";
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Minutely.ToString())
                    timeLineType = "1m";
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Hourly.ToString())
                    timeLineType = "1h";
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Daily.ToString())
                    timeLineType = "1d";

                var response = await _weatherRepository.FetchWeatherDetailsByLocations(loc, timeLineType);
                var forecastString = await response.Content.ReadAsStringAsync();
                Weather weatherForecast = new Weather();
                if (response != null && !string.IsNullOrEmpty(forecastString))
                {
                    weatherForecast = JsonConvert.DeserializeObject<Weather>(forecastString);
                }
                keyValuePairs.Add(loc, weatherForecast);
            }

            var resp = WeatherForecastMapper.mapDeltaForecastData(keyValuePairs[request.Locations[0]], keyValuePairs[request.Locations[1]], request.Locations[0], request.Locations[1]);
            if (resp == null)
            {
                resp.IsSuccess= false;
                resp.reason=new List<string>() { "No data found for source" };
            }
            return resp;
        }


        public async Task<WeatherForecasts> FetchWeatherForecast(WeatherForecastDto request)
        {
            Dictionary<string, Weather> keyValuePairs = new Dictionary<string, Weather>();
            
                string timeLineType = "";
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Minutely.ToString())
                    timeLineType = "1m";
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Hourly.ToString())
                    timeLineType = "1h";
                if (request.TimeLine.ToString()== WeatherTimeLineEnum.Daily.ToString())
                    timeLineType = "1d";

                var response = await _weatherRepository.FetchWeatherDetailsByLocations(request.Location, timeLineType);
                var forecastString = await response.Content.ReadAsStringAsync();
                Weather weatherForecast = new Weather();
                if (response != null && !string.IsNullOrEmpty(forecastString))
                {
                    weatherForecast = JsonConvert.DeserializeObject<Weather>(forecastString);
                }
                keyValuePairs.Add(request.Location, weatherForecast);

            var weatherforecasts = WeatherForecastMapper.mapForecastData(keyValuePairs[request.Location], request.Location);
            if (weatherforecasts == null)
            {
                weatherforecasts.IsSuccess= false;
                weatherforecasts.reason=new List<string>() { "No data found for source" };
            }
            return weatherforecasts;
        }

        public async Task<LocationResponse> GetLocations()
        {
            LocationResponse locationResponse = new LocationResponse() { locResponse= new List<LocResponse>() };
            var resp = _memoryCache.Get<List<LocResponse>>("locationData");
            if (resp == null)
            {
                var locSeedData = this.GetSeedData<LocationSeedData>();
                locationResponse.locResponse = WeatherForecastMapper.mapLocationSeedData(locSeedData);
                //var response = await _weatherRepository.GetLocationData();
                //var locData = await response.Content.ReadAsStringAsync();

                //if (!string.IsNullOrEmpty(locData))
                //{
                //    var locationJson = JsonConvert.DeserializeObject<LocationMetaData>(locData);
                //    locationJson.data.locations
                //        .ForEach(x => locationResponse.locResponse
                //        .Add(
                //            new LocResponse()
                //            {
                //                name=x.name,
                //                latLong= x.geometry.coordinates[0].ToString()+","+x.geometry.coordinates[0].ToString()
                //            }));
                //    _memoryCache.Set("locationData", locationResponse.locResponse, TimeSpan.FromMinutes(30));
                //}
            }
            else
                locationResponse.locResponse=resp;
            return locationResponse;
        }

        private List<T> GetSeedData<T>()
        {
            return JsonConvert.DeserializeObject<List<T>>(ReadFile(typeof(T).Name+".json"));
        }
        private string ReadFile(string fileName)
        {
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var dataFilePath = applicationPath + "Seed\\" + "location.json";
            return File.ReadAllText(dataFilePath);
        }
    }
}
