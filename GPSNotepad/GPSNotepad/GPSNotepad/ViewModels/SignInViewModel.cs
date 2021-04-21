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

        public ICommand ImageLeftTapCommand => new Command(OnImageLeftTapCommandAsync);
        public ICommand LogInButtonTapCommand => new Command(OnLogInButtonTapAsync);    
        public ICommand ImageEntryClearTapCommand => new Command(OnImageEntryClearTapCommandAsync);
        public ICommand ImageEntryEyeTapCommand => new Command(OnImageEntryEyeTapCommand);


        private bool _isHidePassword;
        public bool IsHidePassword
        {
            get => _isHidePassword;
            set => SetProperty(ref _isHidePassword, value);
        }

        private string _navBarTitle;
        public string NavBarTitle
        {
            get => _navBarTitle;
            set => SetProperty(ref _navBarTitle, value);
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

        private string _labelEmail;
        public string LabelEmail
        {
            get => _labelEmail;
            set => SetProperty(ref _labelEmail, value);
        }

        private string _entryEmail;
        public string EntryEmail
        {
            get => _entryEmail;
            set => SetProperty(ref _entryEmail, value);
        }

        private string _entryEmailPlaceholder;
        public string EntryEmailPlaceholder
        {
            get => _entryEmailPlaceholder;
            set => SetProperty(ref _entryEmailPlaceholder, value);
        }

        private string _labelEmailError;
        public string LabelEmailError
        {
            get => _labelEmailError;
            set => SetProperty(ref _labelEmailError, value);
        }

        private string _labelPassword;
        public string LabelPassword
        {
            get => _labelPassword;
            set => SetProperty(ref _labelPassword, value);
        }

        private string _entryPassword;
        public string EntryPassword
        {
            get => _entryPassword;
            set => SetProperty(ref _entryPassword, value);
        }

        private string _entryPasswordPlaceholder;
        public string EntryPasswordPlaceholder
        {
            get => _entryPasswordPlaceholder;
            set => SetProperty(ref _entryPasswordPlaceholder, value);
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

        private Color _borderColor;
        public Color BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        private bool _isVisibleEmailError;
        public bool IsVisibleEmailError
        {
            get => _isVisibleEmailError;
            set => SetProperty(ref _isVisibleEmailError, value);
        }

        private bool _isVisiblePasswordError;
        public bool IsVisiblePasswordError
        {
            get => _isVisiblePasswordError;
            set => SetProperty(ref _isVisiblePasswordError, value);
        }

        private bool _isVisibleImageEmail;
        public bool IsVisibleImageEmail
        {
            get => _isVisibleImageEmail;
            set => SetProperty(ref _isVisibleImageEmail, value);
        }

        private bool _isVisibleImagePassword;
        public bool IsVisibleImagePassword
        {
            get => _isVisibleImagePassword;
            set => SetProperty(ref _isVisibleImagePassword, value);
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
            if (parameters.TryGetValue(Constants.LoginKey, out string login))
            {
                EntryEmail = login;
            }

            IsHidePassword = false;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            NavBarTitle = Resources.NavBarTitleLogIn;
            LabelEmail = Resources.LabelEmail;
            EntryEmailPlaceholder = Resources.EntryPlaseholderEmail;
            LabelEmailError = Resources.LabelEmailError;
            LabelPassword = Resources.LabelPassword;
            EntryPasswordPlaceholder = Resources.EntryPasswordPlaceholder;
            LabelPasswordError = Resources.LabelPasswordError;
            BorderColor = Color.Gray;
            IsVisibleEmailError = false;
            IsVisiblePasswordError = false;
            IsHidePassword = true;
            ClearSource = "ic_clear";
            EyeSource = "ic_eye_off";
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnImageLeftTapCommandAsync()
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
                IsVisibleImageEmail = false;
            }
            else
            {
                IsVisibleImageEmail = true;
            }   
        }

        private void ShowHideImagePassword(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                IsVisibleImagePassword = false;
            }
            else
            {
                IsVisibleImagePassword = true;
            }
        }

        private async Task<bool> TryToSignInAsync()
        {
            var isSuccess = await _authenticationService.SignInAsync(_entryEmail, _entryPassword);

            if (!isSuccess)
            {
                IsVisibleEmailError = true;
                IsVisiblePasswordError = true;
                EntryEmail = string.Empty;
                EntryPassword = string.Empty;
            }

            return isSuccess;
        }

        private void OnImageEntryEyeTapCommand()
        {
            if (IsHidePassword)
            {
                EyeSource = "ic_eye";
                IsHidePassword = false;
            }
            else
            {
                EyeSource = "ic_eye_off";
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
