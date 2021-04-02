using GPSNotepad.Models;
using GPSNotepad.Services.Repositiry;
using System.Collections.Generic;
using System.Linq;

namespace GPSNotepad.Services.Pin
{
    public class PinService : IPinService
    {
        public int AddPin(PinModel profile)
        {
            var result = Repository.InsertAsync(profile).Result;

            return result;
        }

        public int UpdatePin(PinModel profile)
        {
            var result = Repository.UpdateAsync(profile).Result;

            return result;
        }

        public int DeletePin(PinModel profile)
        {
            var result = Repository.DeleteAsync(profile).Result;

            return result;
        }

        public List<PinModel> GetAllPins(int userId)
        {
            var pin = Repository.GetAllAsync<PinModel>().Result;
            pin = pin.Where(x => x.UserId == userId).ToList();

            return pin;
        }
    }
}
