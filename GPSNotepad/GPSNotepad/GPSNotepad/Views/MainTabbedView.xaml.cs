using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace GPSNotepad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedView : Xamarin.Forms.TabbedPage
    {
        public MainTabbedView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}