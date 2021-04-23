using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNotepad.Controls.Views
{
    //CustomEntry -> CustomFrame
    public partial class CustomEntry : StackLayout
    {
        public CustomEntry()
        {
            InitializeComponent();
        }

        // SUBTITLE.
        public static readonly BindableProperty SubtitleFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(SubtitleFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double SubtitleFontSize
        {
            get => (double)GetValue(SubtitleFontSizeProperty);
            set => SetValue(SubtitleFontSizeProperty, value);
        }

        public static readonly BindableProperty SubtitleTextColorProperty =
            BindableProperty.Create(
                propertyName: nameof(SubtitleTextColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color SubtitleTextColor
        {
            get => (Color)GetValue(SubtitleTextColorProperty);
            set => SetValue(SubtitleTextColorProperty, value);
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

        // FRAME
        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryBorderColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryBorderColor
        {
            get => (Color)GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        public static readonly BindableProperty EntryBackgoundColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryBackgoundColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryBackgoundColor
        {
            get => (Color)GetValue(EntryBackgoundColorProperty);
            set => SetValue(EntryBackgoundColorProperty, value);
        }

        // ENTRY
        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double EntryFontSize
        {
            get => (double)GetValue(EntryFontSizeProperty);
            set => SetValue(EntryFontSizeProperty, value);
        }

        public static readonly BindableProperty EntryPlaceholderColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryPlaceholderColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryPlaceholderColor
        {
            get => (Color)GetValue(EntryPlaceholderColorProperty);
            set => SetValue(EntryPlaceholderColorProperty, value);
        }

        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryPlaceholder),
                returnType: typeof(string),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string EntryPlaceholder
        {
            get => (string)GetValue(EntryPlaceholderProperty);
            set => SetValue(EntryPlaceholderProperty, value);
        }

        public static readonly BindableProperty EntryTextColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryTextColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryTextColor
        {
            get => (Color)GetValue(EntryTextColorProperty);
            set => SetValue(EntryTextColorProperty, value);
        }

        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryText),
                returnType: typeof(string),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty IsEntryPasswordProperty =
            BindableProperty.Create(
                propertyName: nameof(IsEntryPassword),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsEntryPassword
        {
            get => (bool)GetValue(IsEntryPasswordProperty);
            set => SetValue(IsEntryPasswordProperty, value);
        }

        // WRONG LABEL
        public static readonly BindableProperty WrongFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(WrongFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double WrongFontSize
        {
            get => (double)GetValue(WrongFontSizeProperty);
            set => SetValue(WrongFontSizeProperty, value);
        }

        public static readonly BindableProperty WrongColorProperty =
            BindableProperty.Create(
                propertyName: nameof(WrongColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color WrongColor
        {
            get => (Color)GetValue(WrongColorProperty);
            set => SetValue(WrongColorProperty, value);
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

        public static readonly BindableProperty IsWrongVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsWrongVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsWrongVisible
        {
            get => (bool)GetValue(IsWrongVisibleProperty);
            set => SetValue(IsWrongVisibleProperty, value);
        }

        // BUTTON
        public static readonly BindableProperty ButtonImageProperty =
            BindableProperty.Create(
                propertyName: nameof(ButtonImage),
                returnType: typeof(ImageSource),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ImageSource ButtonImage
        {
            get => (ImageSource)GetValue(ButtonImageProperty);
            set => SetValue(ButtonImageProperty, value);
        }

        public static readonly BindableProperty ClickCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(ClickCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(CustomEntry),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //use bindings
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SubtitleFontSize))
            {
                Subtitle.FontSize = SubtitleFontSize;
            }
            if (propertyName == nameof(SubtitleTextColor))
            {
                Subtitle.TextColor = SubtitleTextColor;
            }
            if (propertyName == nameof(SubtitleText))
            {
                Subtitle.Text = SubtitleText;
            }
            if (propertyName == nameof(EntryBorderColor))
            {
                Frame.BorderColor = EntryBorderColor;
            }
            if (propertyName == nameof(EntryBackgoundColor))
            {
                Frame.BackgroundColor = EntryBackgoundColor;
            }
            if (propertyName == nameof(EntryFontSize))
            {
                Entry.FontSize = EntryFontSize;
            }
            if (propertyName == nameof(EntryPlaceholderColor))
            {
                Entry.PlaceholderColor = EntryPlaceholderColor;
            }
            if (propertyName == nameof(EntryPlaceholder))
            {
                Entry.Placeholder = EntryPlaceholder;
            }
            if (propertyName == nameof(EntryTextColor))
            {
                Entry.TextColor = EntryTextColor;
            }
            if (propertyName == nameof(EntryText))
            {
                Entry.Text = EntryText;
            }
            if (propertyName == nameof(IsEntryPassword))
            {
                Entry.IsPassword = IsEntryPassword;
            }
            if (propertyName == nameof(WrongFontSize))
            {
                WrongLabel.FontSize = WrongFontSize;
            }
            if (propertyName == nameof(WrongColor))
            {
                WrongLabel.TextColor = WrongColor;
            }
            if (propertyName == nameof(WrongText))
            {
                WrongLabel.Text = WrongText;
            }
            if (propertyName == nameof(IsWrongVisible))
            {
                if (IsWrongVisible)
                {
                    WrongLabel.IsVisible = true;
                    Frame.BorderColor = WrongColor;
                }
                else
                {
                    WrongLabel.IsVisible = false;
                    Frame.BorderColor = EntryBorderColor;
                }
            }
            if (propertyName == nameof(ButtonImage))
            {
                EntryButton.Source = ButtonImage;
            }
            if (propertyName == nameof(ClickCommand))
            {
                EntryButton.Command = ClickCommand;
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryText = Entry.Text;
            if (string.IsNullOrWhiteSpace(Entry.Text))
            {
                EntryButton.IsVisible = false;
            }
            else
            {
                EntryButton.IsVisible = true;
            }
        }
    }
}