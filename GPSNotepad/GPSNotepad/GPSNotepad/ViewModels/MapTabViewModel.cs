using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using GPSNotepad.Views;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.ViewModels
{
    public class MapTabViewModel : BaseTabViewModel
    {
        public MapTabViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService) : base(navigationService, authorizationService, pinService)
        {
            Pins = new ObservableCollection<Pin>();
            PinCollection = new ObservableCollection<PinViewModel>();
            TopBorder = new Rectangle(0.0, 0.12, 1, 0.0);
        }

        #region --- Public Properties ---

        public ICommand SearchBarTappedCommand => new Command(OnSearchBarTap);

        public ICommand PinTappedCommand => new Command<Pin>(OnPinTap);
        public ICommand PinSearchCommand => new Command(OnSearchPins);
        public ICommand MapTappedCommand => new Command(OnMapTap);

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private ObservableCollection<PinViewModel> _pinCollection;
        public ObservableCollection<PinViewModel> PinCollection
        {
            get { return _pinCollection; }
            set => SetProperty(ref _pinCollection, value);
        }

        private object _pinSelectedItem;
        public object PinSelectedItem
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

        private bool _animated = true;
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

            if (parameters.TryGetValue("pinViewModel", out PinViewModel value))
            {
                MakePinFocus(value);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var pinViewModels = GetAllPins(authorizationService.UserId);

            InitPins(pinViewModels);
            InitPinsTitle(pinViewModels);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnPinTap(Pin pin)
        {
            var pinViewModel = FindPin(pin);
            var parameters = new NavigationParameters();

            parameters.Add("pinViewModel", pinViewModel);

            await navigationService.NavigateAsync($"{nameof(PinInfoView)}", parameters, useModalNavigation: true);
        }

        private void OnSearchPins()
        {
            var resultSearch = SearchPins();

            InitPinsTitle(resultSearch);
            OnSearchBarTap();
        }

        private void OnSelectedItemTap()
        {
            var pin = _pinSelectedItem as PinViewModel;

            OnMapTap();
            MakePinFocus(pin);
        }

        private void OnSearchBarTap()
        {
            TopBorder = new Rectangle(0.0, 0.12, 1, 0.31);
        }

        private void OnMapTap()
        {
            TopBorder = new Rectangle(0.0, 0.12, 1, 0.0);
        }

        #endregion

        #region --- Private Methods ---

        private PinViewModel FindPin(Pin pin)
        {
            var pinViewModels = GetAllPins(authorizationService.UserId);
            var pinViewModel = pinViewModels.Where(x => x.PinId == pin.ZIndex).FirstOrDefault();

            return pinViewModel;
        }

        private Pin CreatePin(PinViewModel pinViewModel)
        {
            var pin = new Pin
            {
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                Label = pinViewModel.Name,
                ZIndex = pinViewModel.PinId
            };

            return pin;
        }

        private List<PinViewModel> SearchPins()
        {
            var pinViewModels = GetAllPins(authorizationService.UserId);
            var pinList = pinViewModels.Where(x => x.Name.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower()).ToList();

            return pinList;
        }

        private void InitPins(List<PinViewModel> pins)
        {
            Pins.Clear();

            foreach (var item in pins)
            {
                Pins.Add(CreatePin(item));
            }
        }

        private void InitPinsTitle(List<PinViewModel> pins)
        {
            PinCollection.Clear();

            foreach (var item in pins)
            {
                PinCollection.Add(item);
            }
        }

        private void MakePinFocus(PinViewModel pinViewModel)
        {
            var pin = CreatePin(pinViewModel);

            Region = MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(2));
        }

        #endregion
    }
}
