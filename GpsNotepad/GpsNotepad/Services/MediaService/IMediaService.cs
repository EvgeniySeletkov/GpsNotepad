using System.Threading.Tasks;

namespace GpsNotepad.Services.MediaService
{
    interface IMediaService
    {
        Task<string> TakePhotoWithCameraAsync();

        Task<string> TakePhotoFromGalleryAsync();
    }
}
