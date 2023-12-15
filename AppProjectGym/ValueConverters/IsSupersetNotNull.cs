using System.Globalization;
using AppProjectGym.Models;

namespace AppProjectGym.ValueConverters
{
    public class IsSupersetNotNull : IValueConverter
    {
        public static object Superset { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is not null && value is SetDisplay setDisplay && setDisplay != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isEnabled)
                return null;

            return isEnabled ? Superset : null;
        }
    }
}
