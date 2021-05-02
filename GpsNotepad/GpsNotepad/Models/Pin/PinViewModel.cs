using Prism.Mvvm;

namespace GpsNotepad.Models.Pin
{
    class PinViewModel : BindableBase
    {
        private int _pinId;
        public int PinId
        {
            get => _pinId;
            set => SetProperty(ref _pinId, value);
        }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }


        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public string Coordinates
        {
            get => $"{Latitude}, {Longitude}";
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private bool isFavorite;
        public bool IsFavorite
        {
            get => isFavorite;
            set => SetProperty(ref isFavorite, value);
        }

        private string description;

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }


        private string image;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        private int userId;
        public int UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }

    }
}
