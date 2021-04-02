using GPSNotepad.Models;
using GPSNotepad.Services.Repositiry;
using System.Collections.Generic;
using System.Linq;

namespace GPSNotepad.Services.Pin
{
    public class PinService : IPinService
    {
        private IRepository _repository;

        public PinService(IRepository repository)
        {
            _repository = repository;
        }

        public int AddPin(PinModel profile)
        {
            var result = _repository.InsertAsync(profile).Result;

            return result;
        }

        public int UpdatePin(PinModel profile)
        {
            var result = _repository.UpdateAsync(profile).Result;

            return result;
        }

        public int DeletePin(PinModel profile)
        {
            var result = _repository.DeleteAsync(profile).Result;

            return result;
        }

        public List<PinModel> GetAllPins(int userId)
        {
            var pin = _repository.GetAllAsync<PinModel>().Result;
            pin = pin.Where(x => x.UserId == userId).ToList();

            return pin;
        }
    }
}
