using Xamarin.Forms;

namespace GpsNotepad.Views
{
    public class BaseTabbedPage : TabbedPage
    {
        public static readonly BindableProperty SelectedTabFillColorProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectedTabFillColor),
                returnType: typeof(Color),
                declaringType: typeof(BaseTabbedPage),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color SelectedTabFillColor
        {
            get => (Color)GetValue(SelectedTabFillColorProperty);
            set => SetValue(SelectedTabFillColorProperty, value);
        }

    }
}
