using Architecture.Core;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class Chart : SKCanvasView
    {
        protected void DrawFrame(SKCanvas canvas, SKRect rect)
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

        protected void DrawVerticalLabels(SKCanvas canvas, SKRect frame, SKRect chart)
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

                if (!string.IsNullOrEmpty(VerticalUnit))
                {
                    canvas.DrawText(this.VerticalUnit, ChartMargin.Left, top, paint);
                }

                canvas.DrawText((maximumValue / 2).ToString(), ChartMargin.Left, middle + (VerticalLabelTextSize / 4), paint);

                canvas.DrawText("0", ChartMargin.Left, bottom, paint);
            }

            //using (var paint = new SKPaint
            //{
            //    Style = SKPaintStyle.Stroke,
            //    Color = SKColors.Gray,
            //    StrokeWidth = 2,
            //})
            //{
            //    canvas.DrawLine(chart.Left, middle, chart.Right, middle, paint);
            //}
        }

        protected void DrawHorizontalLabels(SKCanvas canvas, SKRect frame, float y)
        {
            if (HideHorizontalLabels)
            {
                if (!string.IsNullOrEmpty(MinLabel) && !string.IsNullOrEmpty(MaxLabel))
                {
                    // Draws the left first x value
                    DrawHorizontalText(MinLabel, frame.Left, y, canvas, isBold: true);

                    // Measures the bounds on right text
                    var rightBounds = MeasureHorizontalLabel(MaxLabel, frame, isBold: true);

                    // Draws the last x value
                    DrawHorizontalText(MaxLabel, frame.Right - rightBounds.Width, y, canvas, isBold: true);
                }

                var item = GetChartEntriesFromXPoint(TouchedPoint.X, frame)?.FirstOrDefault();

                if (item?.Item1 != null)
                {
                    // Find x value where the slider is
                    var selectedLabelText = item.Item1.Label;
                    var nextLabelText = item.Item2?.Label;

                    // Don't display if close to start or end
                    if (selectedLabelText == MaxLabel || nextLabelText == MaxLabel || selectedLabelText == MinLabel)
                    {
                        return;
                    }

                    if (!(TouchedPoint.X > frame.Right || TouchedPoint.X < frame.Left))
                    {
                        // Measure the selected x value where the slider is
                        var selectedLabelTextBounds = MeasureHorizontalLabel(selectedLabelText, frame);

                        // Draws the x value where the slider is
                        DrawHorizontalText(selectedLabelText, TouchedPoint.X - (selectedLabelTextBounds.Width / 2), y, canvas);
                    }
                }
            }
            else
            {
                // Here we display ALL the x values //

                var chartEntry = ChartEntries.FirstOrDefault(x => x.IsVisible == true && x.Items.Any());

                if (chartEntry?.Items?.Any() != true)
                {
                    return;
                }

                var points = ChartEntries.FirstOrDefault().Items.Select(x => x.Point).ToArray();

                for (int i = 0; i < chartEntry.Items.Count(); i++)
                {
                    var entry = chartEntry.Items.ElementAt(i);
                    var point = points[i];

                    if (string.IsNullOrEmpty(entry?.Label))
                    {
                        continue;
                    }

                    var labelBounds = MeasureHorizontalLabel(entry.Label, frame);

                    DrawHorizontalText(entry.Label, point.X - (labelBounds.Width / 2), y, canvas);
                }
            }
        }

        protected void DrawSlider(SKCanvas canvas, SKRect frame)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderColor,
                StrokeWidth = this.SliderWidth
            })
            {
                float x = frame.GetInsideXValue(TouchedPoint.X);

                // Straight slider line
                canvas.DrawLine(x, frame.Top + VerticalLabelTextSize, x, frame.Bottom, paint);

                float y = frame.GetInsideYValue(TouchedPoint.Y, padding: 20);

                paint.StrokeWidth = 8f;

                if (TouchedPoint.X > (frame.Left + 40))
                {
                    // Left hint
                    canvas.DrawLine(x - 40, y, x - 20, y - 20, paint);
                    canvas.DrawLine(x - 40, y, x - 20, y + 20, paint);
                }

                if (TouchedPoint.X < (frame.Right - 40))
                {
                    // Right hint
                    canvas.DrawLine(x + 40, y, x + 20, y - 20, paint);
                    canvas.DrawLine(x + 40, y, x + 20, y + 20, paint);
                }
            }
        }

        protected SKRect MeasureHorizontalLabel(string text, SKRect frame, bool isBold = false)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = this.HorizontalLabelTextSize;
                paint.Color = this.HorizontalLabelColor;
                paint.FakeBoldText = isBold;
                paint.IsStroke = false;
                paint.IsAntialias = false;

                var bounds = new SKRect();
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

                return bounds;
            }
        }

        protected void DrawHorizontalText(string text, float x, float y, SKCanvas canvas, bool isBold = false)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = this.HorizontalLabelTextSize;
                paint.Color = this.HorizontalLabelColor;
                paint.FakeBoldText = isBold;
                paint.IsStroke = false;
                paint.IsAntialias = false;

                canvas.DrawText(text, x, y, paint);
            }
        }

        protected void GetExactValueFromPosition(SKCanvas canvas, SKRect frame)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            var chartEntryItems = this.GetChartEntriesFromXPoint(TouchedPoint.X, frame);

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
                var currentXPosition = TouchedPoint.X - chartEntry.Point.X;
                var points = currentXPosition * xValue;

                var y = (points / yValue) + chartEntry.Point.Y;

                using (var circlePaint = new SKPaint
                {
                    Color = tuple.Item3,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = this.SliderPointSize
                })
                {
                    canvas.DrawCircle(new SKPoint(TouchedPoint.X, y), this.SliderPointSize, circlePaint);
                }

                using (var textPaint = new SKPaint())
                {
                    textPaint.TextSize = HorizontalLabelTextSize;
                    textPaint.IsAntialias = true;
                    textPaint.Color = SKColors.White;
                    textPaint.IsStroke = false;

                    float currentVal = 0;

                    try
                    {
                        currentVal = chartEntry.Value + (int)Math.Round(decimal.Parse(points.ToString()), 0, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                        ex.Print();
                    }

                    if (TouchedPoint.X == chartEntry.Point.X)
                    {
                        currentVal = chartEntry.Value;
                    }
                    else if (TouchedPoint.X == nextChartEntry.Point.X)
                    {
                        currentVal = nextChartEntry.Value;
                    }

                    string text = currentVal.ToString() + " " + this.VerticalUnit;

                    SKRect textBounds = new SKRect();
                    textPaint.MeasureText(MaxValue.ToString() + " " + this.VerticalUnit, ref textBounds);

                    var rectWidth = textBounds.Width + 15;
                    var rectHeight = textBounds.Height + 15;

                    float xPosition, yPosition;

                    var count = chartEntryItems.Count;

                    if (SliderDetailOrientation == StackOrientation.Vertical)
                    {
                        var totalHeight = (rectHeight * count) + (5 * count);

                        xPosition = TouchedPoint.X - (rectWidth / 2);
                        yPosition = ((frame.Top + rectHeight) - (rectHeight / 2)) - (index * rectHeight) - (index * 5);
                    }
                    else
                    {

                        var totalDetailWidth = (rectWidth * count) + (5 * count);

                        if ((totalDetailWidth / 2) >= TouchedPoint.X - frame.Left)
                        {
                            xPosition = TouchedPoint.X + (index * rectWidth) + index * 5;
                        }
                        else if ((totalDetailWidth / 2) >= frame.Right - TouchedPoint.X)
                        {
                            xPosition = TouchedPoint.X - (totalDetailWidth - 5) + (((index * rectWidth) + index * 5));
                        }
                        else
                        {
                            var totalMiddle = totalDetailWidth / 2;

                            xPosition = TouchedPoint.X - (totalMiddle - (index * rectWidth)) + index * 5;
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

        protected void GetValueFromPosition(SKCanvas canvas, SKRect frame)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            var chartEntryItems = this.GetChartEntriesFromXPoint(TouchedPoint.X, frame);

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

                float y;

                if (nextChartEntry != null)
                {
                    // Difference in value
                    var diffVal = nextChartEntry.Value - chartEntry.Value;

                    // Difference in Y position
                    var diffY = nextChartEntry.Point.Y - chartEntry.Point.Y;

                    // Difference in X position
                    var diffX = nextChartEntry.Point.X - chartEntry.Point.X;

                    var yValue = diffVal / diffY;
                    var xValue = diffVal / diffX;

                    // Current X position over first item
                    var currentXPosition = TouchedPoint.X - chartEntry.Point.X;
                    var points = currentXPosition * xValue;

                    y = (points / yValue) + chartEntry.Point.Y;
                }
                else
                {
                    y = chartEntry.Point.Y;
                }

                using (var circlePaint = new SKPaint
                {
                    Color = tuple.Item3,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = this.SliderPointSize
                })
                {
                    float circleX = frame.GetInsideXValue(TouchedPoint.X);

                    if (this.LineMode == LineMode.Spline)
                    {
                        circleX = chartEntry.Point.X;
                        y = chartEntry.Point.Y;
                    }

                    canvas.DrawCircle(new SKPoint(circleX, y), this.SliderPointSize, circlePaint);
                }

                using (var textPaint = new SKPaint())
                {
                    textPaint.TextSize = HorizontalLabelTextSize;
                    textPaint.IsAntialias = true;
                    textPaint.Color = SKColors.White;
                    textPaint.IsStroke = false;

                    string text = Math.Round(double.Parse(chartEntry.Value.ToString()), 0, MidpointRounding.AwayFromZero).ToString() + " " + this.VerticalUnit;

                    SKRect textBounds = new SKRect();
                    textPaint.MeasureText(MaxValue + " " + this.VerticalUnit, ref textBounds);

                    var rectWidth = textBounds.Width + 25;
                    var rectHeight = textBounds.Height + 25;

                    float xPosition, yPosition;

                    var count = chartEntryItems.Count();

                    if (SliderDetailOrientation == StackOrientation.Vertical)
                    {
                        var totalHeight = (rectHeight * count) + (5 * count);

                        if ((TouchedPoint.X - (rectWidth / 2)) < frame.Left)
                        {
                            xPosition = frame.Left;
                        }
                        else if ((TouchedPoint.X + (rectWidth / 2)) > frame.Right)
                        {
                            xPosition = frame.Right - rectWidth;
                        }
                        else
                        {
                            xPosition = TouchedPoint.X - (rectWidth / 2);
                        }

                        yPosition = ((frame.Top + rectHeight) - (rectHeight / 2)) - (index * rectHeight) - (index * 5);
                    }
                    else
                    {

                        var totalDetailWidth = (rectWidth * count) + (5 * count);

                        if ((totalDetailWidth / 2) >= TouchedPoint.X - frame.Left)
                        {
                            xPosition = TouchedPoint.X + (index * rectWidth) + index * 5;
                        }
                        else if ((totalDetailWidth / 2) >= frame.Right - TouchedPoint.X)
                        {
                            xPosition = TouchedPoint.X - (totalDetailWidth - 5) + (((index * rectWidth) + index * 5));
                        }
                        else
                        {
                            var totalMiddle = totalDetailWidth / 2;

                            xPosition = TouchedPoint.X - (totalMiddle - (index * rectWidth)) + index * 5;
                        }

                        yPosition = frame.Top - (rectHeight / 2);
                    }

                    using (var rectPaint = new SKPaint
                    {
                        Style = SKPaintStyle.StrokeAndFill,
                        StrokeWidth = 3f,
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

                        SKRect textB = new SKRect();

                        textPaint.MeasureText(text + " " + this.VerticalUnit, ref textB);

                        canvas.DrawText(text, rect.MidX - (textB.Width / 2), rect.MidY + (textB.Height / 2), textPaint);
                    }
                }
            }
        }

        protected void DrawPoints(SKCanvas canvas, SKPoint[] points, ChartEntry entry)
        {
            if (points?.Any() != true)
            {
                return;
            }

            canvas.DrawPoints(SKPointMode.Points, points, new SKPaint
            {
                Color = entry.PointColor,
                StrokeWidth = entry.PointSize,
                StrokeCap = entry.StrokeCap
            });
        }

        /// <summary>
        /// Returns the a list of tuples of all chart entry items on given X position. 
        /// Also returns the next chart entry item in the list and the color of these chart entries parent.
        /// </summary>
        protected IList<Tuple<ChartEntryItem, ChartEntryItem, SKColor>> GetChartEntriesFromXPoint(float xPosition, SKRect frame)
        {
            // Return type
            var items = new List<Tuple<ChartEntryItem, ChartEntryItem, SKColor>>();

            if (!ChartEntries.Any(x => x.IsVisible))
            {
                return items;
            }

            foreach (var chartEntry in ChartEntries.Where(x => x.IsVisible))
            {
                // Order list and takes the first that's lower then X value
                var item = chartEntry.Items.OrderByDescending(c => c.Point.X).FirstOrDefault(c => c.Point.X <= xPosition);

                // No item found so check if we touched left of the frame
                if (item == null)
                {
                    item = chartEntry.Items.FirstOrDefault();

                    if (xPosition <= frame.Left && (item.Point.X <= frame.Left + 10))
                    {
                        items.Add(new Tuple<ChartEntryItem, ChartEntryItem, SKColor>(item, null, chartEntry.Color));
                    }

                    continue;
                }

                // Get the index of the found item
                int index = chartEntry.Items.IndexOf(item);

                // It's the last item in the list so add just the this entry
                if (index + 1 >= chartEntry.Items.Count)
                {
                    if (xPosition >= frame.Right && (item.Point.X >= frame.Right - 10))
                    {
                        items.Add(new Tuple<ChartEntryItem, ChartEntryItem, SKColor>(item, null, chartEntry.Color));
                    }

                    continue;
                }

                // Takes the next item in the list
                var nextItem = chartEntry.Items[index + 1];

                if (item != null)
                {
                    // Add current item, next item and parents line color
                    items.Add(new Tuple<ChartEntryItem, ChartEntryItem, SKColor>(item, nextItem, chartEntry.Color));
                }
            }

            return items;
        }

        /// <summary>
        /// Calculates the all X points 
        /// </summary>
        protected void CalculateXPoints(SKRect chart)
        {
            XPoints = new List<Tuple<string, float>>();

            var chartEntries = ChartEntries.Where(c => c.IsVisible)?.SelectMany(c => c.Items)?.GroupBy(c => c.Label)?.Select(c => c.First());

            // Calculate the width of one item (distance between items)
            var itemWidth = chart.GetItemWidth(chartEntries.Count());

            for (int i = 0; i < chartEntries.Count(); i++)
            {
                var entry = chartEntries.ElementAt(i);

                float x = chart.Left + (i * itemWidth);

                XPoints.Add(new Tuple<string, float>(entry.Tag, x));
            }
        }

        /// <summary>
        /// Calculate points towards chart and frame size
        /// </summary>
        protected SKPoint[] CalculatePoints(ChartEntry chartEntry, SKRect frame, SKRect chart)
        {
            var result = new List<SKPoint>();

            var width = chart.Width;

            var height = frame.Height;

            // Calculate how many values one y position is
            var valueY = MaxValue / height;

            // Calculate the width of one item (distance between items)
            var itemWidth = width / (MaxItems - 1);

            for (int i = 0; i < chartEntry.Items.Count(); i++)
            {
                var entry = chartEntry.Items.ElementAt(i);

                float x;

                if (XPoints.FirstOrDefault(p => p.Item1 == entry.Tag)?.Item2 > 0)
                {
                    x = XPoints.FirstOrDefault(p => p.Item1 == entry.Tag).Item2;
                }
                else
                {
                    x = chart.Left + (i * itemWidth);
                }

                // Calculate items y position with frame height (bottom to upper)
                var y = frame.Top + (height - (entry.Value / valueY));

                var point = new SKPoint(x, y);
                result.Add(point);

                entry.Point = point;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Calculates the footer height. Takes items label text size plus some margins.
        /// </summary>
        protected float CalculateFooterHeight()
        {
            var result = this.ChartMargin.Bottom;

            if (this.ChartEntries.SelectMany(x => x.Items).Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += VerticalLabelTextSize + this.ChartMargin.Bottom;
            }

            return result;
        }

        /// <summary>
        /// Calculates the left width where X axels labels will be.
        /// </summary>
        protected float CalculateLeftWidth(SKRect[] valueLabelSizes)
        {
            var result = this.ChartMargin.Left;

            if (this.ChartEntries.Any())
            {
                var maxValueWidth = valueLabelSizes.Max(x => x.Width);

                if (maxValueWidth > 0)
                {
                    result += maxValueWidth + this.ChartMargin.Left;
                }
            }

            return result;
        }

        protected SKRect[] MeasureVerticalLabels()
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = VerticalLabelTextSize;
                return this.ChartEntries.Select(e =>
                {
                    if (string.IsNullOrEmpty(MaxValue.ToString()))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    var text = MaxValue.ToString();
                    paint.MeasureText(text, ref bounds);
                    return bounds;
                }).ToArray();
            }
        }

        protected SKRect[] MeasureHorizontalLabels()
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = HorizontalLabelTextSize;
                return this.ChartEntries.SelectMany(x => x.Items).Select(e =>
                {
                    if (string.IsNullOrEmpty(e.Label))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    var text = e.Label;
                    paint.MeasureText(text, ref bounds);
                    return bounds;
                }).ToArray();
            }
        }

        private static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Chart view))
            {
                return;
            }

            view.InvalidateSurface();
        }

        public static readonly BindableProperty ChartEntriesProperty =
            BindableProperty.Create(
                "ChartEntries",
                typeof(IList<ChartEntry>),
                typeof(Chart),
                default(IList<ChartEntry>),
                propertyChanged: ItemSourceChanged);

        public IList<ChartEntry> ChartEntries
        {
            get { return (IList<ChartEntry>)GetValue(ChartEntriesProperty); }
            set { SetValue(ChartEntriesProperty, value); }
        }

        public SKColor FrameColor { get; set; } = SKColors.Gray;
        public float FrameWidth { get; set; } = 3f;

        public bool IsSliderVisible { get; set; } = true;
        public SKColor SliderColor { get; set; } = SKColors.Gray;
        public SKColor HintSliderColor { get; set; } = SKColors.Gray;
        public float SliderWidth { get; set; } = 4f;
        public float SliderPointSize { get; set; } = 8f;
        public StackOrientation SliderDetailOrientation { get; set; } = StackOrientation.Horizontal;

        public SKRect ChartPadding { get; set; } = new SKRect(0, 40, 0, 40);
        public SKRect ChartMargin { get; set; } = new SKRect(20, 100, 20, 20);

        public string VerticalUnit { get; set; }
        public string HorizontalUnit { get; set; }
        public SKColor VerticalLabelColor { get; set; } = SKColors.Black;
        public SKColor HorizontalLabelColor { get; set; } = SKColors.Black;
        public float HorizontalLabelTextSize { get; set; } = 42;
        public float VerticalLabelTextSize { get; set; } = 42;
        public bool HideHorizontalLabels { get; set; } = true;

        public float MinValue
        {
            get
            {
                if (!this.ChartEntries.Any())
                {
                    return 0;
                }

                if (this.InternalMinValue == null)
                {
                    return Math.Min(0, this.ChartEntries.SelectMany(x => x.Items).Min(x => x.Value));
                }

                return Math.Min(this.InternalMinValue.Value, this.ChartEntries.SelectMany(x => x.Items).Min(x => x.Value));
            }

            set => this.InternalMinValue = value;
        }
        public float MaxValue
        {
            get
            {
                if (this.ChartEntries?.SelectMany(x => x.Items)?.Any() != true)
                {
                    return 0;
                }

                var maxChartEntryValue = this.ChartEntries.SelectMany(x => x.Items).Max(x => x.Value);

                if (this.InternalMaxValue == null)
                {
                    if (maxChartEntryValue < 10)
                    {
                        return 10;
                    }
                    else if (maxChartEntryValue < 20)
                    {
                        return 20;
                    }
                    else if (maxChartEntryValue < 40)
                    {
                        return 40;
                    }
                    else if (maxChartEntryValue < 60)
                    {
                        return 60;
                    }
                    else if (maxChartEntryValue < 90)
                    {
                        return 100;
                    }

                    // Max value is beeing calculated for at least 100 values over top value
                    var max = Math.Round(maxChartEntryValue / 100d, 0, MidpointRounding.AwayFromZero) * 100;

                    max = max <= maxChartEntryValue + 100 ? max + 100 : max;

                    return (float)Math.Max(0, max);
                }

                return Math.Max(this.InternalMaxValue.Value, maxChartEntryValue);
            }

            set => this.InternalMaxValue = value;
        }
        protected float? InternalMinValue { get; set; }
        protected float? InternalMaxValue { get; set; }
        protected float ValueRange => this.MaxValue - this.MinValue;
        protected int MaxItems => ChartEntries.Where(c => c.IsVisible)?.SelectMany(c => c.Items)?.GroupBy(c => c.Label)?.Select(c => c.First())?.Count() ?? 0;
        protected string MaxLabel => ChartEntries.Where(c => c.IsVisible)?.SelectMany(c => c.Items)?.GroupBy(c => c.Label)?.Select(c => c.First())?.OrderBy(c => c.Point.X)?.LastOrDefault()?.Label;
        protected string MinLabel => ChartEntries.Where(c => c.IsVisible)?.SelectMany(c => c.Items)?.GroupBy(c => c.Label)?.Select(c => c.First())?.OrderBy(c => c.Point.X)?.FirstOrDefault()?.Label;
        protected IList<Tuple<string, float>> XPoints { get; set; }
        protected SKPoint TouchedPoint { get; set; } = new SKPoint(0, 0);
        public LineMode LineMode { get; set; } = LineMode.Straight;
    }
}
