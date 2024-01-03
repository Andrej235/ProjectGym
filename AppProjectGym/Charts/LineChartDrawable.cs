using AppProjectGym.Utilities.Models;
using Color = Microsoft.Maui.Graphics.Color;
using PointF = Microsoft.Maui.Graphics.PointF;
using RectangleF = Microsoft.Maui.Graphics.RectF;

namespace AppProjectGym.Charts
{
    internal class LineChartDrawable : View, IDrawable
    {
        public float maxValue;
        public double xAxisScaleOrigin;
        private float lastPointXAxis;
        private float firstPointXAxis = 0.0f;

        public IEnumerable<ValuePoint> Points
        {
            get => points;
            set
            {
                points = value;
                randomizedRelativeTextPositions = points is null ? [] : points.Select(x => new Vector2(Random.Shared.Next(-15, 15), 0)).ToList();
                OnPropertyChanged();
            }
        }
        private IEnumerable<ValuePoint> points;
        private List<Vector2> randomizedRelativeTextPositions;

        public double XAxisScale
        {
            get => xAxisScale;
            set
            {
                xAxisScale = value;
                OnPropertyChanged();
            }
        }
        private double xAxisScale;

        public Color FillColor
        {
            get => fillColor;
            set
            {
                fillColor = value;
                OnPropertyChanged();
            }
        }
        private Color fillColor;

        public Color LineColor
        {
            get => lineColor;
            set
            {
                lineColor = value;
                OnPropertyChanged();
            }
        }
        private Color lineColor;

        public Color TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                OnPropertyChanged();
            }
        }
        private Color textColor;

        public float FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                OnPropertyChanged();
            }
        }
        private float fontSize;



        public LineChartDrawable() => VerticalOptions = LayoutOptions.Fill;

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.ResetState();
            canvas.StrokeColor = LineColor;
            canvas.StrokeSize = 3;
            canvas.FontColor = TextColor;
            canvas.FontSize = FontSize;

            //If the slider was moved then change x axis for the first bar
            if (XAxisScale != xAxisScaleOrigin)
                firstPointXAxis += (float)(XAxisScale - xAxisScaleOrigin) * -lastPointXAxis;

            var currentPointXAxis = firstPointXAxis;
            var mainLinePath = new PathF();

            //Generate path
            var numberOfPoints = Points.Count();
            for (var i = 0; i < numberOfPoints; i++)
            {
                var currentPoint = Points.ElementAt(i);
                var currentPointYAxis = dirtyRect.Height - (dirtyRect.Height * (currentPoint.Value / maxValue)) + 125;

                if (i == 0)
                    mainLinePath.MoveTo(new PointF(currentPointXAxis, currentPointYAxis));
                else
                    mainLinePath.LineTo(new PointF(currentPointXAxis, currentPointYAxis));



                //Draw text
                var offSet = randomizedRelativeTextPositions[i];
                if (i % 2 == 1)
                    offSet += new Vector2(0, -(250 - 125 * (currentPoint.Value / maxValue)));
                else
                    offSet += new Vector2(0, -100);

                var textPos = new PointF(currentPointXAxis, currentPointYAxis) + offSet;

                PathF textPath = new();
                textPath.MoveTo(new PointF(currentPointXAxis, currentPointYAxis - 5));
                textPath.LineTo(textPos);
                canvas.DrawPath(textPath);

                var pointText = $"{currentPoint.Name}: {currentPoint.Value:F2}";
                canvas.DrawString(pointText, textPos.X + 3, textPos.Y - 3, HorizontalAlignment.Center);

                //Remember last point x axis
                if (i == numberOfPoints - 1)
                    lastPointXAxis = currentPointXAxis;

                //Move x axis to next point
                const int POINT_SEGMENT_DISTANCE = 110;
                currentPointXAxis += POINT_SEGMENT_DISTANCE;
            }

            var linearGradientPaint = new LinearGradientPaint
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = [
                    (new PaintGradientStop(1f, FillColor)),
                    (new PaintGradientStop(.5f, new(FillColor.Red, FillColor.Green, FillColor.Blue, 1)))
                    ]
            };
            canvas.SetFillPaint(linearGradientPaint, new RectangleF(0, dirtyRect.Height - 100, lastPointXAxis, dirtyRect.Height - 100));

            //Connect bottom of the line chart to the top
            mainLinePath.LineTo(new PointF(lastPointXAxis, dirtyRect.Height));
            mainLinePath.LineTo(new PointF(firstPointXAxis, dirtyRect.Height));
            mainLinePath.LineTo(new PointF(firstPointXAxis, mainLinePath.FirstPoint.Y));

            canvas.DrawPath(mainLinePath);
            mainLinePath.Close();

            //Fill chart with gradient
            canvas.FillPath(mainLinePath);

            //Remember selected x axis
            xAxisScaleOrigin = XAxisScale;
        }
    }
}
