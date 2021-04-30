using Acr.UserDialogs;
using GpsNotepad.Models;
using GpsNotepad.Services.Localization;
using GpsNotepad.Services.PinImage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace GpsNotepad.ViewModels
{
    class PinImagesPageViewModel : BaseViewModel
    {
        private int _pinId;

        private IPinImageService _pinImageService;

        private Action GalleryAction;
        private Action CameraAction;

        public PinImagesPageViewModel(INavigationService navigationService, 
                                      ILocalizationService localizationService,
                                      IPinImageService pinImageService) : base(navigationService, localizationService)
        {
            _pinImageService = pinImageService;

            //refactor
            GalleryAction += TakePhotoFromGallery;
            CameraAction += TakePhotoWithCamera;
        }

        private ObservableCollection<PinImageModel> pinImageModelList;
        public ObservableCollection<PinImageModel> PinImageModelList
        {
            get => pinImageModelList;
            set => SetProperty(ref pinImageModelList, value);
        }

        private ICommand addImageTapCommand;
        public ICommand AddImageTapCommand => 
            addImageTapCommand ?? (addImageTapCommand = new DelegateCommand(OnAddImageTap));

        private PinImageModel CreatePinImageModel(string image)
        {
            var pinImagemodel = new PinImageModel()
            {
                ImagePath = image,
                PinId = _pinId
            };

            return pinImagemodel;
        }

        private void SaveImage(string image)
        {
            var pinImageModel = CreatePinImageModel(image);

            //await!
            _pinImageService.SavePinImageAsync(pinImageModel);
            PinImageModelList.Add(pinImageModel);
        }
        
        private async void TakePhotoFromGallery()
        {
            
            
        }

        private async void TakePhotoWithCamera()
        {
            //create MediaService
            
        }

        private void OnAddImageTap()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig().
                SetTitle("Alert").
                Add("Gallery", TakePhotoFromGallery, "ic_collections_black.png").
                Add("Camera", TakePhotoWithCamera, "ic_camera_alt_black.png"));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            //trygetvalue
            _pinId = parameters.GetValue<int>("pinId");
            var pinImages = await _pinImageService.GetAllPinImagesAsync(_pinId);
            PinImageModelList = new ObservableCollection<PinImageModel>(pinImages);
        }

        //public override async void Initialize(INavigationParameters parameters)
        //{
        //    var pinImages = await _pinImageService.GetAllImagesAsync(1);
        //    PinImageModelList = new ObservableCollection<PinImageModel>(pinImages);
        //}
    }
}
