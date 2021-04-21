using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNotepad.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEntry
    {
        public CustomEntry()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ImageTapCommandProperty = BindableProperty.Create(nameof(ImageTapCommand), typeof(Command),
            typeof(CustomNavigationBar));

        public ICommand ImageTapCommand
        {
            get => (ICommand)GetValue(ImageTapCommandProperty);
            set => SetValue(ImageTapCommandProperty, value);
        }

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText),
            typeof(string), typeof(CustomEntry));
        
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText),
            typeof(string), typeof(CustomEntry),defaultBindingMode: BindingMode.TwoWay);

        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder),
            typeof(string), typeof(CustomEntry));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty IsHidePasswordProperty = BindableProperty.Create(nameof(IsHidePassword),
            typeof(bool), typeof(CustomEntry));

        public bool IsHidePassword
        {
            get => (bool)GetValue(IsHidePasswordProperty);
            set => SetValue(IsHidePasswordProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource),
            typeof(ImageSource), typeof(CustomEntry));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }


        public static readonly BindableProperty LabelErrorTextProperty = BindableProperty.Create(nameof(LabelErrorText),
            typeof(string), typeof(CustomEntry));

        public string LabelErrorText
        {
            get => (string)GetValue(LabelErrorTextProperty);
            set => SetValue(LabelErrorTextProperty, value);
        }

        public static readonly BindableProperty IsVisibleErrorProperty = BindableProperty.Create(nameof(IsVisibleError),
            typeof(bool), typeof(CustomEntry));

        public bool IsVisibleError
        {
            get => (bool)GetValue(IsVisibleErrorProperty);
            set => SetValue(IsVisibleErrorProperty, value);
        }

        public static readonly BindableProperty IsVisibleImageProperty = BindableProperty.Create(nameof(IsVisibleImage),
            typeof(bool), typeof(CustomEntry));

        public bool IsVisibleImage
        {
            get => (bool)GetValue(IsVisibleImageProperty);
            set => SetValue(IsVisibleImageProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry));

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
    }
}