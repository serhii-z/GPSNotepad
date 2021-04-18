﻿using GPSNotepad.Drawing;
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

        #endregion
    }
}
