using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public enum LineMode
    {
        Straight,
        Spline
    }

    public class LineChart : Chart
    {
        public LineChart()
        {
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

            touchedPoint = e.Location;
            lastActionType = e.ActionType;

            switch (lastActionType)
            {
                case SKTouchAction.Entered:
                case SKTouchAction.Pressed:
                case SKTouchAction.Moved:
                    isHolding = true;
                    break;
                case SKTouchAction.Released:
                case SKTouchAction.Exited:
                case SKTouchAction.Cancelled:
                    isHolding = false;
                    break;
                default:
                    isHolding = false;
                    break;
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

            // Calculate size of vertical/left labels
            var verticalLabelSize = MeasureVerticalLabels();

            // Calculate left width where vertical labels will be
            var leftPadding = CalculateLeftWidth(verticalLabelSize);

            // Calculate footer height where horizontal labels will be
            var footerHeight = CalculateFooterHeight();

            // Create a rectangle of the frame
            var frameRect = new SKRect(
                left: leftPadding,
                top: ChartMargin.Top,
                right: info.Width - ChartMargin.Right,
                bottom: info.Height - footerHeight);

            var chartRect = new SKRect(
                left: leftPadding + ChartPadding.Left,
                top: ChartMargin.Top + ChartPadding.Top,
                right: info.Width - ChartMargin.Right - ChartPadding.Right,
                bottom: info.Height - footerHeight - ChartMargin.Bottom);

            // Draw the frame on the canvas
            DrawFrame(canvas, frameRect);

            foreach (var entries in ChartEntries)
            {
                var points = CalculatePoints(entries.Items, frameRect, chartRect);

                DrawLines(entries, canvas, points, frameRect.Width);
            }

            DrawHorizontalLabels(canvas, ChartEntries.FirstOrDefault().Items.Select(x => x.Point).ToArray(), frameRect, info.Height, footerHeight);

            DrawVerticalLabels(canvas, frameRect, chartRect);

            DrawSlider(canvas, frameRect);

            GetValueFromPosition(canvas, frameRect);
        }

        private void DrawSlider(SKCanvas canvas, SKRect frame)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderColor,
                StrokeWidth = this.SliderWidth
            })
            {
                // Straight slider line
                canvas.DrawLine(touchedPoint.X, frame.Top + VerticalLabelTextSize, touchedPoint.X, frame.Bottom, paint);

                paint.StrokeWidth = 4f;
                paint.Color = SKColors.Black;

                float y;

                if (isHolding)
                {
                    if (touchedPoint.Y <= frame.Top + 20)
                    {
                        y = frame.Top + 20;
                    }
                    else if (touchedPoint.Y >= frame.Bottom - 20)
                    {
                        y = frame.Bottom - 20;
                    }
                    else
                    {
                        y = touchedPoint.Y;
                    }
                }
                else
                {
                    y = frame.MidY;
                }

                // Left hint
                canvas.DrawLine(touchedPoint.X - 20, y, touchedPoint.X - 10, y - 10, paint);
                canvas.DrawLine(touchedPoint.X - 20, y, touchedPoint.X - 10, y + 10, paint);

                // Right hint
                canvas.DrawLine(touchedPoint.X + 20, y, touchedPoint.X + 10, y - 10, paint);
                canvas.DrawLine(touchedPoint.X + 20, y, touchedPoint.X + 10, y + 10, paint);
            }
        }

        private void DrawFrame(SKCanvas canvas, SKRect rect)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = this.FrameColor,
                StrokeWidth = this.FrameWidth,
                IsAntialias = true
            })
            {
                // Left to bottom
                canvas.DrawLine(rect.Left + (paint.StrokeWidth / 2), rect.Top, rect.Left + (paint.StrokeWidth / 2), rect.Bottom, paint);

                // Left to right
                canvas.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom, paint);
            }
        }

        private void DrawVerticalLabels(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            float bottom = frame.Bottom;
            float top = frame.Top;
            float middle = (frame.Height * 0.5f) + ChartMargin.Top;
            float middleTop = (frame.Height * 0.75f) + ChartMargin.Top;
            float middleBottom = (frame.Height * 0.25f) + ChartMargin.Top;

            using (var paint = new SKPaint())
            {
                paint.TextSize = VerticalLabelTextSize;
                paint.Color = this.VerticalLabelColor;
                paint.IsStroke = false;
                paint.IsAntialias = true;

                var height = chart.Height;

                var valueY = MaxValue / height;

                var maximumValue = chart.Height * valueY;

                canvas.DrawText(maximumValue.ToString(), ChartMargin.Left, top + VerticalLabelTextSize, paint);

                canvas.DrawText(this.VerticalUnit, ChartMargin.Left, top, paint);

                canvas.DrawText((maximumValue / 2).ToString(), ChartMargin.Left, middle + (VerticalLabelTextSize / 4), paint);

                canvas.DrawText("0", ChartMargin.Left, bottom, paint);
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Gray,
                StrokeWidth = 2,
            })
            {
                canvas.DrawLine(chart.Left, middle, chart.Right, middle, paint);
            }
        }

        private void DrawHorizontalLabels(SKCanvas canvas, SKPoint[] points, SKRect frame, int height, float footerHeight)
        {
            if (HideHorizontalLabels)
            {
                var leftLabel = ChartEntries.FirstOrDefault().Items.FirstOrDefault();
                var rightLabel = ChartEntries.FirstOrDefault().Items.LastOrDefault();

                if (string.IsNullOrEmpty(leftLabel?.Label) && string.IsNullOrEmpty(rightLabel?.Label))
                {
                    return;
                }

                using (var paint = new SKPaint())
                {
                    paint.TextSize = this.HorizontalLabelTextSize;
                    paint.Color = this.HorizontalLabelColor;
                    paint.FakeBoldText = true;
                    paint.IsStroke = false;
                    paint.IsAntialias = false;

                    var bounds = new SKRect();
                    var text = leftLabel.Label;
                    paint.MeasureText(text, ref bounds);

                    if (bounds.Width > frame.Width)
                    {
                        text = text.Substring(0, Math.Min(3, text.Length));
                        paint.MeasureText(text, ref bounds);
                    }

                    if (bounds.Width > frame.Width)
                    {
                        text = text.Substring(0, Math.Min(1, text.Length));
                        paint.MeasureText(text, ref bounds);
                    }

                    if (lastActionType == SKTouchAction.Pressed)
                    {
                        SKRect leftClickRect = new SKRect(
                            leftLabel.Point.X - bounds.Width, 
                            (height - (footerHeight / 2)) - bounds.Height, 
                            leftLabel.Point.X + bounds.Width, 
                            (height - (footerHeight / 2)) + bounds.Height);

                        if (leftClickRect.Contains(touchedPoint))
                        {
                            Application.Current.MainPage.DisplayAlert($"You clicked {leftLabel.Label}", "Hello", "Ok");
                        }

                        SKRect rightClickRect = new SKRect(
                            rightLabel.Point.X - bounds.Width,
                            (height - (footerHeight / 2)) - bounds.Height,
                            rightLabel.Point.X + bounds.Width,
                            (height - (footerHeight / 2)) + bounds.Height);

                        if (rightClickRect.Contains(touchedPoint))
                        {
                            Application.Current.MainPage.DisplayAlert($"You clicked {rightLabel.Label}", "Hello", "Ok");
                        }
                    }

                    canvas.DrawText(text, leftLabel.Point.X, height - (footerHeight / 2), paint);
                    canvas.DrawText(rightLabel.Label, rightLabel.Point.X - bounds.Width, height - (footerHeight / 2), paint);
                }

                var item = GetChartEntriesFromXPoint(touchedPoint.X)?.FirstOrDefault();

                if (item != null)
                {
                    var xLabel = item.Item1.Label;
                    var nextXLabel = item.Item2.Label;

                    if (xLabel == rightLabel.Label || nextXLabel == rightLabel.Label || xLabel == leftLabel.Label)
                    {
                        return;
                    }

                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = this.HorizontalLabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = this.HorizontalLabelColor;
                        paint.IsStroke = false;

                        var bounds = new SKRect();
                        var text = xLabel;
                        paint.MeasureText(text, ref bounds);

                        if (bounds.Width > frame.Width)
                        {
                            text = text.Substring(0, Math.Min(3, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        if (bounds.Width > frame.Width)
                        {
                            text = text.Substring(0, Math.Min(1, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        canvas.DrawText(text, touchedPoint.X - (bounds.Width / 2), height - (footerHeight / 2), paint);
                    }
                }
            }
            else
            {
                var chartEntry = ChartEntries.FirstOrDefault(x => x.Items.Any());

                for (int i = 0; i < chartEntry.Items.Count(); i++)
                {
                    var entry = chartEntry.Items.ElementAt(i);
                    var point = points[i];

                    if (string.IsNullOrEmpty(entry?.Label))
                    {
                        continue;
                    }

                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = this.HorizontalLabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = this.HorizontalLabelColor;
                        paint.IsStroke = false;

                        var bounds = new SKRect();
                        var text = entry.Label;
                        paint.MeasureText(text, ref bounds);

                        if (bounds.Width > frame.Width)
                        {
                            text = text.Substring(0, Math.Min(3, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        if (bounds.Width > frame.Width)
                        {
                            text = text.Substring(0, Math.Min(1, text.Length));
                            paint.MeasureText(text, ref bounds);
                        }

                        canvas.DrawText(text, point.X - (bounds.Width / 2), height - (footerHeight / 2), paint);
                    }
                }
            }
        }

        private void DrawLines(ChartEntry chartEntry, SKCanvas canvas, SKPoint[] points, float width)
        {
            if (points?.Any() != true)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = chartEntry.Color,
                StrokeWidth = chartEntry.LineWidth,
                IsAntialias = true,
            })
            {
                var path = new SKPath();

                path.MoveTo(points.First());

                var last = LineMode == LineMode.Straight ? points.Length : points.Length - 1;

                for (int i = 0; i < last; i++)
                {
                    if (LineMode == LineMode.Spline)
                    {
                        var total = chartEntry.Items.Count;
                        var w = (width - ((total + 1) * this.ChartMargin.Left)) / total;

                        var (control, nextPoint, nextControl) = this.CalculateCubicInfo(points, i, w);
                        path.CubicTo(control, nextControl, nextPoint);
                    }
                    else
                    {
                        path.LineTo(points[i]);
                    }
                }

                // Saves path on chart entry
                chartEntry.Path = path;

                canvas.DrawPath(path, paint);
            }
        }

        private (SKPoint control, SKPoint nextPoint, SKPoint nextControl) CalculateCubicInfo(SKPoint[] points, int i, float width)
        {
            var point = points[i];
            var nextPoint = points[i + 1];
            var controlOffset = new SKPoint(width * 0.8f, 0);
            var currentControl = point + controlOffset;
            var nextControl = nextPoint - controlOffset;
            return (currentControl, nextPoint, nextControl);
        }

        private void DrawPoints(SKCanvas canvas, SKPoint[] points)
        {
            if (points?.Any() != true)
            {
                return;
            }

            canvas.DrawPoints(SKPointMode.Points, points, new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 25,
            });
        }

        private void GetValueFromPosition(SKCanvas canvas, SKRect frame)
        {
            var chartEntryItems = this.GetChartEntriesFromXPoint(touchedPoint.X);

            if (chartEntryItems?.Any() != true)
            {
                return;
            }

            int index = -1;

            foreach (var tuple in chartEntryItems)
            {
                index++;

                var chartEntry = tuple.Item1;

                var nextChartEntry = tuple.Item2;

                // Difference in value
                var diffVal = nextChartEntry.Value - chartEntry.Value;

                // Difference in Y position
                var diffY = nextChartEntry.Point.Y - chartEntry.Point.Y;

                // Difference in X position
                var diffX = nextChartEntry.Point.X - chartEntry.Point.X;

                var yValue = diffVal / diffY;
                var xValue = diffVal / diffX;

                // Current X position over first item
                var currentXPosition = touchedPoint.X - chartEntry.Point.X;
                var points = currentXPosition * xValue;

                var y = (points / yValue) + chartEntry.Point.Y;

                using (var circlePaint = new SKPaint
                {
                    Color = tuple.Item3,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 4f
                })
                {
                    canvas.DrawCircle(new SKPoint(touchedPoint.X, y), 4, circlePaint);
                }

                using (var textPaint = new SKPaint())
                {
                    textPaint.TextSize = HorizontalLabelTextSize;
                    textPaint.IsAntialias = true;
                    textPaint.Color = SKColors.White;
                    textPaint.IsStroke = false;

                    var currentVal = chartEntry.Value + (int)Math.Round(decimal.Parse(points.ToString()), 0, MidpointRounding.AwayFromZero);

                    if (touchedPoint.X == chartEntry.Point.X)
                    {
                        currentVal = chartEntry.Value;
                    }
                    else if (touchedPoint.X == nextChartEntry.Point.X)
                    {
                        currentVal = nextChartEntry.Value;
                    }

                    string text = currentVal.ToString() + " " + this.VerticalUnit;

                    SKRect textBounds = new SKRect();
                    textPaint.MeasureText(MaxValue.ToString() + " " + this.VerticalUnit, ref textBounds);

                    var rectWidth = textBounds.Width + 10;
                    var rectHeight = textBounds.Height + 10;

                    float xPosition, yPosition;

                    if (SliderDetailOrientation == StackOrientation.Vertical)
                    {
                        xPosition = touchedPoint.X - (rectWidth / 2);
                        yPosition = (frame.Top - (rectHeight / 2)) - (index * rectHeight) - (index * 5);
                    }
                    else
                    {
                        var count = chartEntryItems.Count;

                        var totalDetailWidth = (rectWidth * count) + (5 * count);

                        if ((totalDetailWidth / 2) >= touchedPoint.X - frame.Left)
                        {
                            xPosition = touchedPoint.X + (index * rectWidth) + index * 5;
                        }
                        else if ((totalDetailWidth / 2) >= frame.Right - touchedPoint.X)
                        {
                            xPosition = touchedPoint.X - (totalDetailWidth - 5) + (((index * rectWidth) + index * 5));
                        }
                        else
                        {
                            var totalMiddle = totalDetailWidth / 2;

                            xPosition = touchedPoint.X - (totalMiddle - (index * rectWidth)) + index * 5;
                        }

                        yPosition = frame.Top - (rectHeight / 2);
                    }

                    using (var rectPaint = new SKPaint
                    {
                        Style = SKPaintStyle.StrokeAndFill,
                        StrokeWidth = 2f,
                        StrokeCap = SKStrokeCap.Round,
                        Color = tuple.Item3,
                    })
                    {
                        SKRect rect = new SKRect(
                            left: xPosition,
                            top: yPosition,
                            right: xPosition + rectWidth,
                            bottom: yPosition + rectHeight);

                        canvas.DrawRect(rect, rectPaint);

                        canvas.DrawText(text, rect.MidX - (textBounds.Width / 2), rect.MidY + (textBounds.Height / 2), textPaint);
                    }
                }
            }
        }

        public bool HideHorizontalLabels { get; set; } = true;
        public LineMode LineMode { get; set; } = LineMode.Straight;
        public StackOrientation SliderDetailOrientation { get; set; } = StackOrientation.Horizontal;

        private SKTouchAction lastActionType;
        private SKPoint touchedPoint = new SKPoint(0, 0);
        private bool isHolding;
    }
}
