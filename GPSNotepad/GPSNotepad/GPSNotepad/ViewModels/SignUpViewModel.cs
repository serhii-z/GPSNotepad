using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public SignUpViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        #endregion

        #region ---- Private Helpers ---

        private void OnSignUpTap(object obj)
        {
            
        }

        #endregion
    }
}
