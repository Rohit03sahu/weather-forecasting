namespace weatherforecast.Model.Common
{
    public class Data
    {
        public List<string> locations { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class Meta
    {
        public int totalItems { get; set; }
    }

    public class LocationData
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
        public Links links { get; set; }
    }
}
