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

        //remove async-await and return task
        public async Task DeletePinAsync(PinImageModel pinImageModel)
        {
            await _repository.DeleteAsync(pinImageModel);
        }

        public async Task<List<PinImageModel>> GetAllImagesAsync(int pinId)
        {
            var pinImages = await _repository.GetAllAsync<PinImageModel>();
            return pinImages.Where(p => p.PinId == pinId).ToList();
        }

        //remove async-await and return task
        public async Task InsertPinAsync(PinImageModel pinImageModel)
        {
            await _repository.InsertAsync(pinImageModel);
        }
    }
}
