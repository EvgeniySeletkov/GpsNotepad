using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.ViewModels
{
    class BaseViewModel : BindableBase, IInitialize
    {
        protected INavigationService _navigationService { get; set; }

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {         
        }
    }
}
