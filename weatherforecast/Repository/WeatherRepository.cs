using System.Net;
using System.Net.Mime;
using System.Text;
using weatherforecast.Config;
using weatherforecast.Model;

namespace weatherforecast.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly AppSettings _appSettings;
        public WeatherRepository(AppSettings appSettings) 
        {
            this._appSettings = appSettings;
        }
        public async Task<HttpResponseMessage> FetchWeatherDetailsByLocations(List<string> locations)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_appSettings.WeatherApiUrl);  //https://api.tomorrow.io/v4/weather/forecast?location=new%20york&apikey=3Ru3wXyFO5lVAy2BzL1kqto1QmRpe6FH");
            var response = await client.GetAsync($"{_appSettings.WeatherApiEndpoint}?location={locations.FirstOrDefault().ToString()}&apikey={_appSettings.WeatherApiKey}");
            if(response == null || response.StatusCode != HttpStatusCode.OK) {
                throw new Exception($"unable to fetch records from weather api {response?.ReasonPhrase}");
            }
            return response;
        }

        public async Task<HttpResponseMessage> GetLocationData()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_appSettings.WeatherApiUrl);  //https://api.tomorrow.io/v4/weather/forecast?location=new%20york&apikey=3Ru3wXyFO5lVAy2BzL1kqto1QmRpe6FH");
            var response = await client.GetAsync($"{_appSettings.LocationApiEndpoint}?apikey={_appSettings.WeatherApiKey}");
            if (response == null || response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"unable to fetch records from weather api {response?.ReasonPhrase}");
            }
            return response;
        }
    }
}
