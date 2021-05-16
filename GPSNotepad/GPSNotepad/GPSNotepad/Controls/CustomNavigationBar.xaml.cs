using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationBar
    {
        public CustomNavigationBar()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty ImageLeftTapCommandProperty = BindableProperty.Create(nameof(ImageLeftTapCommand), 
            typeof(Command), typeof(CustomNavigationBar));

        public ICommand ImageLeftTapCommand
        {
            get => (ICommand)GetValue(ImageLeftTapCommandProperty);
            set => SetValue(ImageLeftTapCommandProperty, value);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
            typeof(string), typeof(CustomNavigationBar));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion
    }
}