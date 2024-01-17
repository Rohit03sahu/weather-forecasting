using weatherforecast.Model.Common;
using weatherforecast.Model.ResponseDto;

namespace weatherforecast.Mapper
{
    public class WeatherForecastMapper
    {
        public static List<WeatherForecast> MapMinutelyForecast(Weather weather)
        {
            List<WeatherForecast> weatherForecast = new List<WeatherForecast>();
            foreach (var report in weather.timelines.minutely)
            {
                weatherForecast.Add
                    (
                    new WeatherForecast { Location= weather.location.name, TimeStamp= report.time, TempratureInC= report.values.temperature, TempratureInF= convertCelToF(report.values.temperature) });
            }
            return weatherForecast;

        }
        
        public static List<WeatherForecast> MapHourlyForecast(Weather weather)
        {
            List<WeatherForecast> weatherForecast = new List<WeatherForecast>() ;
            foreach (var report in weather.timelines.hourly)
            {
                weatherForecast.Add(new WeatherForecast { Location= weather.location.name, TimeStamp= report.time, TempratureInC= report.values.temperature, TempratureInF=convertCelToF(report.values.temperature) });
            }
            return weatherForecast;
        }

        public static List<WeatherForecast> MapDailyForecast(Weather weather)
        {
            List<WeatherForecast> weatherForecast = new List<WeatherForecast>();
            foreach (var report in weather.timelines.daily)
            {
                weatherForecast.Add(new WeatherForecast { Location= weather.location.name, TimeStamp= report.time, TempratureInC= report.values.temperature, TempratureInF=convertCelToF(report.values.temperature) });
            }
            return weatherForecast;
        }
        public static double convertCelToF(double celsius)
        {
            var value = (9/5 * celsius) + 32;
            return value;
        }

    }
}
