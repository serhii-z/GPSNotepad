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

namespace GPSNotepad.ViewModels
{
    public class PinListViewModel : BaseTabViewModel
    {
        public PinListViewModel(INavigationService navigationService, 
            IAuthorizationService authorizationService, 
            IPinService pinService) : base(navigationService, authorizationService, pinService)
        {
            PinCollection = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand AddTapCommand => new Command(OnAddTap);
        public ICommand SearchCommand => new Command(OnSearchPins);

        private ObservableCollection<PinViewModel> _pinCollection;
        public ObservableCollection<PinViewModel> PinCollection
        {
            get { return _pinCollection; }
            set => SetProperty(ref _pinCollection, value);
        }

        private bool _isNoPins;
        public bool IsNoPins
        {
            get => _isNoPins;
            set => SetProperty(ref _isNoPins, value);
        }

        private object _selectedItem;
        public object SelectedItem
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

        #region --- Private Helpers ---

        private async void OnAddTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(AddPinView)}");
        }

        private async void OnSelectedItemTap()
        {
            var item = _selectedItem as PinViewModel;
            var parameters = new NavigationParameters();

            parameters.Add("pinViewModel", item);

            await navigationService.NavigateAsync($"{nameof(MainView)}", parameters);
        }

        private void OnSearchPins()
        {
            var pins = GetPins();
            var resultSearch = SearchPins(pins);

            InitCollection(resultSearch);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(SelectedItem))
            {
                OnSelectedItemTap();
            }

            if (args.PropertyName == nameof(SearchText))
            {
                if (SearchCommand.CanExecute(null))
                {
                    SearchCommand.Execute(null);
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var pinList = GetPins();

            InitCollection(pinList);
        }

        #endregion

        #region --- Private Methods ---

        private List<PinViewModel> GetPins()
        {
            var pinList = GetAllPins(authorizationService.UserId);

            return pinList;
        }

        private List<PinViewModel> SearchPins(List<PinViewModel> pins)
        {
            var resultName = pins.Where(x => x.Name.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower());
            var resultDescription = pins.Where(x => x.Description.ToLower().Substring(0, _searchText.Length) == _searchText.ToLower());
            var pinList = resultName.Union(resultDescription).Distinct().ToList();

            return pinList;
        }

        private void InitCollection(List<PinViewModel> pinList)
        {
            PinCollection.Clear();

            foreach (var item in pinList)
            {
                PinCollection.Add(item);
            }
        }

        #endregion
    }
}
