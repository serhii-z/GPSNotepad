using GPSNotepad.Models.Weather;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace GPSNotepad.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        public async Task<WeatherResponse> GetWeatherResponseAsync(string lat, string lon, string units)
        {
            WeatherResponse weatherResponse = null;
            var url = string.Format(Constants.OpenWeatherUrl, lat, lon, units, Constants.OpenWeatherKey);
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(content);
            }

            return weatherResponse;
        }
    }
}
