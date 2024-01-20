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
        public async Task<MultiLocWeatherForecast> FetchMultiLocWeatherForecast(WeatherForecastDeltaDto request)
        {
            Dictionary<string, Weather> keyValuePairs = new Dictionary<string, Weather>();
            foreach (var loc in request.Locations)
            {
                string timeLineType = timeLineType=getTimeLineType(request.TimeLine.ToString());

                var response = await _weatherRepository.FetchWeatherDetailsByLocations(loc, timeLineType);
                var forecastString = await response.Content.ReadAsStringAsync();
                Weather weatherForecast = new Weather();
                if (response != null && !string.IsNullOrEmpty(forecastString))
                {
                    weatherForecast = JsonConvert.DeserializeObject<Weather>(forecastString);
                }
                keyValuePairs.Add(await GetLocationNameRespectToLatLong(loc), weatherForecast);
            }

            var intermediateResp = WeatherForecastMapper.mapMultiLocForecastData(keyValuePairs);
            var resp = buildDeltaData(intermediateResp);
            if (resp == null)
            {
                resp.IsSuccess= false;
                resp.reason=new List<string>() { "No data found for source" };
            }
            return resp;
        }


        public async Task<WeatherForecasts> FetchWeatherForecast(WeatherForecastDto request)
        {
            string timeLineType = timeLineType=getTimeLineType(request.TimeLine.ToString());

            var response = await _weatherRepository.FetchWeatherDetailsByLocations(request.Location, timeLineType);
            var forecastString = await response.Content.ReadAsStringAsync();
            Weather weatherForecast = new Weather();
            if (response != null && !string.IsNullOrEmpty(forecastString))
            {
                weatherForecast = JsonConvert.DeserializeObject<Weather>(forecastString);
            }
            var weatherforecasts = WeatherForecastMapper.mapForecastData(weatherForecast, await GetLocationNameRespectToLatLong(request.Location));
            weatherforecasts.MaxTempInC=weatherforecasts.Data.Select(x=> Convert.ToDouble(x.TemperatureInC)).Max().ToString();
            weatherforecasts.MinTempInC=weatherforecasts.Data.Select(x => Convert.ToDouble(x.TemperatureInC)).Min().ToString();
            weatherforecasts.AvgTempInC=weatherforecasts.Data.Select(x => Convert.ToDouble(x.TemperatureInC)).Average().ToString();
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
                _memoryCache.Set("locationData", locationResponse.locResponse, TimeSpan.FromMinutes(30));
            }
            else
                locationResponse.locResponse=resp;
            return locationResponse;
        }

        public async Task<string> GetLocationNameRespectToLatLong(string latLong)
        {
            var resp = await this.GetLocations();            
            return resp.locResponse.Where(x=> x.latLong==latLong).Select(z=> z.name).FirstOrDefault();
        }

        private string getTimeLineType(string timeLine)
        {
            string timeLineType = "";
            if (timeLine.ToString()== WeatherTimeLineEnum.Minutely.ToString())
                timeLineType = "1m";
            if (timeLine.ToString()== WeatherTimeLineEnum.Hourly.ToString())
                timeLineType = "1h";
            if (timeLine.ToString()== WeatherTimeLineEnum.Daily.ToString())
                timeLineType = "1d";
            return timeLineType;
        }

        private MultiLocWeatherForecast buildDeltaData(MultiLocWeatherForecast multiLocWeatherForecast)
        {
            var primaryLocationData = multiLocWeatherForecast.Data.FirstOrDefault()?.LocationForecasts;
            for (int i = 1; i < multiLocWeatherForecast.Data.Count; i++)
            {
                var IntermediateObj= multiLocWeatherForecast.Data[i].LocationForecasts;

                foreach (var item in multiLocWeatherForecast.Data[i].LocationForecasts)
                {
                    var deltaC= primaryLocationData.Where(x => x.TimeStamp?.ToString("yyyy-MM-dd")==item.TimeStamp?.ToString("yyyy-MM-dd")).Select(x => x.TemperatureInC).FirstOrDefault();
                    item.deltaInC= deltaC-item.TemperatureInC;
                }      
            }

            return multiLocWeatherForecast;
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
