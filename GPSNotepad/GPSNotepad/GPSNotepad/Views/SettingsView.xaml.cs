using System;
using Xamarin.Forms;

namespace GPSNotepad.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}