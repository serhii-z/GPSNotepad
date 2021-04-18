using GPSNotepad.Controls;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace GPSNotepad.Drawing
{
    public class ClockCanvas
    {
        private SKCanvasView _canvasView;
        private double _timeCorrection;

        private SKPath _hourHandPath = SKPath.ParseSvgPathData(
            "M 0 -60 C   0 -45 0 -40  5 -20 L  5   0" +
                    "C   5 7.5 -5 7.5 -5    0 L -5 -20" +
                    "C 0 -40  0 -45  0 -60 Z");

        private SKPath _minuteHandPath = SKPath.ParseSvgPathData(
            "M 0 -80 C   0 -75  0 -70  2.5 -60 L  2.5   0" +
                    "C   2.5 5 -2.5 5 -2.5   0 L -2.5 -60" +
                    "C 0 -70  0 -75  0 -80 Z");
   
        private SKPath _secondHandPath = SKPath.ParseSvgPathData(
            "M 0 10 L 0 -80");

        private SKPaint _handStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Brown,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round
        };

        private SKPaint _lightBlueStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.LightBlue,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round
        };

        private SKPaint _handFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Violet
        };

        private SKPaint _minuteMarkPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Orange,
            StrokeWidth = 3,
            StrokeCap = SKStrokeCap.Round,
            PathEffect = SKPathEffect.CreateDash(new float[] { 0, 3 * 3.14159f }, 0)
        };

        private SKPaint _hourMarkPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Orange,
            StrokeWidth = 6,
            StrokeCap = SKStrokeCap.Round,
            PathEffect = SKPathEffect.CreateDash(new float[] { 0, 15 * 3.14159f }, 0)
        };

        #region -- Public methods --

        public void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            SKImageInfo inf = info;
            SKSurface surf = surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(Math.Min(info.Width / (info.Width / 3), info.Height / (info.Height / 3)));

            SKRect rect = new SKRect(-90, -90, 90, 90);
            canvas.DrawOval(rect, _minuteMarkPaint);
            canvas.DrawOval(rect, _hourMarkPaint);

            DateTime dateTime = DateTime.Now.AddHours(_timeCorrection);

            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            canvas.DrawPath(_hourHandPath, _handStrokePaint);
            canvas.DrawPath(_hourHandPath, _handFillPaint);
            canvas.Restore();

            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawPath(_minuteHandPath, _handStrokePaint);
            canvas.DrawPath(_minuteHandPath, _handFillPaint);
            canvas.Restore();

            double t = dateTime.Millisecond / 1000.0;

            if (t < 0.5)
            {
                t = 0.5 * Easing.SpringIn.Ease(t / 0.5);
            }
            else
            {
                t = 0.5 * (1 + Easing.SpringOut.Ease((t - 0.5) / 0.5));
            }

            canvas.Save();
            canvas.RotateDegrees(6 * (dateTime.Second + (float)t));
            canvas.DrawPath(_secondHandPath, _lightBlueStrokePaint);
            canvas.Restore();
        }

        public void StartClock(CustomSKCanvasView view, double timeCorrection)
        {
            _canvasView =  view;
            _timeCorrection = timeCorrection;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                _canvasView.InvalidateSurface();
                return true;
            });
        }

        #endregion
    }
}
