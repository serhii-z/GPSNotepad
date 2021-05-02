using System;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Views;
using GPSNotepad.Droid;
using GPSNotepad.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace GPSNotepad.Droid
{
    [Obsolete]
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        private BaseTabbedPage _baseTabbedPage;
        private BottomNavigationView _bottomNavigationView;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _baseTabbedPage = e.NewElement as BaseTabbedPage;
                _bottomNavigationView = (GetChildAt(0) as Android.Widget.RelativeLayout).GetChildAt(1) as BottomNavigationView;      
                _bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (_bottomNavigationView != null)
            {
                for (int i = 0; i < Element.Children.Count; i++)
                {
                    var item = _bottomNavigationView.Menu.GetItem(i);
                    if (_bottomNavigationView.SelectedItemId == item.ItemId)
                    {
                        SetupBottomNavigationView(item);
                        break;
                    }
                }
            }
        }

        void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            SetupBottomNavigationView(e.Item);
            this.OnNavigationItemSelected(e.Item);
        }

        void SetupBottomNavigationView(IMenuItem item)
        {
            var color = _baseTabbedPage.TapColor;

            var bottomOffset = 112;
            var itemHeight = _bottomNavigationView.Height - bottomOffset;
            var itemWidth = (_bottomNavigationView.Width / Element.Children.Count);
            var leftOffset = item.ItemId * itemWidth;
            var rightOffset = itemWidth * (Element.Children.Count - (item.ItemId + 1));

            GradientDrawable bottomRect = new GradientDrawable();
            bottomRect.SetShape(ShapeType.Rectangle);         
            bottomRect.SetTint(color.ToAndroid());

            var layerDrawable = new LayerDrawable(new Drawable[] { bottomRect });
            layerDrawable.SetLayerInset(0, leftOffset, itemHeight, rightOffset, 0);

            _bottomNavigationView.SetBackground(layerDrawable);
        }
    }
}