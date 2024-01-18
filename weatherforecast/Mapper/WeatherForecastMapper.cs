﻿using weatherforecast.Model.Common;
using weatherforecast.Model.ResponseDto;
using weatherforecast.Seed;

namespace weatherforecast.Mapper
{
    public class WeatherForecastMapper
    {

        public static WeatherForecasts mapForecastData(Weather weatherData,string location)
        {
            WeatherForecasts weatherforecasts = new WeatherForecasts()
            {
                Data= new List<WeatherForecast>()
            };
            weatherData.data.timelines.FirstOrDefault().intervals.ForEach(x=> weatherforecasts.Data.Add(new WeatherForecast() { Location= location, TemperatureInC=x.values.temperature, TemperatureInF= convertCelToF(x.values.temperature), TimeStamp=x.startTime}));
            weatherforecasts.IsSuccess = true;
            return weatherforecasts;
        }

        public static List<LocResponse> mapLocationSeedData(List<LocationSeedData> locationSeedDatas)
        {
            List<LocResponse> locResponses = new List<LocResponse>();
            locationSeedDatas.ForEach(x => locResponses.Add(new LocResponse() { name=x.Name, latLong= x.Coordinates[0]+","+x.Coordinates[1] }));
            return locResponses;
        }
        private static double convertCelToF(double celsius)
        {
            var value = (9/5 * celsius) + 32;
            return value;
        }

    }
}
