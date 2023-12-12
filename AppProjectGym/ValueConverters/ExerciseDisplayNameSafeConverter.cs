using AppProjectGym.Models;
using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class ExerciseDisplayNameSafeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Exercise not found";

            if (value is ExerciseDisplay exerciseDisplay)
                return exerciseDisplay.Name ?? "Exercise not found";

            if (value is string exerciseName)
                return exerciseName ?? "Exercise not found";

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
