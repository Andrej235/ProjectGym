namespace AppProjectGym.Charts;

public partial class PieChartView : ContentView
{
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(PieChartView),
            new Dictionary<string, float>(),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var chartView = ((PieChartView)bindable);
                chartView.Chart.PieChartDrawable.Points = (Dictionary<string, float>)newValue;
            });
    public Dictionary<string, float> Points
    {
        get => (Dictionary<string, float>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }
    public PieChartView() => InitializeComponent();
}