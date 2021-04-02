using Prism.Unity;
using Prism;
using Xamarin.Forms;
using Prism.Ioc;
using GPSNotepad.Views;
using GPSNotepad.ViewModels;
using GPSNotepad.Services.Repositiry;
using GPSNotepad.Services.Authentication;
using GPSNotepad.Services.Authorization;

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
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<PinListView, PinListViewModel>();
            containerRegistry.RegisterForNavigation<AddPinView, AddPinViewModel>();

            //Packages

            //Services
            containerRegistry.RegisterInstance(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
        }

        protected override void OnInitialized()
        {
            GoToView();
        }

        private async void GoToView()
        {
            var userId = Container.Resolve<AuthorizationService>().Id;

            if(userId > 0)
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainView)}");
            else
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
        }
    }
}
