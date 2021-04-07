using GPSNotepad.Models;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.ViewModels
{
    public class AddPinViewModel : BaseViewModel
    {
        private PinModel _pin;
        private IAuthorizationService _authorizationService;
        private IPinService _pinService;

        public AddPinViewModel(INavigationService navigationService, 
            IAuthorizationService authorizationService, 
            IPinService pinService) : base(navigationService)
        {
            _authorizationService = authorizationService;
            _pinService = pinService;
        }

        #region --- Public Properties ---

        public ICommand SaveTappedCommand => new Command(OnSaveTap);

        public ICommand MapTappedCommand => new Command<Position>(OnMapTap);

        private string _entryNameText;
        public string EntryNameText
        {
            get => _entryNameText;
            set => SetProperty(ref _entryNameText, value);
        }

        private string _entryLatitudeText;
        public string EntryLatitudeText
        {
            get => _entryLatitudeText;
            set => SetProperty(ref _entryLatitudeText, value);
        }

        private string _entryLongitudeText;
        public string EntryLongitudeText
        {
            get => _entryLongitudeText;
            set => SetProperty(ref _entryLongitudeText, value);
        }

        private string _editorText;
        public string EditorText
        {
            get => _editorText;
            set => SetProperty(ref _editorText, value);
        }

        #endregion

        #region --- Private Helpers ---

        private void OnMapTap(Position obj)
        {
            var position = obj;

            EntryLatitudeText = position.Latitude.ToString();
            EntryLongitudeText = position.Longitude.ToString();
        }

        private async void OnSaveTap(object obj)
        {
            if (!string.IsNullOrEmpty(_entryNameText) && 
                !string.IsNullOrEmpty(_entryLatitudeText) && 
                !string.IsNullOrEmpty(_entryLongitudeText) && 
                !string.IsNullOrEmpty(_editorText))
            {
                AddPin();

                await navigationService.GoBackAsync();
            }
        }

        #endregion

        #region --- Private Methods ---

        private void CreatePin()
        {
            _pin = new PinModel
            {
                Name = _entryNameText,
                Latitude = Convert.ToDouble(_entryLatitudeText),
                Longitude = Convert.ToDouble(_entryLongitudeText),
                Description = _editorText,
                UserId = _authorizationService.UserId
            };
        }

        private void AddPin()
        {
            if (_pin == null)
            {
                CreatePin();
                _pinService.AddPin(_pin);
            }
        }

        #endregion
    }
}
