using GPSNotepad.Models;
using GPSNotepad.Services.Authentication;
using GPSNotepad.Validators;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

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
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

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

        #region ---- Private Helpers ---

        private async void OnSignUpTap()
        {
            var user = AddUser();

            if (user != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("login", user.Email);

                await navigationService.GoBackAsync(parameters);
            }
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

        private UserModel AddUser()
        {
            UserModel user = null;

            if (IsPassValidation())
            {
                if (IsLogin())
                {
                    ShowAlert("This login is already token!");
                    ClearEntries();
                }
                else
                {
                    user = CreateUser();
                    _authenticationService.AddUser(user);
                }
            }

            return user;
        }

        private bool IsLogin()
        {
            var isBusy = _authenticationService.IsLogin(_entryEmailText);

            return isBusy;
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
            await _pageDialog.DisplayAlertAsync("Message", message, "OK");
        }

        private bool IsPassValidation()
        {
            if (!StringValidator.IsValidName(_entryNameText))
            {
                ShowAlert("Invalid name!");
                ClearEntries();
                return false;
            }
            if (!StringValidator.IsValidEmail(_entryEmailText))
            {
                ShowAlert("Invalid email!");
                ClearEntries();
                return false;
            }
            if (!StringValidator.IsQuantityCorrect(_entryPasswordText, 8))
            {
                ShowAlert("Invalid number of characters!");
                ClearEntries();
                return false;
            }
            if (!StringValidator.IsAvailability(_entryPasswordText))
            {
                ShowAlert("Invalid availability!");
                ClearEntries();
                return false;
            }
            if (!StringValidator.IsPasswordsEqual(_entryPasswordText, _entryConfitmPasswordText))
            {
                ShowAlert("Password and confirm password are not equal!");
                ClearEntries();
                return false;
            }

            return true;
        }

        #endregion
    }
}
