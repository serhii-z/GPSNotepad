using GPSNotepad.Extensions;
using GPSNotepad.Models;
using GPSNotepad.Services.Pin;
using GPSNotepad.Services.SettingsService;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPSNotepad.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        protected IPinService pinService;
        protected ISettingsManager settingsManager;
        public BaseTabViewModel(INavigationService navigationService,
            IPinService pinService, ISettingsManager settingsManager) : base(navigationService)
        {
            this.pinService = pinService;
            this.settingsManager = settingsManager;
        }

        #region --- Protected Methods ---

        protected async Task<List<PinViewModel>> GetAllPinViewModelsAsync()
        {
            var pinModels = await pinService.GetAllPinModelsAsync();
            var pinViewModels = new List<PinViewModel>();

            foreach (var item in pinModels)
            {
                pinViewModels.Add(item.ToViewModel());
            }

            return pinViewModels;
        }

        #endregion
    }
}
