namespace weatherforecast.Model.Common
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WeatherData
    {
        public List<Timeline> timelines { get; set; }
    }

    public class Interval
    {
        public DateTime startTime { get; set; }
        public Values values { get; set; }
    }

    public class Weather
    {
        public WeatherData data { get; set; }
    }

    public class Timeline
    {
        public string timestep { get; set; }
        public DateTime endTime { get; set; }
        public DateTime startTime { get; set; }
        public List<Interval> intervals { get; set; }
    }

    public class Values
    {
        public double temperature { get; set; }
    }


}
