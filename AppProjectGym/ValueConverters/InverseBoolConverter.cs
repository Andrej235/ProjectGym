using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool valueBool ? !valueBool : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is bool valueBool ? !valueBool : null;
    }
}
