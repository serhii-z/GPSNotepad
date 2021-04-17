using System;
using Xamarin.Forms;

namespace GPSNotepad.Controls
{
    public class CustomAbsoluteLayout : AbsoluteLayout
    {
        private BoxView[] _tickMarks = new BoxView[60];
        private static readonly HandParams secondParams = new HandParams(0.02, 1.1, 0.85);
        private static readonly HandParams minuteParams = new HandParams(0.05, 0.8, 0.9);
        private static readonly HandParams hourParams = new HandParams(0.125, 0.6, 0.9);

        public CustomAbsoluteLayout()
        {
            SecondHand = new BoxView();
            SecondHand.Color = Color.LightBlue;
            Children.Add(SecondHand);

            MinuteHand = new BoxView();
            MinuteHand.Color = Color.Violet;
            Children.Add(MinuteHand);

            HourHand = new BoxView();
            HourHand.Color = Color.Violet;
            Children.Add(HourHand);
        }

        #region -- Public properties

        public BoxView SecondHand { get; set; }
        public BoxView MinuteHand { get; set; }
        public BoxView HourHand { get; set; }

        public static readonly BindableProperty TimeCorrectionProperty = BindableProperty.Create(nameof(TimeCorrection), typeof(double),
            typeof(CustomAbsoluteLayout), default(double), propertyChanged: OnStartClockChanged);

        public double TimeCorrection
        {
            get => (double)GetValue(TimeCorrectionProperty);
            set => SetValue(TimeCorrectionProperty, value);
        }

        #endregion


        #region -- Private helpers --

        private static void OnStartClockChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (CustomAbsoluteLayout)bindable;

            for (int i = 0; i < behavior._tickMarks.Length; i++)
            {
                behavior._tickMarks[i] = new BoxView { Color = Color.Orange };
                behavior.Children.Add(behavior._tickMarks[i]);
            }

            Device.StartTimer(TimeSpan.FromSeconds(1.0 / 60), behavior.OnTimerTick);

            behavior.SizeChanged += behavior.CustomAbsoluteLayout_SizeChanged;
        }

        private void CustomAbsoluteLayout_SizeChanged(object sender, EventArgs e)
        {
            // Get the center and radius of the AbsoluteLayout.
            Point center = new Point(Width / 2, Height / 2);
            double radius = 0.45 * Math.Min(Width, Height);

            // Position, size, and rotate the 60 tick marks.
            for (int index = 0; index < _tickMarks.Length; index++)
            {
                double size = radius / (index % 5 == 0 ? 15 : 80);
                double size2 = radius / (index % 5 == 0 ? 15 : 30);
                double radians = index * 2 * Math.PI / _tickMarks.Length;
                double x = center.X + radius * Math.Sin(radians) - size / 2;
                double y = center.Y - radius * Math.Cos(radians) - size / 2;
                SetLayoutBounds(_tickMarks[index], new Rectangle(x, y, size, size2));
                _tickMarks[index].Rotation = 180 * radians / Math.PI;
            }

            // Position and size the three hands.
            LayoutHand(SecondHand, secondParams, center, radius);
            LayoutHand(MinuteHand, minuteParams, center, radius);
            LayoutHand(HourHand, hourParams, center, radius);
        }

        private bool OnTimerTick()
        {
            // Set rotation angles for hour and minute hands.
            var dateTime = DateTime.Now.AddHours(TimeCorrection);
            HourHand.Rotation = 30 * (dateTime.Hour % 12) + 0.5 * dateTime.Minute;
            MinuteHand.Rotation = 6 * dateTime.Minute + 0.1 * dateTime.Second;

            // Do an animation for the second hand.
            double t = dateTime.Millisecond / 1000.0;

            if (t < 0.5)
            {
                t = 0.5 * Easing.SpringIn.Ease(t / 0.5);
            }
            else
            {
                t = 0.5 * (1 + Easing.SpringOut.Ease((t - 0.5) / 0.5));
            }

            SecondHand.Rotation = 6 * (dateTime.Second + t);
            return true;
        }

        #endregion

        #region -- Private methods --

        private void LayoutHand(BoxView boxView, HandParams handParams, Point center, double radius)
        {
            double width = handParams.Width * radius;
            double height = handParams.Height * radius;
            double offset = handParams.Offset;

            SetLayoutBounds(boxView,
                new Rectangle(center.X - 0.5 * width,
                              center.Y - offset * height,
                              width, height));

            //Set the AnchorY property for rotations.
           boxView.AnchorY = handParams.Offset;
        }

        #endregion

        #region -- Inner classes --

        private struct HandParams
        {
            public HandParams(double width, double height, double offset) : this()
            {
                Width = width;
                Height = height;
                Offset = offset;
            }

            public double Width { private set; get; }   // fraction of radius
            public double Height { private set; get; }  // ditto
            public double Offset { private set; get; }  // relative to center pivot
        }

        #endregion
    }
}
