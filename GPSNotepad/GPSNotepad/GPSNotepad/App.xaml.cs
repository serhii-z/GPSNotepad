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

            //Packages
            containerRegistry.RegisterInstance<ISettings>(CrossSettings.Current);

            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
        }

        protected override void OnInitialized()
        {
            GoToView();
        }

        private async void GoToView()
        {
            //var userId = Container.Resolve<AuthorizationService>().UserId;
            var userId = 1;

            if(userId > 0)
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainTabbedView)}");
            else
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
        }
    }
}
