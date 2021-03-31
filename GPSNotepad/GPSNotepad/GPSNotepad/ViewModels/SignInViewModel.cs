using GPSNotepad.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        public SignInViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---
        public ICommand LogInTapCommand => new Command(OnLogInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private string _entryEmailText;
        public string EntryEmailText
        {
            get => _entryEmailText;
            set => SetProperty(ref _entryEmailText, value);
        }

        private string _entryPasswordText;
        public string EntryPasswordText
        {
            get => _entryPasswordText;
            set => SetProperty(ref _entryPasswordText, value);
        }

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        #endregion

        #region --- Private Methods ---

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                DeactivateButton();
            }
            else
            {
                ActivateButton();
            }
        }

        private void ActivateButton()
        {
            if (!string.IsNullOrEmpty(_entryEmailText) &&
                !string.IsNullOrEmpty(_entryPasswordText))
                EnabledButton = true;
        }

        private void DeactivateButton()
        {
            EnabledButton = false;
        }

        #endregion

        #region --- Private Helpers ---
        private async void OnLogInTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(MainView)}");
        }

        private async void OnSignUpTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryEmailText))
            {
                CheckTextInput(_entryEmailText);
            }

            if (args.PropertyName == nameof(EntryPasswordText))
            {
                CheckTextInput(_entryPasswordText);
            }
        }

        #endregion
    }
}
