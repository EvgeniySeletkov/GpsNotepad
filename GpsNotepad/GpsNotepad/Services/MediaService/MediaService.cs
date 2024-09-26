using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GpsNotepad.Services.MediaService
{
    class MediaService : IMediaService
    {
        public async Task<string> TakePhotoFromGalleryAsync()
        {
            string image = null;
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                image = photo.FullPath;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return image;
        }

        public async Task<string> TakePhotoWithCameraAsync()
        {
            string image = null;
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }

                image = photo.FullPath;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return image;
        }
    }
}
