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
        private SKPath _hourHandPath = SKPath.ParseSvgPathData("M 0 -3.5 L 0 -40");
        private SKPath _minuteHandPath = SKPath.ParseSvgPathData("M 0 -3.5 L 0 -60");  
        private SKPath _secondHandPath = SKPath.ParseSvgPathData("M 0 -3 L 0 -80");
        private SKPath _twelvePath = SKPath.ParseSvgPathData("M 0 -90 L 0 -80");
        private SKPath _threePath = SKPath.ParseSvgPathData("M 0 90 L 0 80");
        private SKPath _sixPath = SKPath.ParseSvgPathData("M 90 0 L 80 0");
        private SKPath _ninePath = SKPath.ParseSvgPathData("M -90 0 L -80 0");

        private SKPaint _handStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
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

        private SKPaint _circleFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.AliceBlue
        };

        private SKPaint _textPaint = new SKPaint
        {
            Color = SKColors.Black
        };

        #region -- Public methods --

        public void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(Math.Min(info.Width / (info.Width / 3), info.Height / (info.Height / 3)));

            //SKRect rect = new SKRect(-90, -90, 90, 90);
            canvas.DrawCircle(0, 0, 90, _circleFillPaint);
            canvas.DrawCircle(0, 0, 90, _lightBlueStrokePaint);

            canvas.DrawPath(_twelvePath, _lightBlueStrokePaint);
            canvas.DrawPath(_threePath, _lightBlueStrokePaint);
            canvas.DrawPath(_sixPath, _lightBlueStrokePaint);
            canvas.DrawPath(_ninePath, _lightBlueStrokePaint);

            canvas.DrawText("12", -7f, -68f, _textPaint);
            canvas.DrawText("3", 71f, 4f, _textPaint);
            canvas.DrawText("6", -4f, 76f, _textPaint);
            canvas.DrawText("9", -78f, 4f, _textPaint);

            canvas.DrawCircle(0, 0, 2, _lightBlueStrokePaint);

            DateTime dateTime = DateTime.Now.AddHours(_timeCorrection);

            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            canvas.DrawPath(_hourHandPath, _handStrokePaint);
            canvas.Restore();

            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawPath(_minuteHandPath, _handStrokePaint);
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
