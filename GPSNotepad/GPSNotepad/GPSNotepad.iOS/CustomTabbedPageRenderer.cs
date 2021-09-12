using CoreGraphics;
using GPSNotepad.iOS;
using GPSNotepad.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace GPSNotepad.iOS
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            AddSelectedTabIndicator();
        }

        void AddSelectedTabIndicator()
        {
            var bottomNavigationView = Element as MainTabbedView;

            if (base.ViewControllers != null)
            {
                UITabBar.Appearance.SelectionIndicatorImage = GetImageWithColorPosition(bottomNavigationView.TapColor.ToUIColor(),
                    new CGSize(UIScreen.MainScreen.Bounds.Width / base.ViewControllers.Length, TabBar.Bounds.Size.Height));
            }
        }
        UIImage GetImageWithColorPosition(UIColor color, CGSize size)
        {
            var safeAriaBottom = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom;

            if (safeAriaBottom > 0)
            {
                size.Height = 83;
            }
            
            var rect = new CGRect(0, 0, size.Width, size.Height - safeAriaBottom);

            UIGraphics.BeginImageContextWithOptions(size, false, 0);
            UIColor.Clear.SetFill();
            color.SetFill();
            UIGraphics.RectFill(rect);

            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return img;
        }
    }
}