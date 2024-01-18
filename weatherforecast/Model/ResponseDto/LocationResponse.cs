namespace weatherforecast.Model.ResponseDto
{
    public class LocationResponse
    {
        public List<LocResponse> locResponse {  get; set; }
    }

    public class LocResponse
    {
        public string name { get; set; }
        public string latLong { get; set; }
    }
}
