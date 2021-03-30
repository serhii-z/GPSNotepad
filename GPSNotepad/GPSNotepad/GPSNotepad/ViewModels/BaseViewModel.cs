using Prism.Mvvm;
using Prism.Navigation;

namespace GPSNotepad.ViewModels
{
    public class BaseViewModel : BindableBase, IInitialize, INavigatedAware
    {
        protected readonly INavigationService navigationService;
        public BaseViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void Initialize(INavigationParameters parameters)
        {           
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {         
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {           
        }
    }
}
