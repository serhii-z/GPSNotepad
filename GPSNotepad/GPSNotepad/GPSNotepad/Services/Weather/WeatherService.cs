using GPSNotepad.Models.Weather;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System;

namespace GPSNotepad.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private Root _weatherResponse;

        public async Task<Root> GetWeatherResponseAsync(string latitude, string longitude, string units)
        {        
            var url = string.Format(Constants.OpenWeatherUrl, latitude, longitude, units, Constants.OpenWeatherKey);
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _weatherResponse = JsonConvert.DeserializeObject<Root>(content);
            }

            return _weatherResponse;
        }

        public List<Temp> GetTemperature()
        {
            return _weatherResponse.daily.Select(x => x.temp).ToList();
        }

        public List<List<string>> GetIcons()
        {
            var weather = _weatherResponse.daily.Select(x => x.weather);

            return weather.Select(x => x.Select(y => y.icon).ToList()).ToList();
        }

        public int[] GetNumberDays()
        {
            var numberDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            int[] numberDays = new int[8];

            for (int i = 0, j = numberDay; i < 8; i++, j++)
            {
                if (j > 6)
                {
                    j = 0;
                }
                numberDays[i] = j;
            }

            return numberDays;
        }
    }
}
