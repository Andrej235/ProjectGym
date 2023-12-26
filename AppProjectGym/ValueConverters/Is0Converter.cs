using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class Is0Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is not int valueInt ? null : valueInt == 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
