using GpsNotepad.Models;
using GpsNotepad.ViewModels.ExtendedViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Extensions
{
    static class PinExtension
    {
        public static Pin GetPin(this PinModel pinModel)
        {
            var pin = new Pin()
            {
                Label = pinModel.Label,
                Position = new Position(pinModel.Latitude, pinModel.Longitude),
                Address = pinModel.Address,
                IsVisible = pinModel.IsVisible
            };

            return pin;
        }

        public static Pin GetPin(this PinViewModel pinViewModel)
        {
            var pin = new Pin()
            {
                Label = pinViewModel.Label,
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                Address = pinViewModel.Address,
                IsVisible = pinViewModel.IsVisible
            };

            return pin;
        }

        public static PinViewModel GetPinViewModel(this PinModel pinModel)
        {
            var pinViewModel = new PinViewModel()
            {
                PinId = pinModel.Id,
                Label = pinModel.Label,
                Latitude = pinModel.Latitude,
                Longitude = pinModel.Longitude,
                Address = pinModel.Address,
                IsVisible = pinModel.IsVisible,
                UserId = pinModel.UserId
            };

            return pinViewModel;
        }

        public static PinModel GetPinModel(this PinViewModel pinViewModel)
        {
            var pinModel = new PinModel()
            {
                Id = pinViewModel.PinId,
                Label = pinViewModel.Label,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Address = pinViewModel.Address,
                IsVisible = pinViewModel.IsVisible,
                UserId = pinViewModel.UserId
            };

            return pinModel;
        }
    }
}
