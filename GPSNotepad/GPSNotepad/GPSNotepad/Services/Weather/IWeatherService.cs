using GPSNotepad.Models.Weather;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Weather
{
    public interface IWeatherService
    {
        Task<Root> GetWeatherResponseAsync(string lat, string lon, string units);
        List<Temp> GetTemperature();
        List<List<string>> GetIcons();
        int[] GetNumberDays();
    }
}
