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

        public static readonly BindableProperty PinSearchCommandProperty = BindableProperty.Create(nameof(PinSearchCommand),
            typeof(Command), typeof(CustomTabbedNavigationBar));

        public ICommand PinSearchCommand
        {
            get => (ICommand)GetValue(PinSearchCommandProperty);
            set => SetValue(PinSearchCommandProperty, value);
        }

        public static readonly BindableProperty SearchBarTapCommandProperty = BindableProperty.Create(nameof(SearchBarTapCommand), 
            typeof(Command), typeof(CustomTabbedNavigationBar));

        public ICommand SearchBarTapCommand
        {
            get => (ICommand)GetValue(SearchBarTapCommandProperty);
            set => SetValue(SearchBarTapCommandProperty, value);
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