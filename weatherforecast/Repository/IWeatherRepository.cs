using weatherforecast.Model;

namespace weatherforecast.Repository
{
    public interface IWeatherRepository
    {
        Task<HttpResponseMessage> FetchWeatherDetailsByLocations(List<string> locations); 
    }
}
