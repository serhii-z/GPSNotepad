using GPSNotepad.Models;
using GPSNotepad.Services.Pin;
using GPSNotepad.Services.SettingsService;
using GPSNotepad.Views;
using GPSNotepad.Extensions;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;
using GPSNotepad.Services.Permissions;
using GPSNotepad.Services.GeoLocations;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Weather;

namespace GPSNotepad.ViewModels
{
    public class MapTabViewModel : BaseTabViewModel
    {
        private IPermissionService _permissionService;
        private IGeoLocationService _geoLocationService;
        private IAuthorizationService _authorizationService;
        private IWeatherService _weatherService;
        private PinViewModel _pinViewModel;
        public MapTabViewModel(INavigationService navigationService, IPinService pinService, 
            ISettingsManager settingsManager, PermissionService permissionService, 
            IGeoLocationService geoLocationService, IAuthorizationService authorizationService, IWeatherService weatherService) : 
            base(navigationService, pinService, settingsManager)
        {
            _permissionService = permissionService;
            _geoLocationService = geoLocationService;
            _authorizationService = authorizationService;
            _weatherService = weatherService;

            Pins = new ObservableCollection<Pin>();   
        }

        #region --- Public Properties ---

        public ICommand SettingsTapCommand => new Command(OnSettingsTap);

        private void OnSettingsTap(object obj)
        {
            //go to search page
        }

        public ICommand SearchBarTapCommand => new Command(OnSearchBarTap);                   //?
        public ICommand PinTapCommand => new Command<Pin>(OnPinTapAsync);
        public ICommand PinSearchCommand => new Command(OnSearchPins);
        public ICommand MapTapCommand => new Command(OnMapTap);
        public ICommand LocationButtonTapCommand => new Command(OnLocationButtonTapAsync);
        public ICommand ExitTapCommand => new Command(OnExitTapAsync);
        public ICommand PinInfoTapCommand => new Command(OnPinInfoTap);
        public ICommand TimeButtonTapCommand => new Command(OnTimeButtonTapAsync);

        private bool _isUserPosition;
        public bool IsUserPosition
        {
            get => _isUserPosition;
            set => SetProperty(ref _isUserPosition, value);
        }

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private Pin _pinSelectedItem;
        public Pin PinSelectedItem
        {
            get => _pinSelectedItem;
            set => SetProperty(ref _pinSelectedItem, value);
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

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private Rectangle _topBorder;
        public Rectangle TopBorder
        {
            get => _topBorder;
            set => SetProperty(ref _topBorder, value);
        }

        private Rectangle _bottomBorder;
        public Rectangle BottomBorder
        {
            get => _bottomBorder;
            set => SetProperty(ref _bottomBorder, value);
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

        private string _position;
        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private string _temperature;
        public string Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get => _windSpeed;
            set => SetProperty(ref _windSpeed, value);
        }

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(PinSelectedItem))
            {
                OnSelectedItemTap();
            }
            if (args.PropertyName == nameof(SearchText))
            {
                if (PinSearchCommand.CanExecute(null))
                {
                    PinSearchCommand.Execute(null);
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                MakePinFocus(value);
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            //Pins = new ObservableCollection<PinViewModel>();

            var isStatus = await _permissionService.CheckStatusAsync();

            if (isStatus)
            {
                IsUserPosition = true;
            }

            var pinViewModels = await GetAllPinViewModelsAsync();

            InitPins(pinViewModels);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSearchPins()
        {
            var resultSearch = await SearchPins();

            InitPins(resultSearch);
            OnSearchBarTap();
        }

        private void OnSelectedItemTap()
        {
            var pin = _pinSelectedItem;
            var pinViewmodel = pin.ToPinViewModel();

            OnMapTap();
            MakePinFocus(pinViewmodel);
        }

        private void OnSearchBarTap()
        {
            TopBorder = new Rectangle(0.0, 0.12, 1, 0.31);
        }

        private void OnMapTap()
        {
            TopBorder = new Rectangle(0.0, 0.12, 1, 0.0);
        }

        private async void OnLocationButtonTapAsync()
        {
            var position = await _geoLocationService.GetUserPositionAsync();

            Animated = true;
            Region = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(2));
        }

        private async void OnExitTapAsync()
        {
            _authorizationService.LogOut();

            await navigationService.NavigateAsync($"{nameof(SignInView)}");
        }


        private async void OnPinTapAsync(Pin pin)
        {
            _pinViewModel = pin.ToPinViewModel();

            ShowPinInfo();
            await ShowWeatherAsync(_pinViewModel.Latitude.ToString(), _pinViewModel.Longitude.ToString());

            BottomBorder = new Rectangle(0.5, 1.0, 1.0, 0.45);
        }

        private void OnPinInfoTap()
        {
            BottomBorder = new Rectangle(0.0, 1.0, 1.0, 0.0);
        }

        private async void OnTimeButtonTapAsync()
        {
            OnPinInfoTap();

            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, _pinViewModel);

            await navigationService.NavigateAsync(nameof(ClockView), parameters, useModalNavigation: true);
        }

        #endregion

        #region --- Private Methods ---

        private async Task<List<PinViewModel>> SearchPins()
        {
            var pinViewModels = await GetAllPinViewModelsAsync();
            var pinList = pinViewModels.Where(x => x.Name.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower()).ToList();

            return pinList;
        }

        private void InitPins(List<PinViewModel> pinViewModel)
        {
            Pins.Clear();

            foreach (var item in pinViewModel)
            {
                Pins.Add(item.ToPin());
                //Pins.Add(item);
            }
        }

        private void MakePinFocus(PinViewModel pinViewModel)
        {
            var pin = pinViewModel.ToPin();

            Region = MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(2));
        }

        private void ShowPinInfo()
        {
            Name = _pinViewModel.Name;
            Description = _pinViewModel.Description;
            Position = string.Format("{0} {1}", _pinViewModel.Latitude.ToString(), _pinViewModel.Longitude.ToString());
        }

        private async Task ShowWeatherAsync(string latitude, string longitude)
        {
            var weatherResponse = await _weatherService.GetWeatherResponseAsync(latitude, longitude, Constants.OpenWeatherUnits);
            var icon = weatherResponse.Weather.Select(x => x).First().Icon;

            Temperature = string.Format("{0}: {1}°C", "temperature", weatherResponse.Main.Temp.ToString());
            WindSpeed = string.Format("{0}: {1}km/h", "vind speed", weatherResponse.Wind.Speed.ToString());
            IconPath = string.Format(Constants.OpenWeatherIconPath, icon);
        }

        #endregion
    }
}
