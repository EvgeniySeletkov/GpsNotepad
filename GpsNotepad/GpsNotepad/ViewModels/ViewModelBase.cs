using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.ViewModels
{
    class ViewModelBase : BindableBase
    {
        protected INavigationService _navigationService { get; set; }

        public ViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
