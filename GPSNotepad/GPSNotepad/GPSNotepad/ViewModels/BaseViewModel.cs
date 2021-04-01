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

        public virtual void Initialize(INavigationParameters parameters)
        {           
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {         
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {           
        }
    }
}
