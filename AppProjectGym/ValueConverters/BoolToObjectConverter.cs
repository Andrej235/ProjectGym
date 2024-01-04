using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class BoolToObjectConverter : IValueConverter
    {
        public object TrueObject { get; set; }
        public object FalseObject { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool valueBool ? (valueBool ? TrueObject : FalseObject) : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
