using GPSNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Pin
{
    public interface IPinService
    {
        Task<int> AddPinAsync(PinModel pinModel);
        Task<int> UpdatePinAsync(PinModel pinModel);
        Task<int> DeletePinAsync(PinModel pinModel);
        Task<List<PinModel>> GetAllPinModelsAsync();
        PinModel CreatePinModel(string name, string latitude, string longitude, string description);
    }
}
