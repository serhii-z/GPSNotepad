using GPSNotepad.Models;
using GPSNotepad.Services.Weather;
using Prism.Navigation;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class PinInfoViewModel : BaseViewModel
    {
        private IWeatherService _weatherService;
        public PinInfoViewModel(INavigationService navigationService, IWeatherService weatherService) : base(navigationService)
        {
            _weatherService = weatherService;
        }

        #region -- Public Properties --

        public ICommand GoBackTapCommand => new Command(OnGoBackTap);

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _temperature;
        public string Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get => _windSpeed;
            set => SetProperty(ref _windSpeed, value);
        }

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(Constants.PinViewModelKey, out PinViewModel value))
            {
                ShowPinInfoAsync(value);
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnGoBackTap()
        {
            await navigationService.GoBackAsync(useModalNavigation: true);
        }

        #endregion

        #region -- Private methods --

        private async void ShowPinInfoAsync(PinViewModel pinViewModel)
        {
            Name = pinViewModel.Name;
            Description = pinViewModel.Description;
            Latitude = pinViewModel.Latitude.ToString();
            Longitude = pinViewModel.Longitude.ToString();

            var weatherResponse = await _weatherService.GetWeatherResponseAsync(_latitude, _longitude, Constants.OpenWeatherUnits);
            Temperature = string.Format("{0}: {1}°C", "temperature",  weatherResponse.Main.Temp.ToString());
            WindSpeed = string.Format("{0}: {1}km/h", "vind speed",  weatherResponse.Wind.Speed.ToString());
            var icon = weatherResponse.Weather.Select(x => x).First().Icon;
            IconPath = string.Format(Constants.OpenWeatherIconPath, icon); 
            //IconPath = $"https://openweathermap.org/img/wn/{icon}@4x.png";
        }

        #endregion
    }
}
