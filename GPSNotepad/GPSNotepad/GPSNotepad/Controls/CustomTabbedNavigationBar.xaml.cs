using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomTabbedNavigationBar
    {
        public CustomTabbedNavigationBar()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ImageSettingsTapCommandProperty = BindableProperty.Create(nameof(ImageSettingsTapCommand),
            typeof(Command), typeof(CustomTabbedNavigationBar));

        public ICommand ImageSettingsTapCommand
        {
            get => (ICommand)GetValue(ImageSettingsTapCommandProperty);
            set => SetValue(ImageSettingsTapCommandProperty, value);
        }

        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(nameof(SearchCommand),
            typeof(Command), typeof(CustomTabbedNavigationBar));

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        public static readonly BindableProperty FocusedCommandProperty = BindableProperty.Create(nameof(FocusedCommand), 
            typeof(Command), typeof(CustomTabbedNavigationBar));

        public ICommand FocusedCommand
        {
            get => (ICommand)GetValue(FocusedCommandProperty);
            set => SetValue(FocusedCommandProperty, value);
        }

        public static readonly BindableProperty ImageExitTapCommandProperty = BindableProperty.Create(nameof(ImageExitTapCommand),
            typeof(Command), typeof(CustomTabbedNavigationBar));

        public ICommand ImageExitTapCommand
        {
            get => (ICommand)GetValue(ImageExitTapCommandProperty);
            set => SetValue(ImageExitTapCommandProperty, value);
        }
    }
}