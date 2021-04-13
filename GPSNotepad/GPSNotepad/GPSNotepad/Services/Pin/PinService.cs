using GPSNotepad.Models;
using GPSNotepad.Services.Repositiry;
using GPSNotepad.Services.SettingsService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Pin
{
    public class PinService : IPinService
    {
        private IRepositoryService _repository;
        private ISettingsManager _settingsManager;

        public PinService(IRepositoryService repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        #region -- IPinService implementation --

        public async Task<int> AddPinAsync(PinModel pinModel)
        {
            pinModel.UserId = _settingsManager.GetUserId();
            var result = await _repository.InsertAsync(pinModel);

            return result;
        }
        
        public async Task<int> UpdatePinAsync(PinModel pinModel)
        {
            var result = await _repository.UpdateAsync(pinModel);

            return result;
        }

        public async Task<int> DeletePinAsync(PinModel pinModel)
        {
            var result = await _repository.DeleteAsync(pinModel);

            return result;
        }

        public async Task<List<PinModel>> GetAllPinModelsAsync()
        {
            var pinModels = await _repository.GetAllAsync<PinModel>();
            var userId = _settingsManager.GetUserId();
            pinModels = pinModels.Where(x => x.UserId == userId).ToList();

            return pinModels;
        }

        #endregion
    }
}
