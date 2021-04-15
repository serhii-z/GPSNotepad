using GPSNotepad.Models;
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
        public PinListTabViewModel(INavigationService navigationService,  
            IPinService pinService, ISettingsManager settingsManager) : 
            base(navigationService, pinService, settingsManager)
        {
            PinViewModels = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand AddTapCommand => new Command(OnAddTapAsync);
        public ICommand SearchCommand => new Command(OnSearchPinsAsync);

        private ObservableCollection<PinViewModel> _pinViewModels;
        public ObservableCollection<PinViewModel> PinViewModels
        {
            get { return _pinViewModels; }
            set => SetProperty(ref _pinViewModels, value);
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
                PinViewModels.Add(value);
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var pinList = await GetAllPinViewModelsAsync();

            InitCollection(pinList);
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

            InitCollection(resultSearch);
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

        private void InitCollection(List<PinViewModel> pinList)
        {
            PinViewModels.Clear();

            foreach (var item in pinList)
            {
                PinViewModels.Add(item);
            }
        }

        #endregion
    }
}
