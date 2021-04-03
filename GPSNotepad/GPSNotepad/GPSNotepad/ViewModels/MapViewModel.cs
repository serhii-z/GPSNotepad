using GPSNotepad.Controls;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GPSNotepad.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel(INavigationService navigationService, CustomMap customMap) : base(navigationService)
        {
            Map = customMap;
        }

        #region --- Public Properties ---

        private Map _map;
        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        #endregion
    }
}
