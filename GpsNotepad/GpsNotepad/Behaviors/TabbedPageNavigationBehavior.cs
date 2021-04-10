using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GpsNotepad.Behaviors
{
    class TabbedPageNavigationBehavior : BehaviorBase<TabbedPage>
    {
        private Page _currentPage;

        protected override void OnAttachedTo(TabbedPage bindable)
        {
            bindable.CurrentPageChanged += OnCurrentPageChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(TabbedPage bindable)
        {
            bindable.CurrentPageChanged -= OnCurrentPageChanged;
            base.OnDetachingFrom(bindable);
        }

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            var newPage = AssociatedObject.CurrentPage;

            if (_currentPage != null)
            {
                var parameters = new NavigationParameters();
                PageUtilities.OnNavigatedFrom(_currentPage, parameters);
                PageUtilities.OnNavigatedTo(newPage, parameters);
            }

            _currentPage = newPage;
        }
    }
}
