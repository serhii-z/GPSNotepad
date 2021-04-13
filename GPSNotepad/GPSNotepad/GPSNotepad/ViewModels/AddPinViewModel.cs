using GPSNotepad.Models;
using GPSNotepad.Services.Pin;
using GPSNotepad.Extensions;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.ViewModels
{
    public class AddPinViewModel : BaseViewModel
    {
        private IPinService _pinService;

        public AddPinViewModel(INavigationService navigationService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;

            Pins = new ObservableCollection<Pin>();
        }

        #region -- Public properties --

        public ICommand SaveButtonTapCommand => new Command(OnSaveButtonTapAsync);
        public ICommand MapTapCommand => new Command<Position>(OnMapTap);

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private string _nameText;
        public string NameText
        {
            get => _nameText;
            set => SetProperty(ref _nameText, value);
        }

        private string _latitudeText;
        public string LatitudeText
        {
            get => _latitudeText;
            set => SetProperty(ref _latitudeText, value);
        }

        private string _longitudeText;
        public string LongitudeText
        {
            get => _longitudeText;
            set => SetProperty(ref _longitudeText, value);
        }

        private string _descriptionText;
        public string DescriptionText
        {
            get => _descriptionText;
            set => SetProperty(ref _descriptionText, value);
        }

        #endregion

        #region -- Private helpers --

        private void OnMapTap(Position obj)
        {
            var position = obj;

            LatitudeText = position.Latitude.ToString();
            LongitudeText = position.Longitude.ToString();

            Pins.Clear();
            Pins.Add(CreatePin());
        }

        private async void OnSaveButtonTapAsync()
        {
            if (!string.IsNullOrEmpty(_nameText) && 
                !string.IsNullOrEmpty(_latitudeText) && 
                !string.IsNullOrEmpty(_longitudeText) && 
                !string.IsNullOrEmpty(_descriptionText))
            {
                var pinViewmodel = await AddPinAsync();
                var parameters = new NavigationParameters();
                parameters.Add(Constants.KeyPinViewModel, pinViewmodel);

                await navigationService.GoBackAsync(parameters);
            }
        }

        #endregion

        #region -- Private methods --

        private Pin CreatePin()
        {
            var pin = new Pin
            {
                Position = new Position(double.Parse(LatitudeText), double.Parse(LongitudeText)),
                Label = "PinName"
            };

            return pin;
        }

        private PinViewModel CreatePinViewmodel()
        {
            var pinViewModel = new PinViewModel
            {
                Name = _nameText,
                Latitude = double.Parse(_latitudeText),
                Longitude = double.Parse(_longitudeText),
                Description = _descriptionText
            };

            return pinViewModel;
        }

        private async Task<PinViewModel> AddPinAsync()
        {
            var pinViewModel = CreatePinViewmodel();

            await _pinService.AddPinAsync(pinViewModel.ToPinModel());

            return pinViewModel;
        }

        #endregion
    }
}
