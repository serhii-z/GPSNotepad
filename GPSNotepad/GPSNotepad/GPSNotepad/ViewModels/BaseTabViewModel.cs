using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using Prism.Navigation;
using System.Collections.Generic;

namespace GPSNotepad.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        protected IAuthorizationService authorizationService;
        protected IPinService pinService;
        public BaseTabViewModel(INavigationService navigationService, IAuthorizationService authorizationService,
            IPinService pinService) : base(navigationService)
        {
            this.authorizationService = authorizationService;
            this.pinService = pinService;
        }

        #region --- Protected Methods ---

        protected List<PinViewModel> GetAllPins(int userId)
        {
            var pinViewModels = new List<PinViewModel>();

            foreach (var item in pinService.GetAllPinModels(userId))
            {
                pinViewModels.Add(CreatePinViewModel(item));
            }

            return pinViewModels;
        }

        protected PinViewModel CreatePinViewModel(PinModel pinModel)
        {
            var pinViewModel = new PinViewModel();
            PinViewModelExtension.InitInstance(pinViewModel, pinModel);

            return pinViewModel;
        }

        #endregion
    }
}
