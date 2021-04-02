using GPSNotepad.Services.Authentication;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Views;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IPageDialogService _pageDialog;
        public SignInViewModel(INavigationService navigationService, 
            IAuthenticationService authenticationService, 
            IAuthorizationService authorizationService, 
            IPageDialogService pageDialogService) : base(navigationService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _pageDialog = pageDialogService;
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

        private bool IsAuthorization()
        {
            var userId = _authenticationService.VerifyUser(_entryEmailText, _entryPasswordText);

            if (userId > 0)
            {
                _authorizationService.UserId = userId;
                return true;
            }

            ShowAlert("Invalid login or password!");
            EntryEmailText = string.Empty;
            EntryPasswordText = string.Empty;

            return false;
        }

        private async void ShowAlert(string message)
        {
            await _pageDialog.DisplayAlertAsync("Message", message, "OK");
        }

        #endregion

        #region --- Private Helpers ---
        private async void OnLogInTap()
        {
            if (IsAuthorization())
            {
                await navigationService.NavigateAsync($"{nameof(MainView)}");
            }
        }

        private async void OnSignUpTap()
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("login", out string login))
            {
                EntryEmailText = login;
            }
        }

        #endregion
    }
}
