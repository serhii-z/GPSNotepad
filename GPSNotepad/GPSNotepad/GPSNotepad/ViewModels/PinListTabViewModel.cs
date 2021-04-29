using GPSNotepad.Extensions;
using GPSNotepad.Models;
using GPSNotepad.Properties;
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
        public ICommand EditTapCommand => new Command(OnEditTapAsync);
        public ICommand DeleteTapCommand => new Command(OnDeleteTap);
        public ICommand ImageRightTapCommand => new Command<object>(OnImageRightTapCommandAsync);
        public ICommand ImageLikeTapCommand => new Command<object>(OnImageLikeTapCommandAsync);


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

            var pinsViewModel = await GetAllPinViewModelsAsync();

            UpdatePins(pinsViewModel);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnAddTapAsync(object obj)
        {
            await navigationService.NavigateAsync(nameof(AddPinView));
        }

        private async void OnSearchPinsAsync()
        {
            var pins = await GetAllPinViewModelsAsync();
            var resultSearch = SearchPins(pins);

            UpdatePins(resultSearch);
        }

        private async void OnImageRightTapCommandAsync(object obj)
        {
            var pinViewModel = obj as PinViewModel;
            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, pinViewModel);

            await navigationService.NavigateAsync($"{nameof(MainTabbedView)}", parameters);
        }

        private async void OnExitTapAsync()
        {
            _authorizationService.LogOut();

            await navigationService.NavigateAsync($"{nameof(FirstView)}");
        }

        private async void OnEditTapAsync(object obj)
        {
            var pinViewModel = obj as PinViewModel;
            var parameters = new NavigationParameters();
            parameters.Add(Constants.PinViewModelKey, pinViewModel);

            await navigationService.NavigateAsync(nameof(AddPinView), parameters);
        }

        private async void OnDeleteTap(object obj)
        {
            bool isDialogYes = await _pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle,
                Properties.Resource.AlertMessage, Properties.Resource.AlertYes, Properties.Resource.AlertNo);

            if (isDialogYes)
            {
                var pinViewModel = obj as PinViewModel;
                var answer = await pinService.DeletePinAsync(pinViewModel.ToPinModel());

                if (answer == 1)
                {
                    Pins.Remove(pinViewModel);
                }      
            }
        }

        private async void OnImageLikeTapCommandAsync(object obj)
        {
            var pinViewModel = obj as PinViewModel;

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

            var pinModel = pinViewModel.ToPinModel();

            await SaveChangeAsync(pinModel);
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

        private async Task SaveChangeAsync(PinModel pinModel)
        {
            await pinService.UpdatePinAsync(pinModel);
        }

        #endregion
    }
}
