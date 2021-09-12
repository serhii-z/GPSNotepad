﻿using GPSNotepad.Models;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Extensions
{
    public static class ExtensionsPins
    {
        public static PinViewModel ToViewModel(this PinModel pinModel)
        {
            PinViewModel pinViewModel = new PinViewModel()
            {
                PinId = pinModel.Id,
                Name = pinModel.Name,
                Latitude = pinModel.Latitude,
                Longitude = pinModel.Longitude,
                Description = pinModel.Description,
                ImagePath = pinModel.ImagePath,
                IsFavorit = pinModel.IsFavorit,
                UserId = pinModel.UserId,
            };

            return pinViewModel;
        }

        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            PinModel pinModel = new PinModel()
            {
                Id = pinViewModel.PinId,
                Name = pinViewModel.Name,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Description = pinViewModel.Description,
                ImagePath = pinViewModel.ImagePath,
                IsFavorit = pinViewModel.IsFavorit,
                UserId = pinViewModel.UserId,
            };

            return pinModel;
        }

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
        public static Pin ToPin(this PinViewModel pinViewModel)
        {
            var pin = new Pin
            {
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                Label = pinViewModel.Name,
                ZIndex = pinViewModel.PinId,
                Address = pinViewModel.Description,
                Icon =  BitmapDescriptorFactory.FromBundle(Constants.ImagePlaceholder)
            };

            return pin;
        }
    }
}
