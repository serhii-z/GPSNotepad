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

        public List<PinViewModel> GetAllPins(int userId)
        {
            var pinViewModels = new List<PinViewModel>();

            foreach(var item in GetAllPinModels(userId))
            {
                pinViewModels.Add(CreatePinViewModel(item));
            }

            return pinViewModels;
        }

        private PinViewModel CreatePinViewModel(PinModel pinModel)
        {
            var pinViewModel = new PinViewModel();
            PinViewModelExtension.InitInstance(pinViewModel, pinModel);

            return pinViewModel;
        }

        private List<PinModel> GetAllPinModels(int userId)
        {
            var pinModels = _repository.GetAllAsync<PinModel>().Result;
            pinModels = pinModels.Where(x => x.UserId == userId).ToList();

            return pinModels;
        }
    }
}
