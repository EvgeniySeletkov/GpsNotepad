using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.Controls.Views
{
    public partial class CustomSearchBarFrame : StackLayout
    {
        public CustomSearchBarFrame()
        {
            InitializeComponent();
        }

        #region --- Public properties ---

        public static readonly BindableProperty LeftButtonImageProperty =
            BindableProperty.Create(
                propertyName: nameof(LeftButtonImage),
                returnType: typeof(ImageSource),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ImageSource LeftButtonImage
        {
            get => (ImageSource)GetValue(LeftButtonImageProperty);
            set => SetValue(LeftButtonImageProperty, value);
        }

        public static readonly BindableProperty IsLeftButtonVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsLeftButtonVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsLeftButtonVisible
        {
            get => (bool)GetValue(IsLeftButtonVisibleProperty);
            private set => SetValue(IsLeftButtonVisibleProperty, value);
        }

        public static readonly BindableProperty LeftButtonClickCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(LeftButtonClickCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand LeftButtonClickCommand
        {
            get => (ICommand)GetValue(LeftButtonClickCommandProperty);
            set => SetValue(LeftButtonClickCommandProperty, value);
        }

        public static readonly BindableProperty BackButtonImageProperty =
            BindableProperty.Create(
                propertyName: nameof(BackButtonImage),
                returnType: typeof(ImageSource),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ImageSource BackButtonImage
        {
            get => (ImageSource)GetValue(BackButtonImageProperty);
            set => SetValue(BackButtonImageProperty, value);
        }

        public static readonly BindableProperty IsBackButtonVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsBackButtonVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsBackButtonVisible
        {
            get => (bool)GetValue(IsBackButtonVisibleProperty);
            private set => SetValue(IsBackButtonVisibleProperty, value);
        }

        public static readonly BindableProperty BackButtonClickCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(BackButtonClickCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand BackButtonClickCommand
        {
            get => (ICommand)GetValue(BackButtonClickCommandProperty);
            set => SetValue(BackButtonClickCommandProperty, value);
        }

        public static readonly BindableProperty IsSearchBarFocusedProperty =
            BindableProperty.Create(
                propertyName: nameof(IsSearchBarFocused),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsSearchBarFocused
        {
            get => (bool)GetValue(IsSearchBarFocusedProperty);
            set => SetValue(IsSearchBarFocusedProperty, value);
        }

        public static readonly BindableProperty EntryBackgroundColorProperty =
            BindableProperty.Create(
                propertyName: nameof(EntryBackgroundColor),
                returnType: typeof(Color),
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
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
                declaringType: typeof(CustomEntryFrame),
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

        public static readonly BindableProperty ClearButtonImageProperty =
            BindableProperty.Create(
                propertyName: nameof(ClearButtonImage),
                returnType: typeof(ImageSource),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ImageSource ClearButtonImage
        {
            get => (ImageSource)GetValue(ClearButtonImageProperty);
            set => SetValue(ClearButtonImageProperty, value);
        }

        public static readonly BindableProperty IsClearButtonVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsClearButtonVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsClearButtonVisible
        {
            get => (bool)GetValue(IsClearButtonVisibleProperty);
            private set => SetValue(IsClearButtonVisibleProperty, value);
        }

        public static readonly BindableProperty RightButtonImageProperty =
            BindableProperty.Create(
                propertyName: nameof(RightButtonImage),
                returnType: typeof(ImageSource),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ImageSource RightButtonImage
        {
            get => (ImageSource)GetValue(RightButtonImageProperty);
            set => SetValue(RightButtonImageProperty, value);
        }

        public static readonly BindableProperty IsRightButtonVisibleProperty =
            BindableProperty.Create(
                propertyName: nameof(IsRightButtonVisible),
                returnType: typeof(bool),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsRightButtonVisible
        {
            get => (bool)GetValue(IsRightButtonVisibleProperty);
            private set => SetValue(IsRightButtonVisibleProperty, value);
        }

        public static readonly BindableProperty RightButtonClickCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(RightButtonClickCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(CustomEntryFrame),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand RightButtonClickCommand
        {
            get => (ICommand)GetValue(RightButtonClickCommandProperty);
            set => SetValue(RightButtonClickCommandProperty, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsSearchBarFocused))
            {
                EntryColumnSpan = IsSearchBarFocused ? 2 : 1;
                ChangeButtonsVisible();
            }
        }

        #endregion

        #region --- Private helpers ---

        private void ChangeButtonsVisible()
        {
            IsLeftButtonVisible = !IsLeftButtonVisible;
            IsBackButtonVisible = !IsBackButtonVisible;
            IsRightButtonVisible = !IsRightButtonVisible;
            IsClearButtonVisible = !IsClearButtonVisible;
        }

        private void CustomEntry_Focused(object sender, FocusEventArgs e)
        {
            IsSearchBarFocused = true;
            IsClearButtonVisible = true;
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            EntryText = string.Empty;
        }

        #endregion

    }
}