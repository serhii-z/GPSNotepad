using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Controls
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            PinsSource = Pins as ObservableCollection<Pin>;
            MyLocationEnabled = true;

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

        private static readonly BindablePropertyKey PinsSourcePropertyKey = BindableProperty.CreateReadOnly(nameof(PinsSource),
            typeof(ObservableCollection<Pin>), typeof(CustomMap), default(ObservableCollection<Pin>));

        public static readonly BindableProperty PinsSourceProperty = PinsSourcePropertyKey.BindableProperty;

        public ObservableCollection<Pin> PinsSource
        {
            get => (ObservableCollection<Pin>)GetValue(PinsSourceProperty);
            private set => SetValue(PinsSourcePropertyKey, value);
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

        #endregion

        #region -- Private helpers --

        private void CustomMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            CommandMapTap?.Execute(e.Point);
        }

        private void CustomMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            CommandPinTap?.Execute(e.Pin);
        }

        private static void OnRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                var behavior = (CustomMap)bindable;
                behavior.MoveToRegion((MapSpan)newValue, behavior.Animated);
            }
        }

        #endregion
    }
}

