using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using GPSNotepad.Views;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.ViewModels
{
    public class MapViewModel : BaseTabViewModel
    {
        public MapViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService) : base(navigationService, authorizationService, pinService)
        {
            Pins = new ObservableCollection<Pin>();
        }

        #region --- Public Properties ---

        public ICommand PinTappedCommand => new Command<Pin>(OnPinTap);

        public ICommand FrameTappedCommand => new Command(OnFrameTap);

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private bool _hasVisible;
        public bool HasVisible
        {
            get => _hasVisible;
            set => SetProperty(ref _hasVisible, value);
        }

        private MapSpan _region;
        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private bool _animated = false;
        public bool Animated
        {
            get => _animated;
            set => SetProperty(ref _animated, value);
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ShowPins();

            if (parameters.TryGetValue("pinViewModel", out PinViewModel value))
            {
                MakePinFocus(value);
            }       
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            
            HasVisible = false;
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnPinTap(Pin obj)
        {
            HasVisible = true;

            var pin = obj;
            var pinViewModels = GetAllPins(authorizationService.UserId);
            var pinViewModel = pinViewModels.Where(x => x.PinId == pin.ZIndex).FirstOrDefault();

            var parameters = new NavigationParameters();
            parameters.Add("pinViewModel", pinViewModel);

            await navigationService.NavigateAsync($"{nameof(PinInfoView)}", parameters, useModalNavigation: true);
        }

        private void OnFrameTap()
        {
            HasVisible = false;
        }

        #endregion

        #region --- Private Methods ---

        private Pin CreatePin(PinViewModel pinViewModel)
        {
            Pin pin = new Pin
            {
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                Label = pinViewModel.Name,
                ZIndex = pinViewModel.PinId
            };

            return pin;
        }

        private void ShowPins()
        {
            var pinViewModels = GetAllPins(authorizationService.UserId);

            foreach (var item in pinViewModels)
            {
                Pins.Add(CreatePin(item));
            }
        }

        private void MakePinFocus(PinViewModel pinViewModel)
        {
            var pin = CreatePin(pinViewModel);
            Region = MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(2));
            Animated = false;
        }

        #endregion
    }
}
