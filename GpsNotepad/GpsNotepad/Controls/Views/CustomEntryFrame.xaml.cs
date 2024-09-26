using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.Controls.Views
{
    public partial class CustomEntryFrame : StackLayout
    {
        public CustomEntryFrame()
        {
            InitializeComponent();
        }

        #region --- Public properties ---

        public static readonly BindableProperty SubtitleFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(SubtitleFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double SubtitleFontSize
        {
            get => (double)GetValue(SubtitleFontSizeProperty);
            set => SetValue(SubtitleFontSizeProperty, value);
        }

        public static readonly BindableProperty SubtitleFontProperty =
            BindableProperty.Create(
                propertyName: nameof(SubtitleFont),
                returnType: typeof(string),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string SubtitleFont
        {
            get => (string)GetValue(SubtitleFontProperty);
            set => SetValue(SubtitleFontProperty, value);
        }

        public static readonly BindableProperty SubtitleTextColorProperty =
            BindableProperty.Create(
                propertyName: nameof(SubtitleTextColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string SubtitleText
        {
            get => (string)GetValue(SubtitleTextProperty);
            set => SetValue(SubtitleTextProperty, value);
        }

        public static readonly BindableProperty IsSubtitleVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsSubtitleVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsSubtitleVisible
        {
            get => (bool)GetValue(IsSubtitleVisibleProperty);
            set => SetValue(IsSubtitleVisibleProperty, value);
        }

        public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryBorderColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryBorderColor
        {
            get => (Color)GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        public static readonly BindableProperty EntryNormalBorderColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryNormalBorderColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryNormalBorderColor
        {
            get => (Color)GetValue(EntryNormalBorderColorProperty);
            set => SetValue(EntryNormalBorderColorProperty, value);
        }

        public static readonly BindableProperty EntryBackgoundColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryBackgoundColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color EntryBackgoundColor
        {
            get => (Color)GetValue(EntryBackgoundColorProperty);
            set => SetValue(EntryBackgoundColorProperty, value);
        }

        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double EntryFontSize
        {
            get => (double)GetValue(EntryFontSizeProperty);
            set => SetValue(EntryFontSizeProperty, value);
        }

        public static readonly BindableProperty EntryFontProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryFont),
                returnType: typeof(string),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string EntryFont
        {
            get => (string)GetValue(EntryFontProperty);
            set => SetValue(EntryFontProperty, value);
        }

        public static readonly BindableProperty EntryPlaceholderColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryPlaceholderColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty EntryKeyboardProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryKeyboard),
                returnType: typeof(Keyboard),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Keyboard EntryKeyboard
        {
            get => (Keyboard)GetValue(EntryKeyboardProperty);
            set => SetValue(EntryKeyboardProperty, value);
        }

        public static readonly BindableProperty IsEntryPasswordProperty =
            BindableProperty.Create(
                propertyName: nameof(IsEntryPassword),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsEntryPassword
        {
            get => (bool)GetValue(IsEntryPasswordProperty);
            set => SetValue(IsEntryPasswordProperty, value);
        }

        public static readonly BindableProperty WrongFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(WrongFontSize),
                returnType: typeof(double),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double WrongFontSize
        {
            get => (double)GetValue(WrongFontSizeProperty);
            set => SetValue(WrongFontSizeProperty, value);
        }

        public static readonly BindableProperty WrongFontProperty =
            BindableProperty.Create(
                propertyName: nameof(WrongFont),
                returnType: typeof(string),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string WrongFont
        {
            get => (string)GetValue(WrongFontProperty);
            set => SetValue(WrongFontProperty, value);
        }

        public static readonly BindableProperty WrongColorProperty =
            BindableProperty.Create(
                propertyName: nameof(WrongColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsWrongVisible
        {
            get => (bool)GetValue(IsWrongVisibleProperty);
            set => SetValue(IsWrongVisibleProperty, value);
        }

        public static readonly BindableProperty ButtonImageProperty =
            BindableProperty.Create(
                propertyName: nameof(ButtonImage),
                returnType: typeof(ImageSource),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ImageSource ButtonImage
        {
            get => (ImageSource)GetValue(ButtonImageProperty);
            set => SetValue(ButtonImageProperty, value);
        }

        public static readonly BindableProperty IsButtonVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsButtonVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsButtonVisible
        {
            get => (bool)GetValue(IsButtonVisibleProperty);
            set => SetValue(IsButtonVisibleProperty, value);
        }

        public static readonly BindableProperty ClickCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(ClickCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            
            switch (propertyName)
            {
                case nameof(EntryNormalBorderColor):
                    EntryBorderColor = EntryNormalBorderColor;
                    break;
                case nameof(EntryText):
                    IsButtonVisible = string.IsNullOrWhiteSpace(EntryText) ? false : true;
                    break;
                case nameof(WrongText):
                    if (string.IsNullOrWhiteSpace(WrongText))
                    {
                        EntryBorderColor = EntryNormalBorderColor;
                    }
                    else
                    {
                        EntryBorderColor = WrongColor;
                    }
                    break;
            }
        }

        #endregion

    }
}