using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GPSNotepad.Models;
using GPSNotepad.Extensions;
using System.Runtime.CompilerServices;
using System;

namespace GPSNotepad.Controls
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            PinsSource = new ObservableCollection<PinViewModel>();
            UiSettings.ZoomControlsEnabled = false;

            MapClicked += CustomMap_MapClicked;
            PinClicked += CustomMap_PinClicked;
        }

        #region -- Pablic properties --

        public static readonly BindableProperty CommandMapTapProperty = BindableProperty.Create(nameof(CommandMapTap), typeof(Command),
            typeof(CustomMap), default(Command));

        public ICommand CommandMapTap
        {
            get => (ICommand)GetValue(CommandMapTapProperty);
            set => SetValue(CommandMapTapProperty, value);
        }

        public static readonly BindableProperty CommandPinTapProperty = BindableProperty.Create(nameof(CommandPinTap), typeof(Command),
            typeof(CustomMap), default(Command));

        public ICommand CommandPinTap
        {
            get => (ICommand)GetValue(CommandPinTapProperty);
            set => SetValue(CommandPinTapProperty, value);
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(nameof(PinsSource), typeof(ObservableCollection<PinViewModel>),
            typeof(CustomMap));

        public ObservableCollection<PinViewModel> PinsSource
        {
            get => (ObservableCollection<PinViewModel>)GetValue(PinsSourceProperty);
            set => SetValue(PinsSourceProperty, value);
        }

        public static readonly BindableProperty RegionProperty = BindableProperty.Create(nameof(Region), typeof(MapSpan),
            typeof(CustomMap), default(MapSpan), propertyChanged: OnRegionChanged);

        public new MapSpan Region
        {
            get => (MapSpan)GetValue(RegionProperty);
            set => SetValue(RegionProperty, value);
        }

        public static readonly BindableProperty AnimatedProperty = BindableProperty.Create(nameof(Animated), typeof(bool), typeof(CustomMap), true);

        public bool Animated
        {
            get => (bool)GetValue(AnimatedProperty);
            set => SetValue(AnimatedProperty, value);
        }

        public static readonly BindableProperty StyleMapProperty = BindableProperty.Create(nameof(StyleMap), typeof(MapStyle),
            typeof(CustomMap), propertyChanged: OnStyleMapPropertyChanged);

        public MapStyle StyleMap
        {
            get => (MapStyle)GetValue(StyleMapProperty);
            set => SetValue(StyleMapProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private static void OnRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                var behavior = (CustomMap)bindable;
                behavior.MoveToRegion((MapSpan)newValue, behavior.Animated);
            }
        }

        private void CustomMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            CommandMapTap?.Execute(e.Point);
        }

        private void CustomMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            CommandPinTap?.Execute(e.Pin);
        }

        private static void OnStyleMapPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (CustomMap)bindable;
            var style = newValue as MapStyle;
            behavior.MapStyle = style;
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(PinsSource))
            {
                Pins.Clear();

                if (PinsSource != null)
                {
                    foreach (var item in PinsSource)
                    {
                        this.Pins.Add(item.ToPin());
                    }
                }
            }
        }

        #endregion
    }
}

