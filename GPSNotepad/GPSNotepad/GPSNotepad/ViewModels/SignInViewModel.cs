using GPSNotepad.Services.Authentication;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Views;
using Prism.Navigation;
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

        public SignInViewModel(INavigationService navigationService, 
            IAuthenticationService authenticationService, 
            IAuthorizationService authorizationService) : base(navigationService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        public ICommand GoBackTapCommand => new Command(OnGoBackTapCommandAsync);
        public ICommand LogInButtonTapCommand => new Command(OnLogInButtonTapAsync);    
        public ICommand ImageEntryClearTapCommand => new Command(OnImageEntryClearTapCommandAsync);
        public ICommand ImageEntryEyeTapCommand => new Command(OnImageEntryEyeTapCommand);


        private bool _isHidePassword;
        public bool IsHidePassword
        {
            get => _isHidePassword;
            set => SetProperty(ref _isHidePassword, value);
        }

        private ImageSource _clearSource;
        public ImageSource ClearSource
        {
            get => _clearSource;
            set => SetProperty(ref _clearSource, value);
        }

        private ImageSource _eyeSource;
        public ImageSource EyeSource
        {
            get => _eyeSource;
            set => SetProperty(ref _eyeSource, value);
        }

        private string _entryEmail;
        public string EntryEmail
        {
            get => _entryEmail;
            set => SetProperty(ref _entryEmail, value);
        }

        private string _labelEmailError;
        public string LabelEmailError
        {
            get => _labelEmailError;
            set => SetProperty(ref _labelEmailError, value);
        }

        private string _entryPassword;
        public string EntryPassword
        {
            get => _entryPassword;
            set => SetProperty(ref _entryPassword, value);
        }

        private string _labelPasswordError;
        public string LabelPasswordError
        {
            get => _labelPasswordError;
            set => SetProperty(ref _labelPasswordError, value);
        }

        private bool _isEnabledButton;
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        private bool _isErrorEmail;
        public bool IsErrorEmail
        {
            get => _isErrorEmail;
            set => SetProperty(ref _isErrorEmail, value);
        }

        private bool _isErrorPassword;
        public bool IsErrorPassword
        {
            get => _isErrorPassword;
            set => SetProperty(ref _isErrorPassword, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryEmail))
            {
                CheckTextInput(_entryEmail);
                ShowHideImageEmail(_entryEmail);
            }

            if (args.PropertyName == nameof(EntryPassword))
            {
                IsHidePassword = true;

                CheckTextInput(_entryPassword);
                ShowHideImagePassword(_entryPassword);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {            
            IsHidePassword = false;
            IsHidePassword = true;   
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            IsHidePassword = true;
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnGoBackTapCommandAsync()
        {
            await navigationService.GoBackAsync();
        }

        private async void OnLogInButtonTapAsync()
        {
            var isSuccess = await TryToSignInAsync();

            if (isSuccess)
            {
                await navigationService.NavigateAsync($"{nameof(MainTabbedView)}");
            }
        }

        #endregion

        #region --- Private Methods ---

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                IsEnabledButton = false;
            }
            else if(!string.IsNullOrEmpty(_entryEmail) &&
                    !string.IsNullOrEmpty(_entryPassword))
            {
                IsEnabledButton = true;
            }
        }

        private void ShowHideImageEmail(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                ClearSource = string.Empty;
            }
            else
            {
                ClearSource = Constants.ImageClear;
            }
        }

        private void ShowHideImagePassword(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                EyeSource = string.Empty;
            }
            else
            {
                EyeSource = Constants.ImageEyeOff;
            }
        }

        private async Task<bool> TryToSignInAsync()
        {
            var isSuccess = await _authenticationService.SignInAsync(_entryEmail, _entryPassword);

            if (!isSuccess)
            {
                IsErrorEmail = true;
                IsErrorPassword = true;
                LabelEmailError = (string)App.Current.Resources["LoginPasswordWrong"];
                LabelPasswordError = (string)App.Current.Resources["LoginPasswordWrong"];
            }

            return isSuccess;
        }

        private void OnImageEntryEyeTapCommand()
        {
            if (IsHidePassword)
            {
                EyeSource = Constants.ImageEye;
                IsHidePassword = false;
            }
            else
            {
                EyeSource = Constants.ImageEyeOff;
                IsHidePassword = true;
            }
        }

        private void OnImageEntryClearTapCommandAsync()
        {
            EntryEmail = string.Empty;
        }

        #endregion
    }
}
