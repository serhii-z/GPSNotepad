using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using GPSNotepad.Views;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class PinListViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;
        private IPinService _pinService;

        public PinListViewModel(INavigationService navigationService, 
            IAuthorizationService authorizationService, 
            IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;
            PinList = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand AddTapCommand => new Command(OnAddTap);

        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinList
        {
            get { return _pinList; }
            set => SetProperty(ref _pinList, value);
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

        #endregion

        #region --- Private Methods ---

        private void ShowProfiles(List<PinViewModel> pinList)
        {
            if (pinList.Count > 0)
            {
                PinList.Clear();

                foreach (var item in pinList)
                {
                    PinList.Add(item);
                }

                IsNoPins = false;
            }
            else
            {
                IsNoPins = true;
            }
        }

        private List<PinViewModel> GetPins()
        {
            var pinList = _pinService.GetAllPins(_authorizationService.UserId);

            return pinList;
        }

        private void OnSelectedItemTap()
        {
            
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnAddTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(AddPinView)}");
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedItem))
            {
                OnSelectedItemTap();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var pinList = GetPins();
            ShowProfiles(pinList);
        }

        #endregion
    }
}
