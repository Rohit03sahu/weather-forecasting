using weatherforecast.Model.Common;
using weatherforecast.Model.ResponseDto;

namespace weatherforecast.Mapper
{
    public class WeatherForecastMapper
    {
        public static WeatherForecasts MapMinutelyForecast(Weather weather)
        {
            WeatherForecasts weatherForecasts = new WeatherForecasts() { WeatherForecast= new List<WeatherForecast>() };
            foreach(var report in weather.timelines.minutely)
            {
                weatherForecasts.WeatherForecast.Add(new WeatherForecast { Location= weather.location.name, TimeStamp= report.time, TempratureInC= report.values.temperature.ToString(), TempratureInF="" });
            }
            return weatherForecasts;

        }
        
        public static WeatherForecasts MapHourlyForecast(Weather weather)
        {
            WeatherForecasts weatherForecasts = new WeatherForecasts() { WeatherForecast= new List<WeatherForecast>() };
            foreach (var report in weather.timelines.hourly)
            {
                weatherForecasts.WeatherForecast.Add(new WeatherForecast { Location= weather.location.name, TimeStamp= report.time, TempratureInC= report.values.temperature.ToString(), TempratureInF="" });
            }
            return weatherForecasts;
        }

        public static WeatherForecasts MapDailyForecast(Weather weather)
        {
            WeatherForecasts weatherForecasts = new WeatherForecasts() { WeatherForecast= new List<WeatherForecast>() };
            foreach (var report in weather.timelines.daily)
            {
                weatherForecasts.WeatherForecast.Add(new WeatherForecast { Location= weather.location.name, TimeStamp= report.time, TempratureInC= report.values.temperature.ToString(), TempratureInF="" });
            }
            return weatherForecasts;
        }
    }
}
