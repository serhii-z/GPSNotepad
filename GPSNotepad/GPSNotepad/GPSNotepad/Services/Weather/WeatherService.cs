using GPSNotepad.Models.Weather;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System;
using GPSNotepad.Enums;

namespace GPSNotepad.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private Root _weatherResponse;

        #region -- IWeatherService implement --

        public async Task<List<WeatherView>> GetWeatherAsync(double latitude, double longitude)
        {
            await GetWeatherResponseAsync(latitude.ToString(), longitude.ToString(), Constants.OpenWeatherUnits);
            var temps = GetTemperature();
            var icons = GetIcons();
            var numberDays = GetNumberDays();
            var WeatherList = new List<WeatherView>();


            for (int i = 0; i < 8; i++)
            {
                var nameDay = Convert.ToString((Days)numberDays[i]);
                var icon = string.Format(Constants.OpenWeatherIconPath, icons[i].ToList()[0]);
                var day = Convert.ToInt32(temps[i].day);
                var night = Convert.ToInt32(temps[i].night);
                var temp = string.Format("{0}{1} {2}{3}", day, "°", night, "°");
                WeatherList.Add(new WeatherView(nameDay, icon, temp));
            }

            return WeatherList;
        }

        #endregion

        #region -- Private methods --

        private async Task<Root> GetWeatherResponseAsync(string latitude, string longitude, string units)
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

        private List<Temp> GetTemperature()
        {
            return _weatherResponse.daily.Select(x => x.temp).ToList();
        }

        private List<List<string>> GetIcons()
        {
            var weather = _weatherResponse.daily.Select(x => x.weather);

            return weather.Select(x => x.Select(y => y.icon).ToList()).ToList();
        }


        private int[] GetNumberDays()
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

        #endregion
    }
}
