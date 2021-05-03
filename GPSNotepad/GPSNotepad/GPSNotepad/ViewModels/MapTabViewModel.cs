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
using GPSNotepad.Services.Resources;
using Prism.Services.Dialogs;
using GPSNotepad.Models.Weather;

namespace GPSNotepad.ViewModels
{
    public class MapTabViewModel : BaseTabViewModel
    {
        private IPermissionService _permissionService;
        private IGeoLocationService _geoLocationService;
        private IAuthorizationService _authorizationService;
        private IWeatherService _weatherService;
        private PinViewModel _pinViewModel;
        private IResourceService _resourceService;
        private IDialogService _dialogService;

        public MapTabViewModel(INavigationService navigationService, IPinService pinService, 
            ISettingsManager settingsManager, PermissionService permissionService, 
            IGeoLocationService geoLocationService, IAuthorizationService authorizationService, 
            IWeatherService weatherService, IResourceService resourceService, IDialogService dialogService) : 
            base(navigationService, pinService, settingsManager)
        {
            _permissionService = permissionService;
            _geoLocationService = geoLocationService;
            _authorizationService = authorizationService;
            _weatherService = weatherService;
            _resourceService = resourceService;
            _dialogService = dialogService;
            WeatherCollection = new ObservableCollection<WeatherView>();
            Pins = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand SettingsTapCommand => new Command(OnSettingsTapAsync);
        public ICommand FocusedCommand => new Command(OnFocused);
        public ICommand PinTapCommand => new Command<Pin>(OnPinTapAsync);
        public ICommand PinSearchCommand => new Command(OnSearchPins);
        public ICommand MapTapCommand => new Command(OnMapTap);
        public ICommand LocationButtonTapCommand => new Command(OnLocationButtonTapAsync);
        public ICommand ExitTapCommand => new Command(OnExitTapAsync);
        public ICommand PinInfoTapCommand => new Command(OnPinInfoTap);
        public ICommand TimeButtonTapCommand => new Command(OnTimeButtonTapAsync);

        private ObservableCollection<PinViewModel> _pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private ObservableCollection<WeatherView> _weatherCollection;
        public ObservableCollection<WeatherView> WeatherCollection
        {
            get => _weatherCollection;
            set => SetProperty(ref _weatherCollection, value);
        }

        private bool _isUserPosition;
        public bool IsUserPosition
        {
            get => _isUserPosition;
            set => SetProperty(ref _isUserPosition, value);
        }

        private PinViewModel _pinSelectedItem;
        public PinViewModel PinSelectedItem
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

        private bool _animated;
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

        private string _position;
        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private string _firstDay;
        public string FirstDay
        {
            get => _firstDay;
            set => SetProperty(ref _firstDay, value);
        }

        private string _secondDay;
        public string SecondDay
        {
            get => _secondDay;
            set => SetProperty(ref _secondDay, value);
        }

        private string _thirdDay;
        public string ThirdDay
        {
            get => _thirdDay;
            set => SetProperty(ref _thirdDay, value);
        }

        private string _fourstDay;
        public string FourstDay
        {
            get => _fourstDay;
            set => SetProperty(ref _fourstDay, value);
        }

        private string _firstIcon;
        public string FirstIcon
        {
            get => _firstIcon;
            set => SetProperty(ref _firstIcon, value);
        }

        private string _secondIcon;
        public string SecondIcon
        {
            get => _secondIcon;
            set => SetProperty(ref _secondIcon, value);
        }

        private string _thirdIcon;
        public string ThirdIcon
        {
            get => _thirdIcon;
            set => SetProperty(ref _thirdIcon, value);
        }

        private string _fourstIcon;
        public string FourstIcon
        {
            get => _fourstIcon;
            set => SetProperty(ref _fourstIcon, value);
        }

        private string _firstTemp;
        public string FirstTemp
        {
            get => _firstTemp;
            set => SetProperty(ref _firstTemp, value);
        }

        private string _secondTemp;
        public string SecondTemp
        {
            get => _secondTemp;
            set => SetProperty(ref _secondTemp, value);
        }

        private string _thirdTemp;
        public string ThirdTemp
        {
            get => _thirdTemp;
            set => SetProperty(ref _thirdTemp, value);
        }

        private string _fourstTemp;
        public string FourstTemp
        {
            get => _fourstTemp;
            set => SetProperty(ref _fourstTemp, value);
        }

        private MapStyle _style;
        public MapStyle Style
        {
            get => _style;
            set => SetProperty(ref _style, value);
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

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                MakePinFocus(value);
            }

            Style = _resourceService.GetMapStyle();
            var pinsViewModel = await GetAllPinViewModelsAsync();

            UpdatePins(pinsViewModel);
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var isStatus = await _permissionService.CheckStatusAsync();

            if (isStatus)
            {
                IsUserPosition = true;
            }
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSearchPins()
        {
            var resultSearch = await SearchPins();

            UpdatePins(resultSearch); 
        }

        private void OnSelectedItemTap()
        {
            SearchText = string.Empty;

            OnMapTap();
            MakePinFocus(PinSelectedItem);
        }

        private void OnFocused()
        {
            TopBorder = new Rectangle(0, 0, 1.0, 0.31);
        }

        private void OnMapTap()
        {
            TopBorder = new Rectangle(0, 0, 1.0, 0);
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

            await navigationService.NavigateAsync($"{nameof(FirstView)}");
        }

        private async void OnPinTapAsync(Pin pin)
        {
            _pinViewModel = pin.ToPinViewModel();

            ShowPinInfo();
            await ShowWeatherAsync();

            BottomBorder = new Rectangle(0.5, 1.0, 1.0, 0.45);
        }

        private void OnPinInfoTap()
        {
            BottomBorder = new Rectangle(0, 1.0, 1.0, 0.0);
        }

        private void OnTimeButtonTapAsync()
        {
            OnPinInfoTap();

            var parameters = new DialogParameters();
            parameters.Add(Constants.PinViewModelKey, _pinViewModel);

             _dialogService.ShowDialog(nameof(ClockView), parameters);
        }

        private async void OnSettingsTapAsync(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(SettingsView)}");
        }

        #endregion

        #region --- Private Methods ---

        private async Task<List<PinViewModel>> SearchPins()
        {
            var pinViewModels = await GetAllPinViewModelsAsync();
            var pinList = pinViewModels.Where(x => x.Name.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower()).ToList();

            return pinList;
        }

        private void UpdatePins(List<PinViewModel> pinsViewModel)
        {
            Pins.Clear();
            var temp = new List<PinViewModel>();

            foreach (var item in pinsViewModel)
            {
                if (item.IsFavorit)
                {
                    temp.Add(item);
                }
            }

            Pins = new ObservableCollection<PinViewModel>(temp);
        }

        private void MakePinFocus(PinViewModel pinViewModel)
        {
            var position = new Position(pinViewModel.Latitude, pinViewModel.Longitude);

            Region = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(2));
        }

        private void ShowPinInfo()
        {
            Name = _pinViewModel.Name;
            Position = string.Format("{0}  {1}", _pinViewModel.Latitude.ToString(), _pinViewModel.Longitude.ToString());
        }

        private async Task ShowWeatherAsync()
        {
            var weatherList = await _weatherService.GetWeatherAsync(_pinViewModel.Latitude, _pinViewModel.Longitude);

            foreach(var item in weatherList)
            {
                WeatherCollection.Add(item);
            }
        }

        #endregion
    }
}
