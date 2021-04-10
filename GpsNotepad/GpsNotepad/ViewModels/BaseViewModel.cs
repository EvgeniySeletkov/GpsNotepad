using GpsNotepad.Services.Localization;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.ViewModels
{
    class BaseViewModel : BindableBase, IInitialize, INavigationAware
    {
        protected INavigationService _navigationService { get; private set; }
        public ILocalizationService Resource{ get; private set; }

        public BaseViewModel(INavigationService navigationService,
                              ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            Resource = localizationService;
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

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }
    }
}
