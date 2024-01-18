namespace weatherforecast.Model.Common
{
    public class Data
    {
        public List<LocationData> locations { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class LocationData
    {
        public string id { get; set; }
        public string name { get; set; }
        public Geometry geometry { get; set; }
        public string timezone { get; set; }
        public List<string> tags { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isAccountResource { get; set; }
    }

    public class Meta
    {
        public int totalItems { get; set; }
    }

    public class LocationMetaData
    {
        public Data data { get; set; }
        public Meta meta { get; set; }
        public Links links { get; set; }
    }
}
