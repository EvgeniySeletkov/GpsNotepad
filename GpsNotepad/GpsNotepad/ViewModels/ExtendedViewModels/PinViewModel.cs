﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.ViewModels.ExtendedViewModels
{
    class PinViewModel : BindableBase
    {
        //_pinId
        private int pinId;
        public int PinId
        {
            get => pinId;
            set => SetProperty(ref pinId, value);
        }

        //rename
        private string label;
        public string Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        private double latitude;
        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        public string Coordinates
        {
            get => $"{Latitude}; {Longitude}";
        }


        private double longitude;
        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        private string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        //_isFavorite
        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
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

        //add command PinClickCommand
    }
}
