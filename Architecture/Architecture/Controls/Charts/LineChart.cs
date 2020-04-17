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

            if (ChartEntries.Any(x => x.IsVisible))
            {
                CalculateChartValuesXPoints(chart);

                DrawSlider(canvas, frame, chart);

                DrawHorizontalLabels(canvas, frame, chart);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    DrawLines(entry, canvas, CalculatePoints(entry.Items, frame, chart));
                }

                // Get items on x axis
                var valueItems = ChartEntries.GetChartValueItemFromX(chart.GetInsideXValue(TouchedPoint.X), frame, frame.GetItemWidth(MaxItems), UseExactValue);

                // Send selected items with command
                SelectedValuesCommand?.Execute(new SelectedChartValueItemArgs
                {
                    ChartValueItems = valueItems,
                    TouchedPoint = new SKPoint(chart.GetInsideXValue(TouchedPoint.X), TouchedPoint.Y)
                });

                DrawHorizontalLabel(valueItems?.FirstOrDefault()?.ChartValueItem, canvas, frame, chart);

                DrawSliderPoints(valueItems, canvas, chart);
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
                IsStroke = true,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = chartItem.Color.ToSKColor(),
                StrokeWidth = chartItem.LineWidth
            })
            {
                if (chartItem.UseDashedEffect)
                {
                    // Draw solid transparent path
                    paint.Color = chartItem.Color.ToSKColor().AsTransparency();
                    DrawPath(canvas, paint, points);

                    // Draw dashed path
                    paint.Color = chartItem.Color.ToSKColor();
                    paint.PathEffect = SKPathEffect.CreateDash(new float[] { StrokeDashFirst, StrokeDashSecond }, StrokeDashPhase);
                    DrawPath(canvas, paint, points);
                }
                else
                {
                    DrawPath(canvas, paint, points);
                }
            }
        }

        private void DrawPath(SKCanvas canvas, SKPaint paint, SKPoint[] points)
        {
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

        private void DrawSlider(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            float x = chart.GetInsideXValue(TouchedPoint.X);

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderColor.ToSKColor(),
                StrokeWidth = this.SliderWidth
            })
            {
                // Straight slider line
                canvas.DrawLine(x, chart.Top, x, DisplayHorizontalValuesBySlider ? frame.Bottom + ChartRectMargin.Bottom : chart.Bottom - FrameWidth, paint);

                DrawSliderHint(canvas, x);
            }

            // Get items on x axis
            var valueItems = ChartEntries.GetChartValueItemFromX(x, frame, frame.GetItemWidth(MaxItems), UseExactValue);

            // Send selected items with command
            SelectedValuesCommand?.Execute(new SelectedChartValueItemArgs
            {
                ChartValueItems = valueItems,
                TouchedPoint = new SKPoint(x, TouchedPoint.Y)
            });
        }

        private void DrawSliderHint(SKCanvas canvas, float x)
        {
            if (!UseSliderHint)
            {
                return;
            }

            var y = HintSize;

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderColor.ToSKColor(),

            })
            {
                canvas.DrawCircle(x, y, y, paint);
            }

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderDetailTextColor.ToSKColor(),
                StrokeWidth = this.SliderWidth
            })
            {

                var left = x;
                var right = x;

                // Left hint 
                canvas.DrawLine(left - (HintSize * 0.6f), y, left - (HintSize * 0.2f), y - (HintSize * 0.5f), paint);
                canvas.DrawLine(left - (HintSize * 0.6f), y, left - (HintSize * 0.2f), y + (HintSize * 0.5f), paint);
                canvas.DrawLine(left - (HintSize * 0.2f), y - (HintSize * 0.5f), left - (HintSize * 0.2f), y + (HintSize * 0.5f), paint);

                // Right hint
                canvas.DrawLine(right + (HintSize * 0.6f), y, right + (HintSize * 0.2f), y - (HintSize * 0.5f), paint);
                canvas.DrawLine(right + (HintSize * 0.6f), y, right + (HintSize * 0.2f), y + (HintSize * 0.5f), paint);
                canvas.DrawLine(right + (HintSize * 0.2f), y - (HintSize * 0.5f), right + (HintSize * 0.2f), y + (HintSize * 0.5f), paint);
            }
        }

        public LineMode LineMode { get; set; } = LineMode.Straight;
    }
}
