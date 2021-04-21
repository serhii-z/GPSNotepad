using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin;

namespace GPSNotepad.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            FormsGoogleMaps.Init("AIzaSyBsit-18ALcqPPuyDQsMsWXYIX06g47RbE");
            LoadApplication(new App(new IOSInitializer()));   
            return base.FinishedLaunching(app, options);
        }

        public class IOSInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                
            }
        }
    }
}
