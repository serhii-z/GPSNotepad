using GPSNotepad.Models;
using GPSNotepad.Services.SettingsService;
using GPSNotepad.Services.Time;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class ClockViewModel : BaseViewModel, IDialogAware
    {
        private ITimeService _timeService;
        private ISettingsManager _settingsManager;
        private IDialogService _dialogService;

        public ClockViewModel(INavigationService navigationService, ITimeService timeService,
            ISettingsManager settingsManager, IDialogService dialogService) : base(navigationService)
        {
            _timeService = timeService;
            _settingsManager = settingsManager;
            _dialogService = dialogService;
        }

        #region -- Public properties --

        //public ICommand GoBackTapCommand => new Command(OnGoBackTapAsync);

        private double _timeCorrection;
        public double TimeCorrection
        {
            get => _timeCorrection;
            set => SetProperty(ref _timeCorrection, value);
        }

        private string _gmtInfo;
        public string GMTInfo
        {
            get => _gmtInfo;
            set => SetProperty(ref _gmtInfo, value);
        }

        private string _time;
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _colorClock;

        public event Action<IDialogParameters> RequestClose;

        public string ColorClock
        {
            get => _colorClock;
            set => SetProperty(ref _colorClock, value);
        }

        #endregion

        #region -- IDialogAware implement --

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                ShowTime(value);
            }

            ColorClock = _settingsManager.ColorClock;
        }

        #endregion

        #region -- Private helpers --

        //private void OnGoBackTapAsync()
        //{
        //    navigationService.GoBackAsync();
        //}

        #endregion

        #region -- Private methods --

        private void ShowTime(PinViewModel pinViewmodel)
        {
            var hours = _timeService.GetTimeCorrection(pinViewmodel);
            var gmt = hours + 3;

            Name = pinViewmodel.Name;
            GMTInfo = gmt >= 0  ? string.Format("GMT+{0}:00",  gmt) : string.Format("GMT {0}:00", gmt);

            if (hours == 0)
            {
                TimeCorrection = 0.1;
            }
            else
            {
                TimeCorrection = hours;
            }

            StartTimer(hours);
        }

        private void StartTimer(int hours)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1.0), () =>
            {
                Time = DateTime.Now.AddHours(hours).AddMilliseconds(800).ToString("HH.mm.ss");
                return true;
            });
        }



        #endregion
    }
}
