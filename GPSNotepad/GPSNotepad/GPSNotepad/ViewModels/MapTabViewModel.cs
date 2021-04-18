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

namespace GPSNotepad.ViewModels
{
    public class MapTabViewModel : BaseTabViewModel
    {
        private IPermissionService _permissionService;
        private IGeoLocationService _geoLocationService;
        private IAuthorizationService _authorizationService;
        public MapTabViewModel(INavigationService navigationService, IPinService pinService, 
            ISettingsManager settingsManager, PermissionService permissionService, 
            IGeoLocationService geoLocationService, IAuthorizationService authorizationService) : 
            base(navigationService, pinService, settingsManager)
        {
            _permissionService = permissionService;
            _geoLocationService = geoLocationService;
            _authorizationService = authorizationService;

            Pins = new ObservableCollection<Pin>();   
        }

        #region --- Public Properties ---

        public ICommand SearchBarTappedCommand => new Command(OnSearchBarTap);
        public ICommand PinTapCommand => new Command<Pin>(OnPinTap);
        public ICommand PinSearchCommand => new Command(OnSearchPins);
        public ICommand MapTapCommand => new Command(OnMapTap);
        public ICommand LocationButtonTapCommand => new Command(OnLocationButtonTapAsync);
        public ICommand LogOutButtonTapCommand => new Command(OnLogOutButtonTapAsync);

        private bool _isUserPosition = true;
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

            TopBorder = new Rectangle(0.0, 0.12, 1, 0.0);

            var isStatus = await _permissionService.CheckStatusAsync();
            var pinViewModels = await GetAllPinViewModelsAsync();

            InitPins(pinViewModels);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnPinTap(Pin pin)
        {
            var pinViewModel = pin.ToPinViewModel();
            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, pinViewModel);

            await navigationService.NavigateAsync(nameof(PinInfoView), parameters, useModalNavigation: true);
        }

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

        private async void OnLogOutButtonTapAsync()
        {
            _authorizationService.LogOut();

            await navigationService.NavigateAsync($"{nameof(SignInView)}");
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

        #endregion
    }
}
