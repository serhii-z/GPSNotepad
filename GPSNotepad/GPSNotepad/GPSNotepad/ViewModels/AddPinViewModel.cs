using GPSNotepad.Extensions;
using GPSNotepad.Models;
using GPSNotepad.Services.GeoLocations;
using GPSNotepad.Services.Permissions;
using GPSNotepad.Services.Pin;
using GPSNotepad.Services.Resources;
using Prism.Navigation;
using System.Collections.Generic;
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

        private string _navBarTitle;
        public string NavBarTitle
        {
            get => _navBarTitle;
            set => SetProperty(ref _navBarTitle, value);
        }

        private Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private Color _borderColorName;
        public Color BorderColorName
        {
            get => _borderColorName;
            set => SetProperty(ref _borderColorName, value);
        }

        private Color _borderColorDescription;
        public Color BorderColorDescription
        {
            get => _borderColorDescription;
            set => SetProperty(ref _borderColorDescription, value);
        }


        private string _labelName;
        public string LabelName
        {
            get => _labelName;
            set => SetProperty(ref _labelName, value);
        }

        private string _entryName = string.Empty;
        public string EntryName
        {
            get => _entryName;
            set => SetProperty(ref _entryName, value);
        }

        private string _entryPlaceholder;
        public string EntryPlaceholder
        {
            get => _entryPlaceholder;
            set => SetProperty(ref _entryPlaceholder, value);
        }

        private string _labelDescription;
        public string LabelDescription
        {
            get => _labelDescription;
            set => SetProperty(ref _labelDescription, value);
        }

        private string _entryDescription = string.Empty;
        public string EntryDescription
        {
            get => _entryDescription;
            set => SetProperty(ref _entryDescription, value);
        }

        private string _entryDescriptionPlaceholder;
        public string EntryDescriptionPlaceholder
        {
            get => _entryDescriptionPlaceholder;
            set => SetProperty(ref _entryDescriptionPlaceholder, value);
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

        private bool _isVisibleImageName;
        public bool IsVisibleImageName
        {
            get => _isVisibleImageName;
            set => SetProperty(ref _isVisibleImageName, value);
        }

        private bool _isVisibleImageDescription;
        public bool IsVisibleImageDescription
        {
            get => _isVisibleImageDescription;
            set => SetProperty(ref _isVisibleImageDescription, value);
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
            NavBarTitle = (string)App.Current.Resources["NavBarTitleAddPin"];
            LabelName = (string)App.Current.Resources["Label"];
            LabelDescription = (string)App.Current.Resources["LabelDescription"];
            EntryPlaceholder = (string)App.Current.Resources["EntryPlaceholderLabel"];           
            EntryDescriptionPlaceholder = (string)App.Current.Resources["EntryDescriptionPlaceholder"];
            BorderColorName = Color.Gray;
            BorderColorDescription = Color.Gray;
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

        private void OnImageDescriptionClearTapCommand(object obj)
        {
            EntryDescription = string.Empty;
        }

        private void OnImageNameClearTapCommand(object obj)
        {
            EntryName = string.Empty;
        }

        private async void OnGoBackTapCommandAsync(object obj)
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

        #endregion
    }
}
