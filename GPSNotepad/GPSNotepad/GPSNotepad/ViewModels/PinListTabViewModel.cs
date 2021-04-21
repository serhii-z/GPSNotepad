using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using GPSNotepad.Services.SettingsService;
using GPSNotepad.Views;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class PinListTabViewModel : BaseTabViewModel
    {
        private IAuthorizationService _authorizationService;

        public PinListTabViewModel(INavigationService navigationService,  
            IPinService pinService, ISettingsManager settingsManager, IAuthorizationService authorizationService) : 
            base(navigationService, pinService, settingsManager)
        {
            _authorizationService = authorizationService;

            Pins = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand SettingsTapCommandCommand => new Command(OnSettingsTap);

        private void OnSettingsTap(object obj)
        {
            //go to search page
        }
        public ICommand AddTapCommand => new Command(OnAddTapAsync);
        public ICommand SearchCommand => new Command(OnSearchPinsAsync);
        public ICommand ExitTapCommand => new Command(OnExitTapAsync);

        private ObservableCollection<PinViewModel> _pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get { return _pins; }
            set => SetProperty(ref _pins, value);
        }

        private PinViewModel _selectedItem;
        public PinViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(SelectedItem))
            {
                OnSelectedItemTapAsync();
            }

            if (args.PropertyName == nameof(SearchText) &&
                SearchCommand.CanExecute(null))
            {
                SearchCommand.Execute(null);
            }
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                Pins.Add(value);
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var pinList = await GetAllPinViewModelsAsync();

            InitPins(pinList);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnAddTapAsync(object obj)
        {
            await navigationService.NavigateAsync(nameof(AddPinView));
        }

        private async void OnSelectedItemTapAsync()
        {
            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, _selectedItem);

            await navigationService.NavigateAsync($"{nameof(MainTabbedView)}", parameters);
        }

        private async void OnSearchPinsAsync()
        {
            var pins = await GetAllPinViewModelsAsync();
            var resultSearch = SearchPins(pins);

            InitPins(resultSearch);
        }

        private async void OnExitTapAsync()
        {
            _authorizationService.LogOut();

            await navigationService.NavigateAsync($"{nameof(SignInView)}");
        }

        #endregion

        #region --- Private Methods ---

        private List<PinViewModel> SearchPins(List<PinViewModel> pins)
        {
            var resultName = pins.Where(x => x.Name.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower());
            var resultDescription = pins.Where(x => x.Description.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower());
            var pinList = resultName.Union(resultDescription).Distinct().ToList();

            return pinList;
        }

        private void InitPins(List<PinViewModel> pinList)
        {
            Pins.Clear();

            foreach (var item in pinList)
            {
                Pins.Add(item);
            }
        }

        #endregion
    }
}
