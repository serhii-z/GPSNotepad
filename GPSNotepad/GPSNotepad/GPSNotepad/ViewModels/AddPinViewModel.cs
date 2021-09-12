using GPSNotepad.Extensions;
using GPSNotepad.Models;
using GPSNotepad.Services.GeoLocations;
using GPSNotepad.Services.Permissions;
using GPSNotepad.Services.Pin;
using GPSNotepad.Services.Resources;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.ViewModels
{
    public class AddPinViewModel : BaseViewModel
    {  
        private IPinService _pinService;
        private IGeoLocationService _geoLocationService;
        private IPermissionService _permissionService;
        private IResourceService _resourceService;

        public AddPinViewModel(INavigationService navigationService, IPinService pinService, 
            IGeoLocationService geoLocationService, IPermissionService permissionService, 
            IResourceService resourceService) : base(navigationService)
        {
            _pinService = pinService;
            _geoLocationService = geoLocationService;
            _permissionService = permissionService;
            _resourceService = resourceService;
        }

        #region -- Public properties --

        public ICommand GoBackTapCommand => new Command(OnGoBackTapCommandAsync);
        public ICommand SaveTapCommand => new Command(OnSaveTapAsync);
        public ICommand MapTapCommand => new Command<Position>(OnMapTap);
        public ICommand ImageNameClearTapCommand => new Command(OnImageNameClearTapCommand);
        public ICommand ImageDescriptionClearTapCommand => new Command(OnImageDescriptionClearTapCommand);
        public ICommand LocationButtonTapCommand => new Command(OnLocationButtonTapAsync);

        private ObservableCollection<PinViewModel> _pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private bool _isUserPosition;
        public bool IsUserPosition
        {
            get => _isUserPosition;
            set => SetProperty(ref _isUserPosition, value);
        }

        private string _entryName = string.Empty;
        public string EntryName
        {
            get => _entryName;
            set => SetProperty(ref _entryName, value);
        }

        private string _entryDescription = string.Empty;
        public string EntryDescription
        {
            get => _entryDescription;
            set => SetProperty(ref _entryDescription, value);
        }

        private string _entryLatitude;
        public string EntryLatitude
        {
            get => _entryLatitude;
            set => SetProperty(ref _entryLatitude, value);
        }

        private string _entryLongitude;
        public string EntryLongitude
        {
            get => _entryLongitude;
            set => SetProperty(ref _entryLongitude, value);
        }

        private ImageSource _clearSourceName;
        public ImageSource ClearSourceName
        {
            get => _clearSourceName;
            set => SetProperty(ref _clearSourceName, value);
        }

        private ImageSource _clearSourceDescription;
        public ImageSource ClearSourceDescription
        {
            get => _clearSourceDescription;
            set => SetProperty(ref _clearSourceDescription, value);
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

        private MapStyle _style;
        public MapStyle Style
        {
            get => _style;
            set => SetProperty(ref _style, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryName))
            {
                ShowHideImageName(_entryName);
            }

            if (args.PropertyName == nameof(EntryDescription))
            {
                ShowHideImageDescription(_entryDescription);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                EntryName = value.Name;
                EntryLatitude = value.Latitude.ToString();
                EntryLongitude = value.Longitude.ToString();
                EntryDescription = value.Description;
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var isStatus = await _permissionService.CheckStatusAsync();

            if (isStatus)
            {
                IsUserPosition = true;
            }

            Style = _resourceService.GetMapStyle();
        }

        #endregion

        #region -- Private helpers --

        private void OnMapTap(Position position)
        {
            EntryLatitude = position.Latitude.ToString();
            EntryLongitude = position.Longitude.ToString();

            var pinViewModel = CreatePinViewModel();

            UpdatePins(pinViewModel);
        }

        private async void OnSaveTapAsync()
        {
            if (!string.IsNullOrEmpty(EntryName) &&
                !string.IsNullOrEmpty(EntryLatitude) &&
                !string.IsNullOrEmpty(EntryLongitude) &&
                !string.IsNullOrEmpty(EntryDescription))
            {
                var pinViewmodel = await AddPinAsync();
                var parameters = new NavigationParameters();
                parameters.Add(Constants.PinViewModelKey, pinViewmodel);

                await navigationService.GoBackAsync(parameters);
            }
        }

        private void OnImageDescriptionClearTapCommand()
        {
            EntryDescription = string.Empty;
        }

        private void OnImageNameClearTapCommand()
        {
            EntryName = string.Empty;
        }

        private async void OnGoBackTapCommandAsync()
        {
            await navigationService.GoBackAsync();
        }

        private async void OnLocationButtonTapAsync()
        {
            var position = await _geoLocationService.GetUserPositionAsync();

            Animated = true;
            Region = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(2));
        }

        #endregion

        #region -- Private methods --

        private void UpdatePins(PinViewModel pinViewModel)
        {
            var temp = new List<PinViewModel>();
            temp.Add(pinViewModel);

            Pins = new ObservableCollection<PinViewModel>(temp);       
        }

        private async Task<PinViewModel> AddPinAsync()
        {
            var pinViewModel = CreatePinViewModel();

            await _pinService.AddPinAsync(pinViewModel.ToPinModel());

            return pinViewModel;
        }

        private PinViewModel CreatePinViewModel() 
        {
            var pinViewModel = new PinViewModel
            {
                Name = EntryName,
                Latitude = double.Parse(EntryLatitude),
                Longitude = double.Parse(EntryLongitude),
                Description = EntryDescription,
                ImagePath = Constants.ImageLikeGray,
                IsFavorit = false
            };

            return pinViewModel;
        }

        private void ShowHideImageName(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                ClearSourceName = string.Empty;
            }
            else
            {
                ClearSourceName = Constants.ImageClear;
            }
        }

        private void ShowHideImageDescription(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                ClearSourceDescription = string.Empty;
            }
            else
            {
                ClearSourceDescription = Constants.ImageClear;
            }
        }

        #endregion
    }
}
