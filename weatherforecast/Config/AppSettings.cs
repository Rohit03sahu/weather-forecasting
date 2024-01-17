namespace weatherforecast.Config
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;
        public AppSettings(IConfiguration configuration) 
        {
            this._configuration = configuration;
        }

        public string WeatherApiKey => _configuration.GetValue<string>("WeatherApiKey");
        public string WeatherApiUrl => _configuration.GetValue<string>("WeatherApiUrl");
        public string WeatherApiEndpoint => _configuration.GetValue<string>("WeatherApiEndpoint");
    }
}
