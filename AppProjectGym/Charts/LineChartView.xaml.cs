namespace AppProjectGym.Charts;

public partial class LineChartView : StackLayout
{
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(IEnumerable<ValuePoint>),
            typeof(LineChartView),
            new List<ValuePoint>(),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var chartView = ((LineChartView)bindable);
                //Give the heighest bar a little head room for aesthetics
                chartView.Chart.LineChartDrawable.Max = chartView.Points?.Select(x => x.Value).Max() * 1.1f ?? 0.0f;
                //Set the points from XAML to component
                chartView.Chart.LineChartDrawable.Points = (IEnumerable<ValuePoint>)newValue;
            });

    public IEnumerable<ValuePoint> Points
    {
        get => (IEnumerable<ValuePoint>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }
    public LineChartView()
    {
        InitializeComponent();
    }
}