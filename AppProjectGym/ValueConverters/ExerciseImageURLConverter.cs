using AppProjectGym.Models;
using System.Globalization;

namespace AppProjectGym.ValueConverters
{
    public class ExerciseImageURLConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;
            Exercise exercise = MainPage.Exercises.FirstOrDefault(e => e.Id == id);
            ExerciseImage image = exercise.Images.FirstOrDefault(i => i.IsMain);

            return image is null || image.ImageURL is null ? "" : image.ImageURL;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
