using System.Globalization;
using AppProjectGym.Models;
using AppProjectGym.Pages;

namespace AppProjectGym.ValueConverters
{
    public class IsSupersetNotNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is not null && value is SetDisplay setDisplay && setDisplay != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
