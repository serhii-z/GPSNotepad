using GPSNotepad.Extensions;
using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using GPSNotepad.Services.SettingsService;
using GPSNotepad.Views;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class PinListTabViewModel : BaseTabViewModel
    {
        private IAuthorizationService _authorizationService;
        private IPageDialogService _pageDialog;

        public PinListTabViewModel(INavigationService navigationService, IPinService pinService, 
            ISettingsManager settingsManager, IAuthorizationService authorizationService, IPageDialogService pageDialog) : 
            base(navigationService, pinService, settingsManager)
        {
            _authorizationService = authorizationService;
            _pageDialog = pageDialog;

            Pins = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand SettingsTapCommand => new Command(OnSettingsTapAsync);
        public ICommand AddTapCommand => new Command(OnAddTapAsync);
        public ICommand SearchCommand => new Command(OnSearchPinsAsync);
        public ICommand ExitTapCommand => new Command(OnExitTapAsync);
        public ICommand EditTapCommand => new Command<PinViewModel>(OnEditTapAsync);
        public ICommand DeleteTapCommand => new Command<PinViewModel>(OnDeleteTap);
        public ICommand RightTapCommand => new Command<PinViewModel>(OnRightTapCommandAsync);
        public ICommand LikeTapCommand => new Command<PinViewModel>(OnLikeTapCommandAsync);


        private ObservableCollection<PinViewModel> _pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get { return _pins; }
            set => SetProperty(ref _pins, value);
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
            if (args.PropertyName == nameof(SearchText) &&
                SearchCommand.CanExecute(null))
            {
                SearchCommand.Execute(null);
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                Pins.Add(value);
            }

            var pinsViewModel = await GetAllPinViewModelsAsync();

            UpdatePins(pinsViewModel);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnAddTapAsync()
        {
            await navigationService.NavigateAsync(nameof(AddPinView));
        }

        private async void OnSearchPinsAsync()
        {
            var pins = await GetAllPinViewModelsAsync();
            var resultSearch = SearchPins(pins);

            UpdatePins(resultSearch);
        }

        private async void OnRightTapCommandAsync(PinViewModel pinViewModel)
        {
            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, pinViewModel);

            await navigationService.NavigateAsync($"{nameof(MainTabbedView)}", parameters);
        }

        private async void OnExitTapAsync()
        {
            _authorizationService.LogOut();

            await navigationService.NavigateAsync($"{nameof(FirstView)}");
        }

        private async void OnEditTapAsync(PinViewModel pinViewModel)
        {
            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, pinViewModel);

            await navigationService.NavigateAsync(nameof(AddPinView), parameters);
        }

        private async void OnDeleteTap(PinViewModel pinViewModel)
        {
            bool isDialogYes = await _pageDialog.DisplayAlertAsync((string)App.Current.Resources["AlertTitle"],
                (string)App.Current.Resources["AlertMessage"], (string)App.Current.Resources["AlertYes"], (string)App.Current.Resources["AlertNo"]);

            if (isDialogYes)
            {
                var answer = await pinService.DeletePinAsync(pinViewModel.ToPinModel());

                if (answer == 1)
                {
                    Pins.Remove(pinViewModel);
                }      
            }
        }

        private async void OnLikeTapCommandAsync(PinViewModel pinViewModel)
        {
            if (pinViewModel.IsFavorit)
            {
                pinViewModel.ImagePath = Constants.ImageLikeGray;
                pinViewModel.IsFavorit = false;
            }
            else
            {
                pinViewModel.ImagePath = Constants.ImageLikeBlue;
                pinViewModel.IsFavorit = true;
            }

            await UpdateChangeAsync(pinViewModel);
        }

        private async void OnSettingsTapAsync()
        {
            await navigationService.NavigateAsync($"{nameof(SettingsView)}");
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

        private void UpdatePins(List<PinViewModel> pinList)
        {
            Pins.Clear();

            foreach (var item in pinList)
            {
                Pins.Add(item);
            }
        }

        private async Task UpdateChangeAsync(PinViewModel pinViewModel)
        {
            var pinModel = pinViewModel.ToPinModel();

            await pinService.UpdatePinAsync(pinModel);
        }

        #endregion
    }
}
