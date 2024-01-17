using weatherforecast.Model.Common;

namespace weatherforecast.Model.ResponseDto
{
    public class WeatherForecasts: BaseResponse
    {
        public List<WeatherForecast> WeatherForecast {  get; set; }
    }

    public class WeatherForecast 
    {
        public string Location { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string TempratureInC { get; set; }
        public string TempratureInF { get; set; }
    }

}
