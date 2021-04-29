using Prism.Unity;
using Prism;
using Xamarin.Forms;
using Prism.Ioc;
using GPSNotepad.Views;
using GPSNotepad.ViewModels;
using GPSNotepad.Services.Repository;
using GPSNotepad.Services.Authentication;
using GPSNotepad.Services.Authorization;
using GPSNotepad.Services.Pin;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using GPSNotepad.Services.SettingsService;
using System.Threading.Tasks;
using GPSNotepad.Services.Permissions;
using GPSNotepad.Services.GeoLocations;
using GPSNotepad.Services.Weather;
using GPSNotepad.Services.Time;
using System;
using GPSNotepad.Services.Resources;

namespace GPSNotepad
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<FirstView, FirstViewModel>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<StartSignUpView, StartSignUpViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedView>();
            containerRegistry.RegisterForNavigation<MapTabView, MapTabViewModel>();
            containerRegistry.RegisterForNavigation<PinListTabView, PinListTabViewModel>();
            containerRegistry.RegisterForNavigation<AddPinView, AddPinViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<ColorClockView, ColorClockViewModel>();

            //Dialogs
            containerRegistry.RegisterDialog<ClockView, ClockViewModel>();

            //Packages
            containerRegistry.RegisterInstance<ISettings>(CrossSettings.Current);
           

            //Services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<IPermissionService>(Container.Resolve<PermissionService>());
            containerRegistry.RegisterInstance<IGeoLocationService>(Container.Resolve<GeoLocationService>());
            containerRegistry.RegisterInstance<IWeatherService>(Container.Resolve<WeatherService>());
            containerRegistry.RegisterInstance<ITimeService>(Container.Resolve<TimeService>());
            containerRegistry.RegisterInstance<IResourceService>(Container.Resolve<ResourceService>());
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
          
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            var isAuthorized = Container.Resolve<AuthorizationService>().IsAuthorized;

            if (isAuthorized)
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainTabbedView)}");
            }
            else
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(FirstView)}");
            }   
        }
    }
}
