using GpsNotepad.Models;
using GpsNotepad.Services.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GpsNotepad.Services.PinImage
{
    class PinImageService : IPinImageService
    {
        private IRepository _repository;

        public PinImageService(IRepository repository)
        {
            _repository = repository;
        }

        public Task DeletePinImageAsync(PinImageModel pinImageModel)
        {
            return _repository.DeleteAsync(pinImageModel);
        }

        public async Task<List<PinImageModel>> GetAllPinImagesAsync(int pinId)
        {
            var pinImages = await _repository.GetAllAsync<PinImageModel>();
            return pinImages.Where(p => p.PinId == pinId).ToList();
        }

        public Task SavePinImageAsync(PinImageModel pinImageModel)
        {
            return _repository.InsertAsync(pinImageModel);
        }
    }
}
