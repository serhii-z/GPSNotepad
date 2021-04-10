using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.ViewModels
{
    public class DropViewModel : BaseViewModel
    {
        public DropViewModel(INavigationService navigationService) : base(navigationService)
        {
            Pins = new ObservableCollection<Pin>();
        }

        #region --- Public Properties ---

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            MoveView();
        }


        public override void Initialize(INavigationParameters parameters)
        {
            //if (parameters.TryGetValue("pinViewModel", out PinViewModel value))
            //{
            //    Name = value.Name;
            //    Description = value.Description;
            //}
        }

        #endregion

        #region --- Private Methods ---

        private void MoveView()
        {
            
        }

        #endregion


    }
}
