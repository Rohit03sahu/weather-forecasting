using weatherforecast.Model;

namespace weatherforecast.Repository
{
    public interface IWeatherRepository
    {
        Task<HttpResponseMessage> FetchWeatherDetailsByLocations(string location,string timeLine);
        Task<HttpResponseMessage> GetLocationData();
    }
}
