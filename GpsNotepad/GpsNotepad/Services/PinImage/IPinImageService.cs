using GpsNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.Services.PinImage
{
    interface IPinImageService
    {
        Task<List<PinImageModel>> GetAllImagesAsync(int pinId);
        Task InsertPinAsync(PinImageModel pinImageModel);
        Task DeletePinAsync(PinImageModel pinImageModel);
    }
}
