using GPSNotepad.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class PinInfoViewModel : BaseViewModel
    {
        public PinInfoViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---

        public ICommand GoBackTapCommand => new Command(OnGoBackTap);

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnGoBackTap()
        {
            await navigationService.GoBackAsync(useModalNavigation: true);
        }

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("pinViewModel", out PinViewModel value))
            {
                Name = value.Name;
                Description = value.Description;
            }
        }

        #endregion
    }
}
