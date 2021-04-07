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
    public class PinListViewModel : BaseTabViewModel
    {
        public PinListViewModel(INavigationService navigationService, 
            IAuthorizationService authorizationService, 
            IPinService pinService) : base(navigationService, authorizationService, pinService)
        {
            PinViewModels = new ObservableCollection<PinViewModel>();
        }

        #region --- Public Properties ---

        public ICommand AddTapCommand => new Command(OnAddTap);

        private ObservableCollection<PinViewModel> _pinViewModels;
        public ObservableCollection<PinViewModel> PinViewModels
        {
            get { return _pinViewModels; }
            set => SetProperty(ref _pinViewModels, value);
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
            ShowPins(pinList);
        }

        #endregion

        #region --- Private Methods ---

        private void ShowPins(List<PinViewModel> pinList)
        {
            if (pinList.Count > 0)
            {
                PinViewModels.Clear();

                foreach (var item in pinList)
                {
                    PinViewModels.Add(item);
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
            var pinList = GetAllPins(authorizationService.UserId);

            return pinList;
        }



        #endregion
    }
}
