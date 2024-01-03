namespace AppProjectGym.Charts;

public partial class LineChartView : StackLayout
{
    public IEnumerable<ValuePoint> Points
    {
        get => (IEnumerable<ValuePoint>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(IEnumerable<ValuePoint>),
            typeof(LineChartView),
            new List<ValuePoint>(),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var chartView = ((LineChartView)bindable);
                chartView.Chart.LineChartDrawable.maxValue = chartView.Points?.Select(x => x.Value).Max() ?? 0.0f;
                chartView.Chart.LineChartDrawable.Points = (IEnumerable<ValuePoint>)newValue;
            });



    public GradientBrush FillColor
    {
        get => (GradientBrush)GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }
    public static readonly BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor),
        typeof(GradientBrush),
        typeof(LineChartView),
        new LinearGradientBrush(),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var chartView = ((LineChartView)bindable);
            chartView.Chart.LineChartDrawable.FillColor = chartView.FillColor;
        });



    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }
    public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor),
        typeof(Color),
        typeof(LineChartView),
        new Color(),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var chartView = ((LineChartView)bindable);
            chartView.Chart.LineChartDrawable.LineColor = chartView.LineColor;
        });



    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
        typeof(Color),
        typeof(LineChartView),
        new Color(),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var chartView = ((LineChartView)bindable);
            chartView.Chart.LineChartDrawable.TextColor = chartView.TextColor;
        });



    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }
    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize),
        typeof(float),
        typeof(LineChartView),
        0f,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var chartView = ((LineChartView)bindable);
            chartView.Chart.LineChartDrawable.FontSize = chartView.FontSize;
        });



    public LineChartView() => InitializeComponent();
}