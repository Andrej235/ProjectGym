using System.Globalization;
using AppProjectGym.Models;
using AppProjectGym.Pages;

namespace AppProjectGym.ValueConverters
{
    public class IsSupersetNotNull : IValueConverter
    {
        //public static object Superset { get; set; }
        public static WorkoutSetDisplay WorkoutSet { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is not null && value is SetDisplay setDisplay && setDisplay != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isEnabled)
                return null;

            var a = WorkoutEditPage.WorkoutSetDisplays.FirstOrDefault(x => x.Id == WorkoutSet?.Id);
            return isEnabled ? a?.Superset : null;
        }
    }
}
