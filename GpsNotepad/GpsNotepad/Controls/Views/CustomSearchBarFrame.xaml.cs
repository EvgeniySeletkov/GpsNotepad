using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls.Views
{
    public partial class CustomSearchBarFrame : StackLayout, INotifyPropertyChanged
    {
        public CustomSearchBarFrame()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty LeftButtonIsVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(LeftButtonIsVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomFrame),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool LeftButtonIsVisible
        {
            get => (bool)GetValue(LeftButtonIsVisibleProperty);
            private set => SetValue(LeftButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty BackButtonIsVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(BackButtonIsVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool BackButtonIsVisible
        {
            get => (bool)GetValue(BackButtonIsVisibleProperty);
            private set => SetValue(BackButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryBackgroundColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryBackgroundColor
        {
            get => (Color)GetValue(EntryBackgroundColorProperty);
            set => SetValue(EntryBackgroundColorProperty, value);
        }

        public static readonly BindableProperty EntryColumnSpanProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryColumnSpan),
                returnType: typeof(int),
                declaringType: typeof(CustomFrame),
                defaultValue: 1,
                defaultBindingMode: BindingMode.TwoWay);

        public int EntryColumnSpan
        {
            get => (int)GetValue(EntryColumnSpanProperty);
            private set => SetValue(EntryColumnSpanProperty, value);
        }

        public static readonly BindableProperty EntryFontProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryFont),
                returnType: typeof(string),
                declaringType: typeof(CustomFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string EntryFont
        {
            get => (string)GetValue(EntryFontProperty);
            set => SetValue(EntryFontProperty, value);
        }

        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double EntryFontSize
        {
            get => (double)GetValue(EntryFontSizeProperty);
            set => SetValue(EntryFontSizeProperty, value);
        }

        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryText),
                returnType: typeof(string),
                declaringType: typeof(CustomFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty RightButtonIsVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(RightButtonIsVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomFrame),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool RightButtonIsVisible
        {
            get => (bool)GetValue(RightButtonIsVisibleProperty);
            private set => SetValue(RightButtonIsVisibleProperty, value);
        }

        private void ChangeButtonsVisible()
        {
            LeftButtonIsVisible = !LeftButtonIsVisible;
            BackButtonIsVisible = !BackButtonIsVisible;
            RightButtonIsVisible = !RightButtonIsVisible;
        }

        private void CustomEntry_Focused(object sender, FocusEventArgs e)
        {
            ChangeButtonsVisible();
            EntryColumnSpan = 2;
        }

        private void CustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            ChangeButtonsVisible();
            EntryColumnSpan = 1;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            //ChangeButtonsVisible();
        }
    }
}