using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Charts
{
    internal class PieChartGraphicsView : GraphicsView
    {
        public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(PieChartGraphicsView),
            new Dictionary<string, float>(),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var chartView = ((PieChartGraphicsView)bindable);

                chartView.PieChartDrawable.Points = (Dictionary<string, float>)newValue;
            });

        public Dictionary<string, float> Points
        {
            get => (Dictionary<string, float>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public PieChartGraphicsView() => Drawable = PieChartDrawable;

        public PieChartDrawable PieChartDrawable = new PieChartDrawable();
    }
}
