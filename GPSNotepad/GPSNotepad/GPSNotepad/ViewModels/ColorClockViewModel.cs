using GPSNotepad.Services.SettingsService;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class ColorClockViewModel : BaseViewModel
    {
        private ISettingsManager _settingsManager;
        public ColorClockViewModel(INavigationService navigationService, ISettingsManager settingsManager) : base(navigationService)
        {
            _settingsManager = settingsManager;
        }

        #region -- Public properties --

        public ICommand GoBackTapCommand => new Command(OnGoBackTapCommandAsync);
        public ICommand BlueTapCommand => new Command(OnBlueTapCommand);
        public ICommand RedTapCommand => new Command(OnRedTapCommand);

        private bool _isChackedBlue;
        public bool IsChackedBlue
        {
            get => _isChackedBlue;
            set => SetProperty(ref _isChackedBlue, value);
        }

        private bool _isChackedRed;
        public bool IsChackedRed
        {
            get => _isChackedRed;
            set => SetProperty(ref _isChackedRed, value);
        }

        #endregion

        #region -- Private helpers --

        private async void OnGoBackTapCommandAsync()
        {
            await navigationService.GoBackAsync();
        }

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            ActivateControl();
        }

        #endregion

        #region -- Private helpers --

        private void OnBlueTapCommand()
        {
            _settingsManager.IsRadioButtonBlue = true;
            _settingsManager.IsRadioButtonRed = false;
            _settingsManager.ColorClock = Constants.ColorClockDefault;
        }

        private void OnRedTapCommand()
        {
            _settingsManager.IsRadioButtonRed = true;
            _settingsManager.IsRadioButtonBlue = false;
            _settingsManager.ColorClock = Constants.ColorClockRed;
        }

        #endregion

        #region -- Private methods --

        private void ActivateControl()
        {
            IsChackedBlue = _settingsManager.IsRadioButtonBlue;
            IsChackedRed = _settingsManager.IsRadioButtonRed;
        }

        #endregion
    }
}
