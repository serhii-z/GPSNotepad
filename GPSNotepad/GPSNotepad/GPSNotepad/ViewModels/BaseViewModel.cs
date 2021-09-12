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

        #region -- IInitialize implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {           
        }

        #endregion

        #region -- INavigatedAware implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {         
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {           
        }

        #endregion
    }
}
