using GpsNotepad.Models.Pin;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.Pin
{
    interface IPinService
    {

        Task<List<PinModel>> GetAllPinsAsync();

        Task InsertPinAsync(PinModel pinModel);

        Task UpdatePinAsync(PinModel pinModel);

        Task DeletePinAsync(PinModel pinModel);

        Task<string> GetAddressAsync(Position position);

        IEnumerable<PinViewModel> SearchPin(List<PinViewModel> pinList, string searchText);

    }
}
