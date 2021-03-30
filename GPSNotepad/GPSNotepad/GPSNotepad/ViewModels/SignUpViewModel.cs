using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public SignUpViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private string _entryNameText;
        public string EntryNameText
        {
            get => _entryNameText;
            set => SetProperty(ref _entryNameText, value);
        }

        private string _entryEmailText;
        public string EntryEmailText
        {
            get => _entryEmailText;
            set => SetProperty(ref _entryEmailText, value);
        }

        private string _entryPasswordText;
        public string EntryPasswordText
        {
            get => _entryPasswordText;
            set => SetProperty(ref _entryPasswordText, value);
        }

        private string _entryConfitmPasswordText;
        public string EntryConfirmPasswordText
        {
            get => _entryConfitmPasswordText;
            set => SetProperty(ref _entryConfitmPasswordText, value);
        }

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        #endregion

        #region --- Private Methods ---

        private void CheckTextInput(string elementText)
        {
            if (string.IsNullOrEmpty(elementText))
            {
                DeactivateButton();
            }
            else
            {
                ActivateButton();
            }
        }

        private void ActivateButton()
        {
            if (!string.IsNullOrEmpty(_entryNameText) &&
                !string.IsNullOrEmpty(_entryEmailText) &&
                !string.IsNullOrEmpty(_entryPasswordText) &&
                !string.IsNullOrEmpty(_entryConfitmPasswordText))
                EnabledButton = true;
        }

        private void DeactivateButton()
        {
            EnabledButton = false;
        }

        #endregion

        #region ---- Private Helpers ---

        private void OnSignUpTap(object obj)
        {
            
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryNameText))
            {
                CheckTextInput(_entryNameText);
            }

            if (args.PropertyName == nameof(EntryEmailText))
            {
                CheckTextInput(_entryEmailText);
            }

            if (args.PropertyName == nameof(EntryPasswordText))
            {
                CheckTextInput(_entryPasswordText);
            }

            if (args.PropertyName == nameof(EntryConfirmPasswordText))
            {
                CheckTextInput(_entryConfitmPasswordText);
            }
        }

        #endregion
    }
}
