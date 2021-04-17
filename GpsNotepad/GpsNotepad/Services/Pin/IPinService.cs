using GpsNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Pin
{
    interface IPinService
    {
        Task<List<PinModel>> GetAllPinsAsync();
        Task InsertPinAsync(PinModel pinModel);
        Task UpdatePinAsync(PinModel pinModel);
        Task DeletePinAsync(PinModel pinModel);
    }
}
