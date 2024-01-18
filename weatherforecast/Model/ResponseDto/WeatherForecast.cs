using weatherforecast.Model.Common;

namespace weatherforecast.Model.ResponseDto
{
    public class WeatherForecasts: BaseResponse
    {
        public List<WeatherForecast> Data {  get; set; }
    }

    public class WeatherForecast 
    {
        public string Location { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double TemperatureInC { get; set; }
        public double TemperatureInF { get; set; }
    }

    public class WeatherForecastWithDelta : BaseResponse
    {
        public List<WeatherForecast> PrimaryLocForecast { get; set; }
        public List<WeatherForecast> SecondaryLocForecast { get; set; }

        public List<WeatherForecast> DeltaForecast { get; set; }
    }

}
