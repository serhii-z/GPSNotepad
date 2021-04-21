using GPSNotepad.Properties;
using GPSNotepad.Services.Authentication;
using GPSNotepad.Validators;
using GPSNotepad.Views;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class StartSignUpViewModel : BaseViewModel
    {
        private IAuthenticationService _authenticationService;

        public StartSignUpViewModel(INavigationService navigationService,
            IAuthenticationService authenticationService) : base(navigationService)
        {
            _authenticationService = authenticationService;
        }

        #region -- Public properties --

        public ICommand ImageLeftTapCommand => new Command(OnImageLeftTapCommandAsync);
        public ICommand NextButtonTapCommand => new Command(OnNextButtonTapCommandAsync);
        public ICommand ImageNameClearTapCommand => new Command(OnImageNameClearTapCommandAsync);
        public ICommand ImageEmailClearTapCommand => new Command(OnImageEmailClearTapCommandAsync);

        private string _navBarTitle;
        public string NavBarTitle
        {
            get => _navBarTitle;
            set => SetProperty(ref _navBarTitle, value);
        }

        private ImageSource _clearSourceName;
        public ImageSource ClearSourceName
        {
            get => _clearSourceName;
            set => SetProperty(ref _clearSourceName, value);
        }

        private ImageSource _cliarSourceEmail;
        public ImageSource ClearSourceEmail
        {
            get => _cliarSourceEmail;
            set => SetProperty(ref _cliarSourceEmail, value);
        }

        private Color _borderColor;
        public Color BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        private string _labelName;
        public string LabelName
        {
            get => _labelName;
            set => SetProperty(ref _labelName, value);
        }

        private string _entryName;
        public string EntryName
        {
            get => _entryName;
            set => SetProperty(ref _entryName, value);
        }

        private string _entryNamePlaceholder;
        public string EntryNamePlaceholder
        {
            get => _entryNamePlaceholder;
            set => SetProperty(ref _entryNamePlaceholder, value);
        }

        private string _labelNameError;
        public string LabelNameError
        {
            get => _labelNameError;
            set => SetProperty(ref _labelNameError, value);
        }

        //

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

        //

        private bool _isVisibleNameError;
        public bool IsVisibleNameError
        {
            get => _isVisibleNameError;
            set => SetProperty(ref _isVisibleNameError, value);
        }

        private bool _isVisibleEmailError;
        public bool IsVisibleEmailError
        {
            get => _isVisibleEmailError;
            set => SetProperty(ref _isVisibleEmailError, value);
        }

        private bool _isVisibleImageName;
        public bool IsVisibleImageName
        {
            get => _isVisibleImageName;
            set => SetProperty(ref _isVisibleImageName, value);
        }

        private bool _isVisibleImageEmail;
        public bool IsVisibleImageEmail
        {
            get => _isVisibleImageEmail;
            set => SetProperty(ref _isVisibleImageEmail, value);
        }

        private bool _isEnabledButton;
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryName))
            {
                CheckTextInput(_entryName);
                ShowHideImageName(_entryName);
            }

            if (args.PropertyName == nameof(EntryEmail))
            {
                CheckTextInput(_entryEmail);
                ShowHideImageEmail(_entryEmail);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            NavBarTitle = Resources.NavBarTitleRegister;
            LabelName = Resources.LabelName;
            EntryNamePlaceholder = Resources.LabelNamePlaceholder;
            LabelNameError = Resources.Input;
            LabelEmail = Resources.LabelEmail;
            EntryEmailPlaceholder = Resources.EntryPlaseholderEmail;
            LabelEmailError = Resources.Input;
            BorderColor = Color.Gray;
            IsVisibleImageName = false;
            IsVisibleImageEmail = false;
            ClearSourceName = "ic_clear";
            ClearSourceEmail = "ic_clear";
        }

        #endregion


        #region -- Private helpers --

        private async void OnImageLeftTapCommandAsync(object obj)
        {
            await navigationService.GoBackAsync();
        }

        private async void OnNextButtonTapCommandAsync()
        {
            IsVisibleNameError = false;
            IsVisibleEmailError = false;

            var isSuccess = CheckValidation();

            if (isSuccess)
            {
                var parameters = new NavigationParameters();
                parameters.Add(Constants.NameKey, _entryName);
                parameters.Add(Constants.LoginKey, _entryEmail);

                await navigationService.NavigateAsync($"{nameof(SignUpView)}", parameters);
            }
        }

        private void OnImageNameClearTapCommandAsync()
        {
            EntryName = string.Empty;
        }

        private void OnImageEmailClearTapCommandAsync()
        {
            EntryEmail = string.Empty;
        }

        #endregion

        #region -- Private methods --

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                IsEnabledButton = false;
            }
            else if (!string.IsNullOrEmpty(_entryEmail) &&
                    !string.IsNullOrEmpty(_entryName))
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

        private void ShowHideImageName(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                IsVisibleImageName = false;
            }
            else
            {
                IsVisibleImageName = true;
            }
        }


        private bool CheckValidation()
        {
            var isSuccess = true;

            if (!StringValidator.CheckName(_entryName))
            {
                IsVisibleNameError = true;
                isSuccess = false;
            }
            if (!StringValidator.CheckLogin(_entryEmail) && isSuccess)
            {
                IsVisibleEmailError = true;
                isSuccess = false;
            }

            return isSuccess;
        }

        #endregion
    }
}
