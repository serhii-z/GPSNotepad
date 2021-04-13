using GPSNotepad.Models;
using GPSNotepad.Services.Authentication;
using GPSNotepad.Validators;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using GPSNotepad.Properties;
using System.Threading.Tasks;

namespace GPSNotepad.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private IAuthenticationService _authenticationService;
        private IPageDialogService _pageDialog;

        public SignUpViewModel(INavigationService navigationService,
            IAuthenticationService authenticationService,
            IPageDialogService pageDialogService) : base(navigationService)
        {
            _authenticationService = authenticationService;
            _pageDialog = pageDialogService;
        }

        #region --- Public Properties ---
        public ICommand SignUpTapCommand => new Command(OnSignUpTapAsync);

        private string _entryNameText;
        public string EntryNameText
        {
            get => _entryNameText;
            set => SetProperty(ref _entryNameText, value);
        }

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

        private string _entryConfitmPasswordText;
        public string EntryConfirmPasswordText
        {
            get => _entryConfitmPasswordText;
            set => SetProperty(ref _entryConfitmPasswordText, value);
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

            if (args.PropertyName == nameof(EntryNameText))
            {
                CheckTextInput(_entryNameText);
            }

            if (args.PropertyName == nameof(EntryEmailText))
            {
                CheckTextInput(_entryEmailText);
            }

            if (args.PropertyName == nameof(EntryPasswordText))
            {
                CheckTextInput(_entryPasswordText);
            }

            if (args.PropertyName == nameof(EntryConfirmPasswordText))
            {
                CheckTextInput(_entryConfitmPasswordText);
            }
        }

        #endregion

        #region ---- Private Helpers ---

        private async void OnSignUpTapAsync()
        {
            var user = await AddUserAsync();

            if (user != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add(Constants.KeyLogin, user.Email);

                await navigationService.GoBackAsync(parameters);
            }
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
            if (!string.IsNullOrEmpty(_entryNameText) &&
                !string.IsNullOrEmpty(_entryEmailText) &&
                !string.IsNullOrEmpty(_entryPasswordText) &&
                !string.IsNullOrEmpty(_entryConfitmPasswordText))
                EnabledButton = true;
        }

        private void DeactivateButton()
        {
            EnabledButton = false;
        }

        private async Task<UserModel> AddUserAsync()
        {
            UserModel user = null;
            var isSuccess = CheckValidation();

            if (isSuccess)
            {
                user = CreateUser();
                isSuccess = await _authenticationService.SignUpAsync(user);

                if (!isSuccess)
                {
                    ShowAlert(Resources.SignUpLoginBusy);
                    ClearEntries();
                    user = null;
                }
            }

            return user;
        }

        private UserModel CreateUser()
        {
            var user = new UserModel()
            {
                Name = _entryNameText,
                Email = _entryEmailText,
                Password = _entryPasswordText
            };

            return user;
        }

        private void ClearEntries()
        {
            EntryNameText = string.Empty;
            EntryEmailText = string.Empty;
            EntryPasswordText = string.Empty;
            EntryConfirmPasswordText = string.Empty;
        }

        private async void ShowAlert(string message)
        {
            await _pageDialog.DisplayAlertAsync(Resources.AlertTitle, message, Resources.AlertOK);
        }

        private bool CheckValidation()
        {
            var isSuccess = true;

            if (!StringValidator.CheckName(_entryNameText))
            {
                ShowAlert(Resources.SignUpInvalidName);
                isSuccess = false;
            }
            if (!StringValidator.CheckLogin(_entryEmailText) && isSuccess)
            {
                ShowAlert(Resources.SignUpInvalidEmail);
                isSuccess = false;
            }
            if (!StringValidator.CheckQuantity(_entryPasswordText, 8) && isSuccess)
            {
                ShowAlert(Resources.SignUpInvalidNumber);
                isSuccess = false;
            }
            if (!StringValidator.CheckPresence(_entryPasswordText) && isSuccess)
            {
                ShowAlert(Resources.SignUpInvalidPresence);
                isSuccess = false;
            }
            if (!StringValidator.CheckPasswordEquality(_entryPasswordText, _entryConfitmPasswordText) && isSuccess)
            {
                ShowAlert(Resources.SignUpNotEqual);
                isSuccess = false;
            }

            ClearEntries();

            return isSuccess;
        }

        #endregion
    }
}
