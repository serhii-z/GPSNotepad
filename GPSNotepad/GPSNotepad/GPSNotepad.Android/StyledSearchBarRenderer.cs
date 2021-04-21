using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GPSNotepad.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(StyledSearchBarRenderer))]
namespace GPSNotepad.Droid
{
    [Obsolete]
    class StyledSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var color = global::Xamarin.Forms.Color.LightGray;
                var searchView = Control as SearchView;

                var searchVie = base.Control as SearchView;
                int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                ImageView searchViewIcon = (ImageView)searchView.FindViewById<ImageView>(searchIconId);
                searchViewIcon.SetImageDrawable(null);

            }
        }
    }
}