﻿using GPSNotepad.Views;
using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace GPSNotepad.Behaviors
{
    public class MainViewNavigationBehaviors : BehaviorBase<MainTabbedView>
    {
        private Page _currentPage;

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            var newPage = this.AssociatedObject.CurrentPage;

            if (this._currentPage != null)
            {
                var parameters = new NavigationParameters();
                PageUtilities.OnNavigatedFrom(this._currentPage, parameters);
                PageUtilities.OnNavigatedTo(newPage, parameters);
            }

            this._currentPage = newPage;
        }

        protected override void OnAttachedTo(MainTabbedView bindable)
        {
            bindable.CurrentPageChanged += this.OnCurrentPageChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(MainTabbedView bindable)
        {
            bindable.CurrentPageChanged -= this.OnCurrentPageChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
