using Prism.Unity;
using Prism;
using Xamarin.Forms;
using Prism.Ioc;
using GPSNotepad.Views;
using GPSNotepad.ViewModels;
using GPSNotepad.Services.Repositiry;
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

namespace GPSNotepad
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

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
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedView>();
            containerRegistry.RegisterForNavigation<MapTabView, MapTabViewModel>();
            containerRegistry.RegisterForNavigation<PinListTabView, PinListTabViewModel>();
            containerRegistry.RegisterForNavigation<AddPinView, AddPinViewModel>();
            containerRegistry.RegisterForNavigation<PinInfoView, PinInfoViewModel>();
            containerRegistry.RegisterForNavigation<ClockView, ClockWiewModel>();

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
        }

        protected async override void OnInitialized()
        {
            await GoToViewAsync();
        }

        private async Task GoToViewAsync()
        {
            var isAuthorized = Container.Resolve<AuthorizationService>().IsAuthorized;

            if (isAuthorized)
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainTabbedView)}");
            }             
            else
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
            }         
        }
    }
}
