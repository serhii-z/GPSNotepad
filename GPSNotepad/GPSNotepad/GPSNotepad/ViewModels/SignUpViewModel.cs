using GPSNotepad.Services.Authentication;
using GPSNotepad.Validators;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using GPSNotepad.Properties;
using System.Threading.Tasks;
using GPSNotepad.Views;

namespace GPSNotepad.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private IAuthenticationService _authenticationService;
        private IPageDialogService _pageDialog;
        private string _name;
        private string _email;

        public SignUpViewModel(INavigationService navigationService,
            IAuthenticationService authenticationService,
            IPageDialogService pageDialogService) : base(navigationService)
        {
            _authenticationService = authenticationService;
            _pageDialog = pageDialogService;
        }

        #region -- Public properties --

        public ICommand ImageLeftTapCommand => new Command(OnImageLeftTapCommandAsync);
        public ICommand CreateAccountButtonTapCommand => new Command(OnCreateAccountButtonTapAsync);
        public ICommand ImagePasswordTapCommand => new Command(OnImagePasswordTapCommand);

        private void OnImagePasswordTapCommand(object obj)
        {
            if (IsHidePassword)
            {
                EyePasswordSource = "ic_eye";
                IsHidePassword = false;
            }
            else
            {
                EyePasswordSource = "ic_eye_off";
                IsHidePassword = true;
            }
        }

        public ICommand ImageConfirmTapCommand => new Command(OnImageConfirmTapCommand);

        private void OnImageConfirmTapCommand(object obj)
        {
            if (IsHidePassword)
            {
                EyeConfirmPasswordSource = "ic_eye";
                IsHidePassword = false;
            }
            else
            {
                EyeConfirmPasswordSource = "ic_eye_off";
                IsHidePassword = true;
            }
        }

        private string _navBarTitle;
        public string NavBarTitle
        {
            get => _navBarTitle;
            set => SetProperty(ref _navBarTitle, value);
        }

        private ImageSource _eyePasswordSource;
        public ImageSource EyePasswordSource
        {
            get => _eyePasswordSource;
            set => SetProperty(ref _eyePasswordSource, value);
        }

        private ImageSource _eyeConfirmPasswordSource;
        public ImageSource EyeConfirmPasswordSource
        {
            get => _eyeConfirmPasswordSource;
            set => SetProperty(ref _eyeConfirmPasswordSource, value);
        }

        private Color _borderColorPassword;
        public Color BorderColorPassword
        {
            get => _borderColorPassword;
            set => SetProperty(ref _borderColorPassword, value);
        }

        private Color _borderColorConfirmPassword;
        public Color BorderColorConfirmPassword
        {
            get => _borderColorConfirmPassword;
            set => SetProperty(ref _borderColorConfirmPassword, value);
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

        private string _labelConfirmPassword;
        public string LabelConfirmPassword
        {
            get => _labelConfirmPassword;
            set => SetProperty(ref _labelConfirmPassword, value);
        }

        private string _entryConfitmPassword;
        public string EntryConfirmPassword
        {
            get => _entryConfitmPassword;
            set => SetProperty(ref _entryConfitmPassword, value);
        }

        private string _entryConfirmPlaceholder;
        public string EntryConfirmPlaceholder
        {
            get => _entryConfirmPlaceholder;
            set => SetProperty(ref _entryConfirmPlaceholder, value);
        }

        private string _labelConfirmPasswordError;
        public string LabelConfirmPasswordError
        {
            get => _labelConfirmPasswordError;
            set => SetProperty(ref _labelConfirmPasswordError, value);
        }

        private bool _isEnabledButton;
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        private bool _isHidePassword;
        public bool IsHidePassword
        {
            get => _isHidePassword;
            set => SetProperty(ref _isHidePassword, value);
        }

        #endregion   

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryPassword))
            {
                CheckTextInput(_entryPassword);
                ShowHideImagePassword(_entryPassword);
            }

            if (args.PropertyName == nameof(EntryConfirmPassword))
            {
                CheckTextInput(_entryConfitmPassword);
                ShowHideImageConfirmPassword(_entryConfitmPassword);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.NameKey, out string name))
            {
                _name = name;
            }
            if (parameters.TryGetValue(Constants.LoginKey, out string login))
            {
                _email = login;
            }
            IsHidePassword = false;
            IsHidePassword = true;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            NavBarTitle = Properties.Resource.NavBarTitleRegister;
            LabelPassword = Properties.Resource.LabelPassword;
            EntryPasswordPlaceholder = Properties.Resource.PasswordPlaceholder;     
            LabelConfirmPassword = Properties.Resource.LabelConfirmPassword;
            EntryConfirmPlaceholder = Properties.Resource.EntryConfirmPlaceholder;
            BorderColorPassword = (Color)App.Current.Resources["entryBorder"];
            BorderColorConfirmPassword = (Color)App.Current.Resources["entryBorder"];
            IsHidePassword = true;  
        }

        #endregion

        #region -- Private helpers --

        private async void OnImageLeftTapCommandAsync()
        {
            await navigationService.GoBackAsync();
        }

        private async void OnCreateAccountButtonTapAsync()
        {
            LabelPasswordError = string.Empty;
            LabelConfirmPasswordError = string.Empty;
            BorderColorPassword = (Color)App.Current.Resources["entryBorder"];
            BorderColorConfirmPassword = (Color)App.Current.Resources["entryBorder"];

            var isSuccess = await TrySignUpUserAsync();

            if (isSuccess)
            {
                await navigationService.NavigateAsync($"{nameof(SignInView)}");
            }
        }

        #endregion

        #region -- Private methods --

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                IsEnabledButton = false;
            }
            else if (!string.IsNullOrEmpty(_entryPassword) &&
                !string.IsNullOrEmpty(_entryConfitmPassword))
            {
                IsEnabledButton = true;
            }
        }

        private async Task<bool> TrySignUpUserAsync()
        {
            var isSuccess = CheckValidation();

            if (isSuccess)
            {
                isSuccess = await _authenticationService.SignUpAsync(_name, _email, _entryPassword);

                if (!isSuccess)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(Constants.LoginKey, Properties.Resource.LoginBusy);

                    await navigationService.GoBackAsync(parameters);
                }
            }

            return isSuccess;
        }

        private async void ShowAlert(string message)
        {
            await _pageDialog.DisplayAlertAsync(Properties.Resource.AlertTitle, message, Properties.Resource.AlertOK);
        }

        private bool CheckValidation()
        {
            var isSuccess = true;

            if (!StringValidator.CheckQuantity(_entryPassword, 8))
            {
                LabelPasswordError = Properties.Resource.PasswordCountingSimbols;
                BorderColorPassword = Color.Red;
                LabelPasswordError = Properties.Resource.LabelPasswordError;
                isSuccess = false;
            }
            if (!StringValidator.CheckPresence(_entryPassword) && isSuccess)
            {
                LabelPasswordError = Properties.Resource.PasswordPresence;
                BorderColorPassword = Color.Red;
                LabelPasswordError = Properties.Resource.LabelPasswordError;
                isSuccess = false;
            }
            if (!StringValidator.CheckPasswordEquality(_entryPassword, _entryConfitmPassword) && isSuccess)
            {
                LabelConfirmPasswordError = Properties.Resource.PasswordConfirmNotEqual;
                BorderColorConfirmPassword = Color.Red;
                LabelConfirmPasswordError = Properties.Resource.LabelConfirmPasswordError;
                isSuccess = false;
            }

            return isSuccess;
        }

        private void ShowHideImagePassword(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                EyePasswordSource = string.Empty;
            }
            else
            {
                EyePasswordSource = "ic_eye_off";
            }
        }

        private void ShowHideImageConfirmPassword(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                EyeConfirmPasswordSource = string.Empty;
            }
            else
            {
                EyeConfirmPasswordSource = "ic_eye_off";
            }
        }

        #endregion
    }
}
