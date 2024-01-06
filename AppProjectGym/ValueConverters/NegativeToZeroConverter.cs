using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class NegativeToZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int valueInt = System.Convert.ToInt32(value);
                return (valueInt < 0 ? 0 : valueInt);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
