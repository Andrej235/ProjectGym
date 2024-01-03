namespace AppProjectGym.Charts
{
    internal class LineChartGraphicsView : GraphicsView
    {
        public static readonly BindableProperty XAxisScaleProperty = BindableProperty.Create(nameof(XAxisScale),
        typeof(double),
        typeof(LineChartGraphicsView),
        0.0,
        propertyChanged: (b, o, n) => {
            var graphicsView = ((LineChartGraphicsView)b);
            graphicsView.LineChartDrawable.XAxisScale = Convert.ToSingle(n);
            graphicsView.Invalidate();
        });

        public double XAxisScale
        {
            get => (double)GetValue(XAxisScaleProperty);
            set => SetValue(XAxisScaleProperty, value);
        }

        public LineChartGraphicsView() => Drawable = LineChartDrawable;
        public LineChartDrawable LineChartDrawable = new();
    }
}
