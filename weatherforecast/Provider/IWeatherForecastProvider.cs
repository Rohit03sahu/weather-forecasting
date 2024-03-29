﻿using weatherforecast.Model.Common;
using weatherforecast.Model.RequestDto;
using weatherforecast.Model.ResponseDto;

namespace weatherforecast.Provider
{
    public interface IWeatherForecastProvider
    {
        Task<WeatherForecasts> FetchWeatherForecast(WeatherForecastDto request);
        Task<MultiLocWeatherForecast> FetchMultiLocWeatherForecast(WeatherForecastDeltaDto request);
        Task<LocationResponse> GetLocations();
    }
}
