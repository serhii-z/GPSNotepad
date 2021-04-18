using GPSNotepad.Models;
using GPSNotepad.Services.Time;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class ClockWiewModel : BaseViewModel
    {
        private ITimeService _timeService;

        public ClockWiewModel(INavigationService navigationService, ITimeService timeService) : base(navigationService)
        {
            _timeService = timeService;
        }

        #region -- Public properties --

        public ICommand GoBackTapCommand => new Command(OnGoBackTapAsync);

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

        #endregion

        #region -- Override --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                ShowTime(value);
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnGoBackTapAsync()
        {
            await navigationService.GoBackAsync(useModalNavigation: true);
        }

        #endregion

        #region -- Private methods --

        private void ShowTime(PinViewModel pinViewmodel)
        {
            var hours = _timeService.GetTimeCorrection(pinViewmodel);
            var gmt = hours + 3;

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
