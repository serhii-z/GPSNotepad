using Xamarin.Forms;

namespace GPSNotepad.Views
{
    public class BaseTabbedPage : Xamarin.Forms.TabbedPage
    {
        public static readonly BindableProperty TapColorProperty = BindableProperty.Create(nameof(TapColor), typeof(Color),
            typeof(BaseTabbedPage));

        public Color TapColor
        {
            get => (Color)GetValue(TapColorProperty);
            set => SetValue(TapColorProperty, value);
        }
    }
}
