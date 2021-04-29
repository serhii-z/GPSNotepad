using GPSNotepad.Drawing;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GPSNotepad.Controls
{
    public class CustomSKCanvasView : SKCanvasView
    {
        private ClockCanvas _clockCanvas;
        public CustomSKCanvasView() 
        {
            _clockCanvas = new ClockCanvas();
        }

        #region -- Public properties --

        public static readonly BindableProperty TimeCorrectionProperty = BindableProperty.Create(nameof(TimeCorrection), typeof(double),
            typeof(CustomSKCanvasView), default(double), propertyChanged: OnStartClockChanged);

        public double TimeCorrection
        {
            get => (double)GetValue(TimeCorrectionProperty);
            set => SetValue(TimeCorrectionProperty, value);
        }

        public static readonly BindableProperty GMTInfoProperty = BindableProperty.Create(nameof(GMTInfo), typeof(string),
            typeof(CustomSKCanvasView), default(string));

        public string GMTInfo
        {
            get => (string)GetValue(GMTInfoProperty);
            set => SetValue(GMTInfoProperty, value);
        }

        public static readonly BindableProperty ColorClockProperty = BindableProperty.Create(nameof(ColorClock), typeof(string),
            typeof(CustomMap), propertyChanged: OnColorClock);

        public string ColorClock
        {
            get => (string)GetValue(ColorClockProperty);
            set => SetValue(ColorClockProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            _clockCanvas.PaintSurface(e.Surface, e.Info);
        }

        #endregion

        #region -- Private helpers --

        private static void OnStartClockChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = bindable as CustomSKCanvasView;

            behavior._clockCanvas.StartClock(behavior, behavior.TimeCorrection);
        }

        private static void OnColorClock(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = bindable as CustomSKCanvasView;
            var color = newValue as string;

            behavior._clockCanvas.ApplyColor(color);
        }

        #endregion
    }
}
