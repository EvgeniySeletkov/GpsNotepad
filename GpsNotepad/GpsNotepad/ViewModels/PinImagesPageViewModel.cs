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
                Image = image,
                PinId = _pinId
            };

            return pinImagemodel;
        }

        private void SaveImage(string image)
        {
            var pinImageModel = CreatePinImageModel(image);
            _pinImageService.InsertPinAsync(pinImageModel);
            PinImageModelList.Add(pinImageModel);
        }
        
        private async void TakePhotoFromGallery()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                string image = photo.FullPath;
                SaveImage(image);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, "Alert", "OK");
            }
        }

        private async void TakePhotoWithCamera()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("ddMMyyyyhhmmss")}.jpg"
                });

                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }

                string image = photo.FullPath;
                SaveImage(image);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message, "Alert", "OK");
            }
        }

        private void OnAddImageTap()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig().
                SetTitle("Alert").
                Add("Gallery", GalleryAction, "ic_collections_black.png").
                Add("Camera", CameraAction, "ic_camera_alt_black.png"));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            _pinId = parameters.GetValue<int>("pinId");
            var pinImages = await _pinImageService.GetAllImagesAsync(_pinId);
            PinImageModelList = new ObservableCollection<PinImageModel>(pinImages);
        }

        //public override async void Initialize(INavigationParameters parameters)
        //{
        //    var pinImages = await _pinImageService.GetAllImagesAsync(1);
        //    PinImageModelList = new ObservableCollection<PinImageModel>(pinImages);
        //}
    }
}
