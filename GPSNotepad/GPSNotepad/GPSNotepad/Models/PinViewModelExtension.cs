namespace GPSNotepad.Models
{
    public static class PinViewModelExtension
    {
        public static void InitInstance(this PinViewModel pinViewModel, PinModel pinModel)
        {
            pinViewModel.PinId = pinModel.Id;
            pinViewModel.Name = pinModel.Name;
            pinViewModel.Latitude = pinModel.Latitude;
            pinViewModel.Longitude = pinModel.Longitude;
            pinViewModel.Description = pinModel.Description;
            pinViewModel.UserId = pinModel.UserId;
        }
    }
}
