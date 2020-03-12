using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;

namespace Architecture.Controls.Charts
{
    public enum LineMode
    {
        Straight,
        Spline
    }

    public class LineChart : BaseChart
    {
        public LineChart()
        {
            ChartType = ChartType.Linear;

            EnableTouchEvents = true;

            this.PaintSurface += LineChart_PaintSurface;
            this.Touch += LineChart_Touch;
        }

        private void LineChart_Touch(object sender, SKTouchEventArgs e)
        {
            if (!(sender is SKCanvasView view))
            {
                return;
            }

            TouchedPoint = e.Location;

            e.Handled = true;

            view.InvalidateSurface();
        }

        private void LineChart_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (ChartEntries?.Any() != true)
            {
                return;
            }

            var info = e.Info;
            var canvas = e.Surface.Canvas;
            
            canvas.Clear();

            var frame = CreateFrame(info);
            var chart = CreateChart(frame);

            DrawFrame(canvas, frame);
            DrawVerticalLabels(canvas, frame, chart);

            if (ChartEntries.Any(x =>  x.IsVisible))
            {
                CalculateChartValuesXPoints(chart);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    DrawLines(entry, canvas, CalculatePoints(entry.Items, frame, chart));
                }
            }
            
            DrawHorizontalLabels(canvas, frame, chart);
            DrawSlider(canvas, chart);

            if (UseExactValue)
            {
                ShowExactSliderValuesPosition(canvas, chart);
            }
            else
            {
                ShowSliderValuesPosition(canvas, chart);
            }
        }

        private void DrawLines(ChartItem chartItem, SKCanvas canvas, SKPoint[] points)
        {
            if (points?.Any() != true)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                IsStroke = true,
                Color = chartItem.Color.ToSKColor(),
                StrokeWidth = chartItem.LineWidth,
                IsAntialias = true
            })
            {
                if (chartItem.UseDashedEffect)
                {
                    paint.PathEffect = SKPathEffect.CreateDash(new float[] { 12, 12 }, 0);
                }

                var path = new SKPath();

                path.MoveTo(points.First());

                var last = LineMode == LineMode.Straight ? points.Length : points.Length - 1;

                for (int i = 0; i < last; i++)
                {
                    if (LineMode == LineMode.Spline)
                    {
                        var point = points[i];
                        var nextPoint = points[i + 1];
                        var offsetPoint = new SKPoint((nextPoint.X - point.X) * 0.8f, 0);

                        var currentPoint = point + offsetPoint;
                        var next = nextPoint - offsetPoint;

                        path.CubicTo(currentPoint, next, nextPoint);
                    }
                    else
                    {
                        path.LineTo(points[i]);
                    }
                }

                canvas.DrawPath(path, paint);
            }
        }

        public LineMode LineMode { get; set; } = LineMode.Straight;
    }
}
