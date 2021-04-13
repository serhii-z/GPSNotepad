using GPSNotepad.Models;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Extensions
{
    public static class ExtensionsPins
    {
        //ToPinViewModel from PinModel
        public static PinViewModel ToViewModel(this PinModel pinModel)
        {
            PinViewModel pinViewModel = new PinViewModel()
            {
                PinId = pinModel.Id,
                Name = pinModel.Name,
                Latitude = pinModel.Latitude,
                Longitude = pinModel.Longitude,
                Description = pinModel.Description
            };

            return pinViewModel;
        }

        //ToPinModel from PinViewModel
        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            PinModel pinModel = new PinModel()
            {
                Name = pinViewModel.Name,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Description = pinViewModel.Description
            };

            return pinModel;
        }


        //ToPinViewModel from Pin
        public static PinViewModel ToPinViewModel(this Pin pin)
        {
            PinViewModel pinViewModel = new PinViewModel()
            {
                PinId = pin.ZIndex,
                Name = pin.Label,
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Description = pin.Address
            };

            return pinViewModel;
        }

        //ToPin from PinViewModel
        public static Pin ToPin(this PinViewModel pinViewModel)
        {
            var pin = new Pin
            {
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                Label = pinViewModel.Name,
                ZIndex = pinViewModel.PinId,
                Address = pinViewModel.Description
            };

            return pin;
        }
    }
}
