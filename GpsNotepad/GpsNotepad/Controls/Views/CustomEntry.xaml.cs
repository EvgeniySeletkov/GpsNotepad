using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls.Views
{
    public partial class CustomEntry : StackLayout
    {
        public CustomEntry()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SubtitleTextProperty =
            BindableProperty.Create(
                propertyName: nameof(SubtitleText),
                returnType: typeof(string),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string SubtitleText
        {
            get => (string)GetValue(SubtitleTextProperty);
            set => SetValue(SubtitleTextProperty, value);
        }

        public static readonly BindableProperty WrongTextProperty =
            BindableProperty.Create(
                propertyName: nameof(WrongText),
                returnType: typeof(string),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string WrongText
        {
            get => (string)GetValue(WrongTextProperty);
            set => SetValue(WrongTextProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SubtitleText))
            {
                Subtitle.Text = SubtitleText;
            }
            if (propertyName == nameof(WrongText))
            {
                WrongLabel.Text = WrongText;
            }
        }
    }
}