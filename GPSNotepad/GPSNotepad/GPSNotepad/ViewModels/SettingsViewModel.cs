using GPSNotepad.Resources.Themes;
using GPSNotepad.Services.Resources;
using GPSNotepad.Services.SettingsService;
using GPSNotepad.Views;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        ISettingsManager _settingsManager;
        IResourceService _resourceService;
        IDialogService _dialogService;
        public SettingsViewModel(INavigationService navigationService, ISettingsManager settingsManager, 
            IResourceService resourceService, IDialogService dialogService) : base(navigationService)
        {
            _settingsManager = settingsManager;
            _resourceService = resourceService;
            _dialogService = dialogService;
        }

        #region -- Public properties --

        public ICommand GoBackTapCommand => new Command(OnGoBackTapCommandAsync);
        public ICommand ColorClockTapCommand => new Command(OnColorClockTapCommandAsync);

        private bool _isToggled;
        public bool IsToggled
        {
            get => _isToggled;
            set => SetProperty(ref _isToggled, value);
        }

        private bool _isRussian;
        public bool IsRussian
        {
            get => _isRussian;
            set => SetProperty(ref _isRussian, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(IsToggled))
            {
                if (IsToggled)
                {
                    _settingsManager.IsDarkTheme = IsToggled;
                    _settingsManager.ThemeName = nameof(DarkTheme);
                }
                else
                {
                    _settingsManager.IsDarkTheme = IsToggled;
                    _settingsManager.ThemeName = string.Empty;
                }

                _resourceService.ApplyTheme();
            }
            if (args.PropertyName == nameof(IsRussian))
            {
                if (IsRussian)
                {
                    _settingsManager.IsRussianCulture = IsRussian;
                    _settingsManager.CultureName = "ru";
                }
                else
                {
                    _settingsManager.IsRussianCulture = IsToggled;
                    _settingsManager.CultureName = string.Empty;
                }

                _resourceService.ApplyCulture();
            }

        }

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ActivateControlsTheme();
            ActivateControlsCulture();
        }

        #endregion

        #region -- Private helpers --

        private async void OnGoBackTapCommandAsync()
        {
            await navigationService.GoBackAsync();
        }

        private async void OnColorClockTapCommandAsync(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(ColorClockView)}");
        }

        #endregion

        #region -- Private methods --

        private void ActivateControlsTheme()
        {
            IsToggled = _settingsManager.IsDarkTheme;
        }

        private void ActivateControlsCulture()
        {
            IsRussian = _settingsManager.IsRussianCulture;
        }

        #endregion
    }
}
