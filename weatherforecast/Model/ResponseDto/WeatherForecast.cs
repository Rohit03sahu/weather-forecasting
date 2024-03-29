﻿using weatherforecast.Model.Common;

namespace weatherforecast.Model.ResponseDto
{
    public class WeatherForecasts : BaseResponse
    {
        public string MinTempInC { get; set; }
        public string MaxTempInC { get; set; }
        public string AvgTempInC { get; set; }
        public List<WeatherForecast> Data { get; set; }
    }

    public class WeatherForecast
    {
        public string Location { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double TemperatureInC { get; set; }
        public double TemperatureInF { get; set; }
        public double deltaInC { get; set; }
    }

    public class MultiLocWeatherForecast : BaseResponse
    {
        public List<MultiLocData> Data { get; set; }

        public List<WeatherForecast> DeltaForecast { get; set; }
    }

    public class MultiLocData
    {
        public string location { get; set; }
        public string MinTempInC { get; set; }
        public string MaxTempInC { get; set; }
        public string AvgTempInC { get; set; }
        public List<WeatherForecast> LocationForecasts { get; set; }
    }
}
