using Prism.Mvvm;

namespace GPSNotepad.Models
{
    public class PinViewModel : BindableBase
    {
        private int _pinId;
        public int PinId
        {
            get => _pinId;
            set => SetProperty(ref _pinId, value);
        }

        private string _name;
        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private double _latitude;
        public double Latitude 
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude 
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _description;
        public string Description 
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _pinId, value);
        }
    }
}
