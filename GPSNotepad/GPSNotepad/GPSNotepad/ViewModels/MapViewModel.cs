using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
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

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private bool _hasVisible;
        public bool HasVisible
        {
            get => _hasVisible;
            set => SetProperty(ref _hasVisible, value);
        }

        private CameraPosition _cameraPosition;
        public CameraPosition CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue("pinViewModel", out PinViewModel value))
            {
                MakePinFocus(value);
            }

            ShowPins();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            HasVisible = false;
        }

        #endregion

        #region --- Private Helpers ---

        private void OnPinTap(Pin obj)
        {
            var pin = obj;
            var pinViewModels = GetAllPins(authorizationService.UserId);
            var pinViewModel = pinViewModels.Where(x => x.PinId == pin.ZIndex).FirstOrDefault();

            Name = pinViewModel.Name;
            Description = pinViewModel.Description;
            HasVisible = true;
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

            Pins.Clear();

            foreach (var item in pinViewModels)
            {
                Pins.Add(CreatePin(item));
            }
        }

        private void MakePinFocus(PinViewModel pinViewModel)
        {
            var pin = CreatePin(pinViewModel);

            CameraPosition = new CameraPosition(pin.Position, 16.0);
        }

        #endregion
    }
}
