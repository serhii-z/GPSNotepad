using GPSNotepad.Views;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public StartViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region -- Public properties --

        public ICommand LogInButtonTapCommand => new Command(OnLogInButtonTapAsync);

        public ICommand CreateAccountButtonTapCommand => new Command(OnCreateAccountButtonTapAsync);



        #endregion

        #region -- Private helpers --

        private async void OnLogInButtonTapAsync(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(SignInView)}");
        }

        private async void OnCreateAccountButtonTapAsync(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(StartSignUpView)}");
        }

        #endregion
    }
}
