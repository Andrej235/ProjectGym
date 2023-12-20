using System.Collections;
using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class IsNullOrEmptyToObject<T> : IValueConverter
    {
        public T NotNullValue { get; set; }
        public T NullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null)
                return NullValue;

            if (value is string s)
                return s == "" ? NullValue : NotNullValue;

            if (value is IEnumerable collection)
                return collection.Cast<object>().Any() ? NullValue : NotNullValue;

            return NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
