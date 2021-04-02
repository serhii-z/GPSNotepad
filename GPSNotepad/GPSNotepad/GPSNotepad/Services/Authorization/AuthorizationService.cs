using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GPSNotepad.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        public int Id 
        {
            get => Preferences.Get(nameof(Id), 0);
            set => Preferences.Set(nameof(Id), value); 
        }
    }
}
