using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace WeatherAlert
{
    public class WeatherFetcher
    {
        public const String WeatherApiUrl = "http://www.weather.com.cn/adat/cityinfo/{0}.html";
        public String location
        {
            get;
            set;
        }
        public async Task<String>  getWeather()
        {
            LocationFetcher locationFetcher = new LocationFetcher();

            var location = await locationFetcher.getCurrentCity();

            HttpClient client = new HttpClient();
            StringBuilder url = new StringBuilder();
            url.AppendFormat(WeatherApiUrl, location.AreaCode);
            HttpResponseMessage response = await client.GetAsync(url.ToString());
            String weatherInfo = await response.Content.ReadAsStringAsync();

            JsonObject jsonObject = JsonObject.Parse(weatherInfo);
            JsonValue jsonValue = jsonObject.GetNamedValue("weatherinfo");
            String weather = jsonValue.GetObject().GetNamedString("weather");
            return weather;
        }
    }
}
