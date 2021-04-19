using GpsNotepad.Models;
using GpsNotepad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task DeletePinAsync(PinImageModel pinImageModel)
        {
            await _repository.DeleteAsync(pinImageModel);
        }

        public async Task<List<PinImageModel>> GetAllImagesAsync(int pinId)
        {
            var pinImages = await _repository.GetAllAsync<PinImageModel>();
            return pinImages.Where(x => x.PinId == pinId).ToList();
        }

        public async Task InsertPinAsync(PinImageModel pinImageModel)
        {
            await _repository.InsertAsync(pinImageModel);
        }
    }
}
