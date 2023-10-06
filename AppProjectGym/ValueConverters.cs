using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym
{
    public class ValueConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;
            Exercise exercise = MainPage.Exercises.FirstOrDefault(e => e.Id == id);
            ExerciseImage image = exercise.Images.FirstOrDefault(i => i.IsMain);

            return image.ImageURL;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
