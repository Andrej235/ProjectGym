using System.Collections;
using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class IsCollectionNotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is IEnumerable collection ? collection.Cast<object>().Any() : (object)false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
