using GPSNotepad.Models;
using System.Collections.Generic;

namespace GPSNotepad.Services.Pin
{
    public interface IPinService
    {
        int AddPin(PinModel profile);
        int UpdatePin(PinModel profile);
        int DeletePin(PinModel profile);
        List<PinModel> GetAllPinModels(int userId);
    }
}
