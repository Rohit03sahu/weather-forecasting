using weatherforecast.Model.Common;
using weatherforecast.Model.ResponseDto;
using weatherforecast.Seed;

namespace weatherforecast.Mapper
{
    public class WeatherForecastMapper
    {

        public static WeatherForecasts mapForecastData(Weather weatherData,string location)
        {
            WeatherForecasts weatherforecasts = new WeatherForecasts()
            {
                Data= new List<WeatherForecast>()
            };
            weatherData.data.timelines.FirstOrDefault().intervals.ForEach(x=> weatherforecasts.Data.Add(new WeatherForecast() { Location= location, TemperatureInC=x.values.temperature, TemperatureInF= convertCelToF(x.values.temperature), TimeStamp=x.startTime}));
            weatherforecasts.IsSuccess = true;
            return weatherforecasts;
        }

        public static MultiLocWeatherForecast mapMultiLocForecastData(Dictionary<string, Weather> keyValuePairs)
        {
            MultiLocWeatherForecast weatherforecasts = new MultiLocWeatherForecast()
            {
                Data= new List<MultiLocData>(),
            };
            foreach (var item in keyValuePairs )
            {
                var data = new List<WeatherForecast>();
                item.Value.data.timelines.FirstOrDefault().intervals.ForEach(x => data.Add(new WeatherForecast() { Location= item.Key, TemperatureInC=x.values.temperature, TemperatureInF= convertCelToF(x.values.temperature), TimeStamp=x.startTime }));
                weatherforecasts.Data.Add
                    ( 
                    new MultiLocData() 
                    { 
                        location=item.Key,
                        LocationForecasts= data,
                        MaxTempInC=data.Select(x => Convert.ToDouble(x.TemperatureInC)).Max().ToString(),
                        MinTempInC=data.Select(x => Convert.ToDouble(x.TemperatureInC)).Min().ToString(),
                        AvgTempInC=data.Select(x => Convert.ToDouble(x.TemperatureInC)).Average().ToString()
            });
            }            
            weatherforecasts.IsSuccess = true;
            return weatherforecasts;
        }

        public static List<LocResponse> mapLocationSeedData(List<LocationSeedData> locationSeedDatas)
        {
            List<LocResponse> locResponses = new List<LocResponse>();
            locationSeedDatas.ForEach(x => locResponses.Add(new LocResponse() { name=x.Name, latLong= x.Coordinates[0]+","+x.Coordinates[1] }));
            return locResponses;
        }
        private static double convertCelToF(double celsius)
        {
            var value = (9/5 * celsius) + 32;
            return value;
        }

    }
}
