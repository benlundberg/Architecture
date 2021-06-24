using System;
using System.Globalization;
using Xamarin.Forms;

namespace Architecture
{
    public class BoolToPrimaryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) == true ? App.Current.PrimaryColor() : App.Current.Get<Color>("GrayDark");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) == true ? App.Current.Get<Color>("GrayDark") : App.Current.PrimaryColor();
        }
    }
}
