using weatherforecast.Enums;

namespace weatherforecast.Model.RequestDto
{
    public class WeatherForecastDeltaDto
    {
        public List<string> Locations { get; set; }
        public WeatherTimeLineEnum TimeLine {  get; set; }

        public List<string> Validate() {
        
            List<string> errorList = new List<string>();
            if (this.Locations == null && this.Locations.Count<=0) { errorList.Add(" Invalid Location "); }
            if (this.TimeLine == null ) { errorList.Add(" Invalid Timeline"); }

            return errorList;
        }
    }

}
