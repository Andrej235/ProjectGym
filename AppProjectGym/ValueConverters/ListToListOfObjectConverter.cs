using System.Collections;
using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class ListToListOfObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is IEnumerable collection ? collection.Cast<object>().ToList() : [];

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
