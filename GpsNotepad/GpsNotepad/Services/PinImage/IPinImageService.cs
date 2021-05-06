using GpsNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsNotepad.Services.PinImage
{
    interface IPinImageService
    {
        Task<List<PinImageModel>> GetAllPinImagesAsync(int pinId);

        Task SavePinImageAsync(PinImageModel pinImageModel);

        Task DeletePinImageAsync(PinImageModel pinImageModel);

    }
}
