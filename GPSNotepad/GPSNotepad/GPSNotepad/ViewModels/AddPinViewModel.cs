using GPSNotepad.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPSNotepad.ViewModels
{
    public class AddPinViewModel : BaseViewModel
    {
        private PinModel _pin;

        public AddPinViewModel(INavigationService navigationService) : base(navigationService)
        {
            Map = new Map();
        }

        #region --- Public Properties ---

        public ICommand SaveTapCommand => new Command(OnSaveTap);


        private Map _map;
        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

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

        #region --- Private Methods ---

        private void CreatePin()
        {
            _pin = new PinModel
            {
                Name = _entryNameText,
                Latitude =  Convert.ToDouble(_entryLatitudeText),
                Longitude = Convert.ToDouble(_entryLongitudeText),
                Description = _editorText,
                //UserId = _authorizationService.GetAuthorizedUserId()
            };
        }

        private void AddPin()
        {
            if (_pin == null)
            {
                CreatePin();
                //_pinService.AddPin(_pin);
            }
        }

        #endregion

        #region --- Private Helpers ---

        private void OnMapTap(object sender, MapClickedEventArgs e)
        {
            var position = e.Position;

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


        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Map))
            {
                _map.MapClicked += new EventHandler<MapClickedEventArgs>(OnMapTap);
            }
        }

        #endregion
    }
}
