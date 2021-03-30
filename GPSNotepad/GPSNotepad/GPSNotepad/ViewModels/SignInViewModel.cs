﻿using GPSNotepad.Views;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNotepad.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        public SignInViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---
        public ICommand LogInTapCommand => new Command(OnLogInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        private bool _enabledButton = false;
        public bool EnabledButton
        {
            get => _enabledButton;
            set => SetProperty(ref _enabledButton, value);
        }

        #endregion

        #region --- Private Helpers ---
        private void OnLogInTap(object obj)
        {
            
        }

        private async void OnSignUpTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        #endregion
    }
}
