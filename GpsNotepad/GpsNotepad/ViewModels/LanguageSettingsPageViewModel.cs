using GpsNotepad.Services.Localization;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GpsNotepad.ViewModels
{
    class LanguageSettingsPageViewModel : BaseViewModel
    {

        public LanguageSettingsPageViewModel(INavigationService navigationService,
                                             ILocalizationService localizationService) : base(navigationService, localizationService)
        {
        }

        private object _selectedLanguage;

        public object SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            switch (Resource.Lang)
            {
                case Constants.ENGLISH_LANGUAGE:
                    SelectedLanguage = Constants.ENGLISH_LANGUAGE;
                    break;
                case Constants.RUSSIAN_LANGUAGE:
                    SelectedLanguage = Constants.RUSSIAN_LANGUAGE;
                    break;
            }
            //SelectedLanguage = Resource.Lang;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedLanguage))
            {
                Resource.Lang = SelectedLanguage.ToString();
                Resource.SetCulture(SelectedLanguage.ToString());
            }
        }

    }
}
