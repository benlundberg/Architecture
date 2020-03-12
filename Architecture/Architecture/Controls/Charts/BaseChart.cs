using Architecture.Core;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public enum ChartType
    {
        Bar,
        Linear
    }

    public enum LabelMode
    {
        None,
        StartEnd,
        StartCenterEnd,
        All
    }

    public class BaseChart : SKCanvasView
    {
        protected SKRect CreateChart(SKRect frame)
        {
            return new SKRect(frame.Left + ChartRectPadding.Left, frame.Top + ChartRectPadding.Top, frame.Right - ChartRectPadding.Right, frame.Bottom - ChartRectPadding.Bottom);
        }

        protected SKRect CreateFrame(SKImageInfo info)
        {
            float leftPadding = 0;
            float bottomPadding = 0;
            float rightPadding = 0;

            if (VerticalLabelMode != LabelMode.None)
            {
                leftPadding = CalculateLeftWidth(MeasureVerticalLabels());
            }

            if (HorizontalLabelMode != LabelMode.None)
            {
                bottomPadding = CalculateFooterHeight();
                rightPadding = CalculateRightWidth(MeasureHorizontalLabels());
            }

            return new SKRect(leftPadding, ChartRectMargin.Top, info.Width - rightPadding, info.Height - bottomPadding);
        }

        protected void DrawFrame(SKCanvas canvas, SKRect frame)
        {
            if (HideFrame)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = this.FrameColor.ToSKColor(),
                StrokeWidth = this.FrameWidth,
                IsAntialias = true
            })
            {
                if (DashedFrame)
                {
                    paint.PathEffect = SKPathEffect.CreateDash(new float[] { 12, 12 }, 0);
                }

                // Left to bottom
                canvas.DrawLine(frame.Left, frame.Top, frame.Left, frame.Bottom, paint);

                // Left to right
                canvas.DrawLine(frame.Left, frame.Bottom - (paint.StrokeWidth / 2), frame.Right, frame.Bottom - (paint.StrokeWidth / 2), paint);
            }
        }

        protected void DrawVerticalLabels(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            using (var paint = new SKPaint
            {
                TextSize = VerticalLabelTextSize,
                Color = this.VerticalLabelColor.ToSKColor(),
                IsAntialias = true
            })
            {
                // Calculates maximum value depending on chart height
                var maximumValue = chart.Height * (MaxValue / chart.Height);

                if (VerticalLabelMode != LabelMode.None)
                {
                    // Draw vertical unit
                    if (!string.IsNullOrEmpty(VerticalUnit))
                    {
                        // Draws maximum value
                        canvas.DrawText(this.VerticalUnit, ChartRectMargin.Left, frame.Top + VerticalLabelTextSize, paint);
                        canvas.DrawText(maximumValue.ToString(), ChartRectMargin.Left, frame.Top + (VerticalLabelTextSize * 2), paint);
                    }
                    else
                    {
                        // Draws maximum value
                        canvas.DrawText(maximumValue.ToString(), ChartRectMargin.Left, frame.Top + VerticalLabelTextSize, paint);
                    }

                    canvas.DrawText("0", ChartRectMargin.Left, frame.Bottom, paint);
                }

                if (VerticalLabelMode == LabelMode.StartCenterEnd || VerticalLabelMode == LabelMode.All)
                {
                    using (var framePaint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = FrameColor.ToSKColor(),
                        StrokeWidth = FrameWidth / 2,
                    })
                    {
                        if (DashedFrame)
                        {
                            framePaint.PathEffect = SKPathEffect.CreateDash(new float[] { 12, 12 }, 0);
                        }

                        if (HideFrame)
                        {
                            canvas.DrawLine(frame.Left, string.IsNullOrEmpty(VerticalUnit) ? frame.Top + (VerticalLabelTextSize / 2) : frame.Top + VerticalLabelTextSize, frame.Right, string.IsNullOrEmpty(VerticalUnit) ? frame.Top + (VerticalLabelTextSize / 2) : frame.Top + VerticalLabelTextSize, framePaint);
                            canvas.DrawLine(frame.Left, frame.Bottom - (VerticalLabelTextSize / 4), frame.Right, frame.Bottom - (VerticalLabelTextSize / 4), framePaint);
                        }

                        canvas.DrawText((maximumValue / 2).ToString(), ChartRectMargin.Left, frame.MidY + (VerticalLabelTextSize / 4), paint);

                        canvas.DrawLine(frame.Left, frame.MidY, frame.Right, frame.MidY, framePaint);

                        if (VerticalLabelMode == LabelMode.All)
                        {
                            canvas.DrawText((maximumValue * .25).ToString(), ChartRectMargin.Left, (frame.Height * .75f) + (VerticalLabelTextSize / 4), paint);
                            canvas.DrawText((maximumValue * .75).ToString(), ChartRectMargin.Left, (frame.Height * .25f) + (VerticalLabelTextSize / 4), paint);

                            canvas.DrawLine(frame.Left, frame.Height * .75f, frame.Right, frame.Height * .75f, framePaint);
                            canvas.DrawLine(frame.Left, frame.Height * .25f, frame.Right, frame.Height * .25f, framePaint);
                        }
                    }
                }
            }
        }

        protected void DrawHorizontalLabels(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            if (HorizontalLabelMode != LabelMode.None)
            {
                if (!string.IsNullOrEmpty(MinLabel) && !string.IsNullOrEmpty(MaxLabel))
                {
                    canvas.DrawHorizontalText(MinLabel, chart.Left, frame.Bottom + HorizontalLabelTextSize, HorizontalLabelTextSize, HorizontalLabelColor.ToSKColor(), SKTextAlign.Center, isBold: true);
                    canvas.DrawHorizontalText(MaxLabel, chart.Right, frame.Bottom + HorizontalLabelTextSize, HorizontalLabelTextSize, HorizontalLabelColor.ToSKColor(), SKTextAlign.Center, isBold: true);
                }
            }

            if (!string.IsNullOrEmpty(HorizontalUnit))
            {
                canvas.DrawHorizontalText(HorizontalUnit, chart.Right, frame.Bottom + (HorizontalLabelTextSize * 2), HorizontalLabelTextSize, HorizontalLabelColor.ToSKColor(), SKTextAlign.Left);
            }

            if (DisplayHorizontalValuesBySlider && IsSliderVisible)
            {
                var x = ChartType == ChartType.Linear ? TouchedPoint.X : chart.GetInsideXValue(TouchedPoint.X);

                var valueItems = ChartEntries.GetChartValueItemFromX(x, frame, MaxItems, ChartType == ChartType.Linear);

                var entry = valueItems?.FirstOrDefault()?.ChartValueItem;

                if (string.IsNullOrEmpty(entry?.Label))
                {
                    return;
                }

                x = ChartType == ChartType.Linear ? chart.GetInsideXValue(TouchedPoint.X) : entry.Point.X;

                canvas.DrawHorizontalText(entry.Label, x, frame.Bottom + HorizontalLabelTextSize, HorizontalLabelTextSize, SliderColor.ToSKColor(), SKTextAlign.Center, isBold: true);
            }
            else if (HorizontalLabelMode == LabelMode.All)
            {
                var items = ChartValuesDistinct;

                if (items?.Any() != true)
                {
                    return;
                }

                var points = ChartEntries.FirstOrDefault().Items.Select(x => x.Point).ToArray();

                for (int i = 0; i < items.Count(); i++)
                {
                    var entry = items.ElementAt(i);
                    var point = points[i];

                    if (string.IsNullOrEmpty(entry?.Label) || entry?.Label == MinLabel || entry?.Label == MaxLabel)
                    {
                        continue;
                    }

                    canvas.DrawHorizontalText(entry.Label, point.X, frame.Bottom + HorizontalLabelTextSize, HorizontalLabelTextSize, HorizontalLabelColor.ToSKColor(), SKTextAlign.Center);
                }
            }
        }

        protected void DrawSlider(SKCanvas canvas, SKRect chart)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderColor.ToSKColor(),
                StrokeWidth = this.SliderWidth
            })
            {
                float x = chart.GetInsideXValue(TouchedPoint.X);

                // Straight slider line
                canvas.DrawLine(x, chart.Top, x, chart.Bottom - FrameWidth, paint);

                DrawDragHintGraphic(canvas, x, chart.GetInsideYValue(TouchedPoint.Y), 0, chart);
            }
        }

        protected void DrawDragHintGraphic(SKCanvas canvas, float x, float y, float width, SKRect frame)
        {
            if (!UseSliderHint)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.SliderColor.ToSKColor(),
                StrokeWidth = Device.RuntimePlatform == Device.Android ? this.SliderWidth * 2 : this.SliderWidth
            })
            {
                var left = x - (width / 2);

                if (TouchedPoint.X > (frame.Left + 40))
                {
                    // Left hint
                    canvas.DrawLine(left - 40, y, left - 20, y - 20, paint);
                    canvas.DrawLine(left - 40, y, left - 20, y + 20, paint);
                }

                var right = x + (width / 2);

                if (TouchedPoint.X < (frame.Right - 40))
                {
                    // Right hint
                    canvas.DrawLine(right + 40, y, right + 20, y - 20, paint);
                    canvas.DrawLine(right + 40, y, right + 20, y + 20, paint);
                }
            }
        }

        protected void ShowExactSliderValuesPosition(SKCanvas canvas, SKRect frame)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            var valueItems = ChartEntries.GetChartValueItemFromX(TouchedPoint.X, frame, MaxItems);

            if (valueItems?.Any() != true)
            {
                return;
            }

            int index = 0;

            foreach (var item in valueItems)
            {
                var chartEntry = item.ChartValueItem;

                var nextChartEntry = item.NextChartValueItem;

                float y, points;

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
                    var x = TouchedPoint.X - chartEntry.Point.X;
                    points = x * xValue;

                    y = (points / yValue) + chartEntry.Point.Y;
                }
                else
                {
                    y = chartEntry.Point.Y;
                    points = chartEntry.Value;
                }

                canvas.DrawSliderCircle(frame.GetInsideXValue(TouchedPoint.X), y, item.Color, SliderPointSize);

                float currentVal = 0;

                if (nextChartEntry != null)
                {
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
                    else if (TouchedPoint.X == nextChartEntry?.Point.X)
                    {
                        currentVal = nextChartEntry.Value;
                    }
                }
                else
                {
                    currentVal = chartEntry.Value;
                }

                string text = currentVal.ToString() + " " + this.VerticalUnit;

                canvas.DrawSliderValue(
                    text,
                    frame.GetInsideXValue(TouchedPoint.X),
                    frame.Top,
                    SliderDetailTextSize,
                    SKColors.White,
                    item.Color,
                    SliderDetailPadding,
                    SliderDetailMargin,
                    MaxValue + " " + VerticalUnit,
                    valueItems.Count,
                    index,
                    SliderDetailOrientation,
                    frame,
                    item.Parent?.UseDashedEffect == true);

                index++;
            }
        }

        protected void ShowSliderValuesPosition(SKCanvas canvas, SKRect frame)
        {
            if (!IsSliderVisible)
            {
                return;
            }

            var valueItems = ChartEntries.GetChartValueItemFromX(TouchedPoint.X, frame, MaxItems);

            if (valueItems?.Any() != true)
            {
                return;
            }

            int index = 0;

            foreach (var item in valueItems)
            {
                canvas.DrawSliderCircle(item.ChartValueItem.Point.X, item.ChartValueItem.Point.Y, item.Color, this.SliderPointSize);

                string text = Math.Round(double.Parse(item.ChartValueItem.Value.ToString()), 0, MidpointRounding.AwayFromZero).ToString() + " " + this.VerticalUnit;

                canvas.DrawSliderValue(
                    text,
                    frame.GetInsideXValue(TouchedPoint.X),
                    frame.Top,
                    SliderDetailTextSize,
                    SKColors.White,
                    item.Color,
                    SliderDetailPadding,
                    SliderDetailMargin,
                    MaxValue + " " + this.VerticalUnit,
                    valueItems.Count,
                    index,
                    SliderDetailOrientation,
                    frame,
                    item.Parent?.UseDashedEffect == true);

                index++;
            }
        }

        protected void DrawPoints(SKCanvas canvas, SKPoint[] points, ChartItem entry)
        {
            if (points?.Any() != true)
            {
                return;
            }

            canvas.DrawPoints(SKPointMode.Points, points, new SKPaint
            {
                Color = entry.PointColor.ToSKColor(),
                StrokeWidth = entry.PointSize,
                StrokeCap = entry.StrokeCap
            });
        }

        protected void CalculateChartValuesXPoints(SKRect chart)
        {
            ChartValueItemsXPoints = new List<Tuple<object, float>>();

            var chartEntries = ChartValuesDistinct;

            // Calculate the width of one item (distance between items)
            var itemWidth = chart.GetItemWidth(chartEntries.Count());

            for (int i = 0; i < chartEntries.Count(); i++)
            {
                var entry = chartEntries.ElementAt(i);

                float x = chart.Left + (i * itemWidth);

                ChartValueItemsXPoints.Add(new Tuple<object, float>(entry.Tag, x));
            }
        }

        protected SKPoint[] CalculatePoints(IList<ChartValueItem> valueItems, SKRect frame, SKRect chart)
        {
            var result = new List<SKPoint>();

            // Calculate how many values one y position is
            var valueY = MaxValue / frame.Height;

            // Calculate the width of one item (distance between items)
            var itemWidth = chart.GetItemWidth(MaxItems);

            for (int i = 0; i < valueItems.Count(); i++)
            {
                var entry = valueItems.ElementAt(i);

                float x;

                if (ChartValueItemsXPoints.FirstOrDefault(p => p.Item1 == entry.Tag)?.Item2 > 0)
                {
                    x = ChartValueItemsXPoints.FirstOrDefault(p => p.Item1 == entry.Tag).Item2;
                }
                else
                {
                    x = chart.Left + (i * itemWidth);
                }

                // Calculate items y position with frame height (bottom to upper)
                var y = frame.Top + (frame.Height - (entry.Value / valueY));

                var point = new SKPoint(x, y);
                result.Add(point);

                entry.Point = point;
            }

            return result.ToArray();
        }

        private float CalculateFooterHeight()
        {
            var result = this.ChartRectMargin.Bottom;

            if (this.ChartEntries.SelectMany(x => x.Items).Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += HorizontalLabelTextSize + this.ChartRectMargin.Bottom;

                if (!string.IsNullOrEmpty(HorizontalUnit))
                {
                    result += HorizontalLabelTextSize + this.ChartRectMargin.Bottom;
                }
            }

            return result;
        }

        private float CalculateLeftWidth(SKRect[] valueLabelSizes)
        {
            var result = this.ChartRectMargin.Left;

            if (VerticalLabelAlignment != TextAlignment.Start)
            {
                return result;
            }

            if (this.ChartEntries.Any())
            {
                var maxValueWidth = valueLabelSizes.Max(x => x.Width);

                if (maxValueWidth > 0)
                {
                    result += maxValueWidth + this.ChartRectMargin.Left;
                }
            }

            return result;
        }

        private float CalculateRightWidth(SKRect[] valueLabelSizes)
        {
            var result = this.ChartRectMargin.Right;

            if (VerticalLabelAlignment != TextAlignment.Start)
            {
                return result;
            }

            if (this.ChartEntries.Any())
            {
                var maxValueWidth = valueLabelSizes.Max(x => x.Width);

                if (maxValueWidth > 0)
                {
                    result += maxValueWidth + this.ChartRectMargin.Right;
                }
            }

            return result;
        }

        private SKRect[] MeasureVerticalLabels()
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

                    var unitBounds = new SKRect();

                    if (!string.IsNullOrEmpty(VerticalUnit))
                    {
                        paint.MeasureText(VerticalUnit, ref unitBounds);
                    }

                    // Returns the biggest width of value or vertical unit bounds
                    return bounds.Width > unitBounds.Width ? bounds : unitBounds;
                }).ToArray();
            }
        }

        private SKRect[] MeasureHorizontalLabels()
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

                    var unitBounds = new SKRect();

                    if (!string.IsNullOrEmpty(HorizontalUnit))
                    {
                        paint.MeasureText(HorizontalUnit, ref unitBounds);
                    }

                    // Returns the biggest width of value or vertical unit bounds
                    return bounds.Width > unitBounds.Width ? bounds : unitBounds;
                }).ToArray();
            }
        }

        private static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is BaseChart view))
            {
                return;
            }

            view.InvalidateSurface();
        }

        public static readonly BindableProperty ChartEntriesProperty =
            BindableProperty.Create(
                "ChartEntries",
                typeof(IList<ChartItem>),
                typeof(BaseChart),
                default(IList<ChartItem>),
                propertyChanged: ItemSourceChanged);

        public IList<ChartItem> ChartEntries
        {
            get { return (IList<ChartItem>)GetValue(ChartEntriesProperty); }
            set { SetValue(ChartEntriesProperty, value); }
        }

        protected ChartType ChartType { get; set; }
        public Color FrameColor { get; set; } = Color.Gray;
        public float FrameWidth { get; set; } = 3f;

        public TextAlignment VerticalLabelAlignment { get; set; } = TextAlignment.Start;
        public LabelMode VerticalLabelMode { get; set; } = LabelMode.All;
        public LabelMode HorizontalLabelMode { get; set; } = LabelMode.All;
        public bool UseExactValue { get; set; }
        public bool UseSliderHint { get; set; }
        public bool DisplayHorizontalValuesBySlider { get; set; }
        public bool HideFrame { get; set; }
        public bool DashedFrame { get; set; }

        public bool IsSliderVisible { get; set; }
        public Color SliderColor { get; set; } = Color.Gray;
        public Color HintSliderColor { get; set; } = Color.Gray;
        public float SliderWidth { get; set; } = 4f;
        public float SliderPointSize { get; set; } = 8f;
        public StackOrientation SliderDetailOrientation { get; set; }

        protected SKRect ChartRectPadding => new SKRect((float)ChartPadding.Left, (float)ChartPadding.Top, (float)ChartPadding.Right, (float)ChartPadding.Bottom);
        protected SKRect ChartRectMargin => new SKRect((float)ChartMargin.Left, (float)ChartMargin.Top, (float)ChartMargin.Right, (float)ChartMargin.Bottom);

        public Thickness ChartPadding { get; set; } = new Thickness(0, 0, 0, 0);
        public Thickness ChartMargin { get; set; } = new Thickness(0, 0, 0, 0);

        public string VerticalUnit { get; set; }
        public string HorizontalUnit { get; set; }
        public Color VerticalLabelColor { get; set; } = Color.Black;
        public Color HorizontalLabelColor { get; set; } = Color.Black;
        public float HorizontalLabelTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 42 : 32;
        public float VerticalLabelTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 42 : 32;
        public float SliderDetailTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 36 : 26;
        public int SliderDetailPadding { get; set; } = 24;
        public int SliderDetailMargin { get; set; } = 4;

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
        protected int MaxItems => ChartValuesDistinct?.Count() ?? 0;
        protected string MaxLabel => ChartValuesDistinct?.OrderBy(c => c.Point.X)?.LastOrDefault()?.Label;
        protected string MinLabel => ChartValuesDistinct?.OrderBy(c => c.Point.X)?.FirstOrDefault()?.Label;
        protected SKPoint TouchedPoint { get; set; } = new SKPoint(0, 0);

        private IList<Tuple<object, float>> ChartValueItemsXPoints { get; set; }
        private IEnumerable<ChartValueItem> ChartValuesDistinct => ChartEntries.Where(c => c.IsVisible)?.SelectMany(c => c.Items)?.GroupBy(c => c.Label)?.Select(c => c.First());
    }
}
