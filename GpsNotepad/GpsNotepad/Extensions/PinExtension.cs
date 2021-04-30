using GpsNotepad.Models.Pin;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Extensions
{
    static class PinExtension
    {
        public static Pin ToPin(this PinModel pinModel)
        {
            var pin = new Pin()
            {
                Label = pinModel.Label,
                Position = new Position(pinModel.Latitude, pinModel.Longitude),
                Address = pinModel.Address,
                IsVisible = pinModel.IsFavorite
            };

            return pin;
        }

        public static Pin ToPin(this PinViewModel pinViewModel)
        {
            var pin = new Pin()
            {
                Label = pinViewModel.Label,
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                Address = pinViewModel.Address,
                IsVisible = pinViewModel.IsFavorite
            };

            return pin;
        }

        public static PinViewModel ToPinViewModel(this PinModel pinModel)
        {
            var pinViewModel = new PinViewModel()
            {
                PinId = pinModel.Id,
                Label = pinModel.Label,
                Latitude = pinModel.Latitude,
                Longitude = pinModel.Longitude,
                Address = pinModel.Address,
                Description = pinModel.Description,
                IsFavorite = pinModel.IsFavorite,
                UserId = pinModel.UserId
            };

            return pinViewModel;
        }

        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            var pinModel = new PinModel()
            {
                Id = pinViewModel.PinId,
                Label = pinViewModel.Label,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Address = pinViewModel.Address,
                Description = pinViewModel.Description,
                IsFavorite = pinViewModel.IsFavorite,
                UserId = pinViewModel.UserId
            };

            return pinModel;
        }
    }
}
