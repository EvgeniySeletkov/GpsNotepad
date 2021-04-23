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
        protected INavigationService NavigationService { get; private set; }
        //protected
        public ILocalizationService Resource{ get; private set; }

        public BaseViewModel(INavigationService navigationService,
                              ILocalizationService localizationService)
        {
            NavigationService = navigationService;
            Resource = localizationService;
        }

        //regions
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
