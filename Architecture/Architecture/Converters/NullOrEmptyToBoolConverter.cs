using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Architecture
{
    public class NullOrEmptyToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(value?.ToString()))
            {
                return false;
            }
            else if (value is IList list)
            {
                return list?.Count > 0;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
