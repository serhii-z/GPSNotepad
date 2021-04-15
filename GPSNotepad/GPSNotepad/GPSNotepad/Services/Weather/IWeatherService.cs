using GPSNotepad.Models.Weather;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Weather
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherResponseAsync(string lat, string lon, string units);
    }
}
