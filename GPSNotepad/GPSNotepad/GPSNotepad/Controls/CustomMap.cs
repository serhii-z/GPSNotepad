using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotepad.Controls
{
    public class CustomMap : Map
    {
        private static readonly BindablePropertyKey PinsSourcePropertyKey = BindableProperty.CreateReadOnly(nameof(PinsSource),
            typeof(ObservableCollection<Pin>), typeof(CustomMap), default(ObservableCollection<Pin>));

        public static readonly BindableProperty PinsSourceProperty = PinsSourcePropertyKey.BindableProperty;

        public static readonly BindableProperty CommandMapTapProperty = BindableProperty.Create(nameof(CommandMapTap), typeof(Command),
            typeof(CustomMap), default(Command));

        public static readonly BindableProperty CommandPinTapProperty = BindableProperty.Create(nameof(CommandPinTap), typeof(Command),
            typeof(CustomMap), default(Command));

        public static readonly BindableProperty RegionProperty = BindableProperty.Create(nameof(Region), typeof(MapSpan), 
            typeof(CustomMap), default(MapSpan), propertyChanged: OnRegionChanged);

        public static readonly BindableProperty AnimatedProperty = BindableProperty.Create(nameof(Animated), typeof(bool), typeof(CustomMap), true);

        public bool Animated => (bool)GetValue(AnimatedProperty);

        public CustomMap()
        {
            PinsSource = Pins as ObservableCollection<Pin>;

            MapClicked += CustomMap_MapClicked;
            PinClicked += CustomMap_PinClicked;    
        }

        #region --- Pablic Properties ---

        public ObservableCollection<Pin> PinsSource
        {
            get => (ObservableCollection<Pin>)GetValue(PinsSourceProperty);
            private set => SetValue(PinsSourcePropertyKey, value);
        }

        public new MapSpan Region
        {
            get => (MapSpan)GetValue(RegionProperty);
            set
            {
                SetValue(RegionProperty, value);
            }
        }

        public ICommand CommandMapTap
        {
            get => (ICommand)GetValue(CommandMapTapProperty);
            set => SetValue(CommandMapTapProperty, value);
        }

        public ICommand CommandPinTap
        {
            get => (ICommand)GetValue(CommandPinTapProperty);
            set => SetValue(CommandPinTapProperty, value);
        }

        #endregion

        #region --- Overrides ---

        private void CustomMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            CommandMapTap?.Execute(e.Point);
        }

        private void CustomMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            CommandPinTap?.Execute(e.Pin);
        }

        #endregion

        private static void OnRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;

            var behavior = (CustomMap)bindable;
            behavior.MoveToRegion((MapSpan)newValue, behavior.Animated);
        }
    }
}

