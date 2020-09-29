using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

            isTouching = true;

            if (e.ActionType == SKTouchAction.Released || e.ActionType == SKTouchAction.Cancelled || e.ActionType == SKTouchAction.Exited)
            {
                isTouching = false;

                var closest = float.MaxValue;

                if (ChartEntries == null)
                {
                    return;
                }

                foreach (var item in ChartEntries.SelectMany(x => x.Items))
                {
                    var distance = SKPoint.Distance(new SKPoint(e.Location.X, 0), new SKPoint(item.Point.X, 0));

                    if (distance < closest)
                    {
                        closest = distance;
                        TouchedPoint = item.Point;
                    }
                }
            }
            else
            {
                TouchedPoint = e.Location;
            }

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

            DrawVerticalLabels(canvas, frame, chart);

            if (ChartEntries.Any(x => x.IsVisible))
            {
                CalculateChartValuesXPoints(chart);

                DrawInnerFrame(canvas, frame);
                DrawBackground(canvas, frame);

                if (!string.IsNullOrEmpty(SelectedTag) && !isTouching)
                {
                    var selectedTagPosition = ChartValueItemsXPoints.FirstOrDefault(x => x.Item1.ToString() == SelectedTag)?.Item2 ?? 0f;

                    TouchedPoint = new SKPoint(selectedTagPosition, 0f);
                }

                DrawSlider(canvas, frame, chart);

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    DrawLines(entry, canvas, CalculatePoints(entry.Items, frame, chart));
                }

                DrawHorizontalLabels(canvas, frame, chart);

                // Get items on x axis
                var valueItems = ChartEntries.GetChartValueItemFromX(chart.GetInsideXValue(TouchedPoint.X), frame, frame.GetItemWidth(MaxItems));

                // Send selected items with command
                SelectedValuesCommand?.Execute(new SelectedChartValueItemArgs
                {
                    ChartValueItems = valueItems,
                    TouchedPoint = new SKPoint(chart.GetInsideXValue(TouchedPoint.X), TouchedPoint.Y)
                });

                DrawHorizontalLabel(valueItems?.FirstOrDefault()?.ChartValueItem, canvas, frame, chart);

                DrawFrame(canvas, frame);
            
                DrawSliderPoints(valueItems, canvas, chart);
            }
            else
            {
                DrawInnerFrame(canvas, frame);
                DrawFrame(canvas, frame);
            }
        }

        private void DrawBackground(SKCanvas canvas, SKRect frame)
        {
            if (!HasBackground)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                Color = ChartBackgroundColor.ToSKColor(),
                Style = SKPaintStyle.Fill
            })
            {
                var items = ChartValueItemsXPoints;

                var width = frame.GetItemWidth(MaxItems);

                for (int i = 0; i < items.Count; i++)
                {
                    if (i % 2 != 0)
                    {
                        continue;
                    }

                    var item = items[i];

                    var left = item.Item2;

                    // Don't draw outside frame
                    if ((left + (width * 2)).ToRounded() <= (frame.Right + FrameWidth).ToRounded())
                    {
                        canvas.DrawRect(left + width, frame.Top, width, frame.Height - FrameWidth, paint);
                    }
                }
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
                    var offsetPoint = new SKPoint((nextPoint.X - point.X) * 0.5f, 0);

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
                canvas.DrawLine(x, chart.Top, x, DisplayHorizontalValuesBySlider ? frame.Bottom + HorizontalTextSize : chart.Bottom - FrameWidth, paint);

                DrawSliderHint(canvas, x);
            }

            // Get items on x axis
            var valueItems = ChartEntries.GetChartValueItemFromX(x, frame, frame.GetItemWidth(MaxItems));

            // Send selected items with command
            SelectedValuesCommand?.Execute(new SelectedChartValueItemArgs
            {
                ChartValueItems = valueItems,
                TouchedPoint = new SKPoint(x, TouchedPoint.Y)
            });
        }

        private void DrawSliderHint(SKCanvas canvas, float x)
        {
            //if (!UseSliderHint)
            //{
            //    return;
            //}

            //var height = 88f * (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            //var width = 42f * (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;

            //if (svgDrop == null)
            //{
            //    string resourceID = "SkekraftKundapp.Media.sliderdrop.svg";

            //    Assembly assembly = GetType().GetTypeInfo().Assembly;

            //    using Stream stream = assembly.GetManifestResourceStream(resourceID);

            //    svgDrop = new SkiaSharp.Extended.Svg.SKSvg(new SKSize(width, height));

            //    svgDrop.Load(stream);
            //}

            //canvas.DrawPicture(svgDrop.Picture, x - (svgDrop.CanvasSize.Width / 2), 0);
        }

        private void DrawSliderPoints(IList<ChartValueItemParam> valueItems, SKCanvas canvas, SKRect chart)
        {
            if (valueItems?.Any() != true)
            {
                return;
            }

            float x = chart.GetInsideXValue(TouchedPoint.X);

            // Draws circle on y axis //

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = SliderPointSize
            })
            {
                foreach (var item in valueItems)
                {
                    paint.Color = item.BackgroundColor.ToSKColor();

                    if (LineMode == LineMode.Straight)
                    {
                        var y = ChartCalculator.CalculateYPositionForStraight(item.ChartValueItem, item.NextChartValueItem, x);

                        canvas.DrawCircle(x, y, SliderPointSize, paint);

                        paint.Color = SKColors.White;

                        canvas.DrawCircle(x, y, SliderPointSize / 4, paint);
                    }
                    else if (LineMode == LineMode.Spline)
                    {
                        var point = ChartCalculator.CalculateYPositionForSpline(item.ChartValueItem, item.NextChartValueItem, x);

                        canvas.DrawCircle(point.X, point.Y, SliderPointSize, paint);

                        paint.Color = SKColors.White;

                        canvas.DrawCircle(point.X, point.Y, SliderPointSize / 4, paint);
                    }
                }
            }
        }

        public LineMode LineMode { get; set; } = LineMode.Spline;

        public float SnapSensitivity { get; set; } = 15f;

        private bool isTouching;
    }
}
