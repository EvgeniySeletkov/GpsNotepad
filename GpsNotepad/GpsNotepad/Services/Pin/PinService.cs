using GpsNotepad.Models.Pin;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.Pin
{
    class PinService : IPinService
    {
        private IRepository _repository;
        private ISettingsManager _settingsManager;

        public PinService(IRepository repository,
                          ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        public Task DeletePinAsync(PinModel pinModel)
        {
            return _repository.DeleteAsync(pinModel);
        }

        public async Task<List<PinModel>> GetAllPinsAsync()
        {
            int userId = _settingsManager.UserId;
            var pins = await _repository.GetAllAsync<PinModel>();
            return pins.Where(x => x.UserId == userId).ToList();
        }

        public Task InsertPinAsync(PinModel pinModel)
        {
            pinModel.UserId = _settingsManager.UserId;
            return _repository.InsertAsync(pinModel);
        }

        public Task UpdatePinAsync(PinModel pinModel)
        {
            return _repository.UpdateAsync(pinModel);
        }

        public async Task<string> GetAddressAsync(Position position)
        {
            var geocoder = new Geocoder();
            var addressList = await geocoder.GetAddressesForPositionAsync(position);
            var fullAddress = addressList != null ? addressList.FirstOrDefault() : string.Empty;
            var address = !string.IsNullOrWhiteSpace(fullAddress) ? fullAddress : string.Empty;

            return address;
        }

        public IEnumerable<PinViewModel> SearchPin(List<PinViewModel> pinList, string searchText)
        {
            var pinViewModelList = pinList.Where(p =>
                           p.Label.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                           p.Latitude.ToString().StartsWith(searchText) ||
                           p.Longitude.ToString().StartsWith(searchText) ||
                           (!string.IsNullOrWhiteSpace(p.Description) &&
                           p.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)));

            return pinViewModelList;
        }
    }
}
