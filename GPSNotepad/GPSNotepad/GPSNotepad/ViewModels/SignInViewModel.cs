using GPSNotepad.Properties;
using GPSNotepad.Services.Authentication;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Views;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;
using System.Threading.Tasks;
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
        public ICommand SignInButtonTapCommand => new Command(OnSignInButtonTapAsync);
        public ICommand SignUpTapCommand => new Command(OnSignUpTapAsync);

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
            if (parameters.TryGetValue(Constants.LoginKey, out string login))
            {
                EntryEmailText = login;
            }
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSignInButtonTapAsync()
        {
            var isSuccess = await TryToSignInAsync();

            if (isSuccess)
            {
                await navigationService.NavigateAsync($"{nameof(MainTabbedView)}");
            }
        }

        private async void OnSignUpTapAsync()
        {
            await navigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        #endregion

        #region --- Private Methods ---

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                EnabledButton = false;
            }
            else if(!string.IsNullOrEmpty(_entryEmailText) &&
                    !string.IsNullOrEmpty(_entryPasswordText))
            {
                EnabledButton = true;
            }
        }
        
        private async Task<bool> TryToSignInAsync()
        {
            var isSuccess = await _authenticationService.SignInAsync(_entryEmailText, _entryPasswordText);

            if (!isSuccess)
            {
                ShowAlertAsync(Resources.SignInInvalidLoginPassword);
                EntryEmailText = string.Empty;
                EntryPasswordText = string.Empty;
            }

            return isSuccess;
        }

        private async void ShowAlertAsync(string message)
        {
            await _pageDialog.DisplayAlertAsync(Resources.AlertTitle, message, Resources.AlertOK);
        }

        #endregion
    }
}
