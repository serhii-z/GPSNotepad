using GPSNotepad.Models.Weather;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Weather
{
    public interface IWeatherService
    {
        Task<List<WeatherView>> GetWeatherAsync(double latitude, double longitude);
    }
}
