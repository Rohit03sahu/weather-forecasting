using weatherforecast.Model.Common;

namespace weatherforecast.Model.ResponseDto
{
    public class WeatherForecasts: BaseResponse
    {
        public List<WeatherForecast> Source {  get; set; }
        public List<WeatherForecast> Destination { get; set; }
        public List<WeatherForecast> Delta { get; set; }
    }

    public class WeatherForecast 
    {
        public string Location { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double TempratureInC { get; set; }
        public double TempratureInF { get; set; }
    }

}
