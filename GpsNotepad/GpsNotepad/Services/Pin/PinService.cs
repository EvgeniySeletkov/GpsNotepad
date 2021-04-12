using GpsNotepad.Models;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task DeletePinAsync(PinModel pinModel)
        {
            await _repository.DeleteAsync(pinModel);
        }

        public async Task<List<PinModel>> GetAllPinsAsync()
        {
            int userId = _settingsManager.UserId;
            var pins = await _repository.GetAllAsync<PinModel>();
            return pins.Where(x => x.UserId == userId).ToList();
        }

        public async Task SavePinAsync(PinModel pinModel)
        {
            if (pinModel.Id == 0)
            {
                pinModel.UserId = _settingsManager.UserId;
                await _repository.InsertAsync(pinModel);
            }
            else
            {
                await _repository.UpdateAsync(pinModel);
            }
        }
    }
}
