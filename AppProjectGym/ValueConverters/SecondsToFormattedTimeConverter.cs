using AppProjectGym.Utilities;
using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class SecondsToFormattedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int time = System.Convert.ToInt32(value);
                return FormatTime(time);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return null;
            }
        }

        static string FormatTime(int seconds) => seconds switch
        {
            // Less than a minute, format as SS
            < 60 => $"{seconds:D2}",
            // Less than an hour, format as MM:SS
            < 3600 => $"{seconds / 60:D2}:{seconds % 60:D2}",
            // 1 hour or more, format as HH:MM:SS
            _ => $"{seconds / 3600:D2}:{seconds % 3600 / 60:D2}:{seconds % 60:D2}",
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
