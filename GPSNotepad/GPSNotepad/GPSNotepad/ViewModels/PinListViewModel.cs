using GPSNotepad.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class PinListViewModel : BaseViewModel
    {
        public PinListViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---

        public ICommand AddTapCommand => new Command(OnAddTap);

        #endregion

        #region --- Private Helpers ---

        private async void OnAddTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(AddPinView)}");
        }

        #endregion
    }
}
