using Color = Microsoft.Maui.Graphics.Color;
using PointF = Microsoft.Maui.Graphics.PointF;
using RectangleF = Microsoft.Maui.Graphics.RectF;

namespace AppProjectGym.Charts
{
    internal class LineChartDrawable : View, IDrawable
    {
        public IEnumerable<ValuePoint> Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }
        public double XAxisScale
        {
            get => _xAxisScale;
            set
            {
                _xAxisScale = value;
                OnPropertyChanged();
            }
        }
        public LineChartDrawable() => VerticalOptions = LayoutOptions.Fill;
        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.ResetState();
            Color accentColor = Color.FromArgb("#7F2CF6");

            canvas.StrokeColor = accentColor;
            canvas.FontColor = accentColor;
            canvas.FontSize = 16;

            //If the slider was moved then change x axis for the first bar
            if (XAxisScale != XAxisScaleOrigin)
                _firstPointXAxis += (float)(XAxisScale - XAxisScaleOrigin) * -_lastPointXAxis;

            var pointXAxis = _firstPointXAxis;
            var linearPath = new PathF();

            //Generate path
            var numberOfPoints = Points.Count();
            for (var i = 0; i < numberOfPoints; i++)
            {
                var currentPoint = Points.ElementAt(i);
                var yAxis = dirtyRect.Height - (dirtyRect.Height * (currentPoint.Value / Max));

                if (i == 0)
                    linearPath.MoveTo(new PointF(pointXAxis, yAxis));
                else
                    linearPath.LineTo(new PointF(pointXAxis, yAxis));



                //Draw text
                //TODO where did MeasureText go?
                var pointText = $"{currentPoint.Name}: {currentPoint.Value:F2}";
                canvas.DrawString(pointText,
                    pointXAxis + 50,
                    yAxis - 50,
                    HorizontalAlignment.Right);

                //Remember last point x axis
                if (i == numberOfPoints - 1)
                    _lastPointXAxis = pointXAxis;

                //Move x axis to next point
                const int POINT_SEGMENT_DISTANCE = 90;
                pointXAxis += POINT_SEGMENT_DISTANCE;
            }

            var linearGradientPaint = new LinearGradientPaint
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = [
                    (new PaintGradientStop(1f, accentColor)),
                    (new PaintGradientStop(.5f, new(accentColor.Red, accentColor.Green, accentColor.Blue, 0.25f)))
                    ]
            };
            canvas.SetFillPaint(linearGradientPaint, new RectangleF(0, dirtyRect.Height - 100, _lastPointXAxis, dirtyRect.Height - 100));

            //Connect bottom of the line chart to the top
            linearPath.LineTo(new PointF(_lastPointXAxis, dirtyRect.Height));
            linearPath.LineTo(new PointF(_firstPointXAxis, dirtyRect.Height));
            linearPath.LineTo(new PointF(_firstPointXAxis, linearPath.FirstPoint.Y));

            canvas.DrawPath(linearPath);
            linearPath.Close();

            //Fill chart with gradient
            canvas.FillPath(linearPath);

            //Remember selected x axis
            XAxisScaleOrigin = XAxisScale;
        }

        public float Max;
        public double XAxisScaleOrigin;
        private double _xAxisScale;
        private float _lastPointXAxis;
        private float _firstPointXAxis = 0.0f;
        private IEnumerable<ValuePoint> _points;
    }
}
