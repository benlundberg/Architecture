using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
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
            float leftPadding = CalculateLeftWidth(MeasureVerticalLabels());
            float rightPadding = CalculateRightWidth(MeasureHorizontalLabels());
            float bottomPadding = CalculateFooterHeight();
            float topPadding = CalculateHeaderHeight();

            return new SKRect(leftPadding, topPadding, info.Width - rightPadding, info.Height - bottomPadding);
        }

        protected void DrawFrame(SKCanvas canvas, SKRect frame)
        {
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

                if (!HideFrame)
                {
                    // Left to bottom
                    canvas.DrawLine(frame.Left, frame.Top, frame.Left, frame.Bottom, paint);

                    // Left to right bottom
                    canvas.DrawLine(frame.Left, frame.Bottom - (paint.StrokeWidth / 2), frame.Right, frame.Bottom - (paint.StrokeWidth / 2), paint);
                }

                // Draws vertical label lines //

                if (InsideFrame == LabelMode.StartCenterEnd || InsideFrame == LabelMode.All)
                {
                    if (HideFrame)
                    {
                        canvas.DrawLine(frame.Left, frame.Top, frame.Right, frame.Top, paint);
                        canvas.DrawLine(frame.Left, frame.Bottom - (paint.StrokeWidth / 2), frame.Right, frame.Bottom - (paint.StrokeWidth / 2), paint);
                    }

                    canvas.DrawLine(frame.Left, frame.MidY, frame.Right, frame.MidY, paint);
                }

                if (InsideFrame == LabelMode.All)
                {
                    canvas.DrawLine(frame.Left, (frame.Height * .75f) + frame.Top, frame.Right, (frame.Height * .75f) + frame.Top, paint);
                    canvas.DrawLine(frame.Left, (frame.Height * .25f) + frame.Top, frame.Right, (frame.Height * .25f) + frame.Top, paint);
                }
            }
        }

        protected void DrawVerticalLabels(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            if (VerticalLabelMode == LabelMode.None)
            {
                return;
            }

            // Calculates maximum value depending on chart height
            var maximumValue = chart.Height * (MaxValue / chart.Height);

            // Calculates where left is for all vertical labels
            var left = VerticalLabelAlignment == TextAlignment.Start ? frame.Left - ChartRectMargin.Left : ChartRectMargin.Left;

            using (var textPaint = new SKPaint
            {
                IsAntialias = true,
                TextSize = VerticalTextSize,
                Color = VerticalTextColor.ToSKColor(),
                Typeface = FontTypeService.GetFontFamily(),
                TextAlign = VerticalLabelAlignment == TextAlignment.Start ? SKTextAlign.Right : SKTextAlign.Left
            })
            {
                if (!string.IsNullOrEmpty(VerticalUnit))
                {
                    // Draws vertical unit and maximum value
                    canvas.DrawText(VerticalUnit, new SKPoint(left, frame.Top.GetVerticalAlignment(textPaint.TextSize, TextAlignment.End)), textPaint);
                    canvas.DrawText(maximumValue.ToString(), new SKPoint(left, (frame.Top + VerticalTextSize).GetVerticalAlignment(VerticalTextSize, TextAlignment.End)), textPaint);
                }
                else
                {
                    // Draws maximum value
                    canvas.DrawText(maximumValue.ToString(), new SKPoint(left, frame.Top.GetVerticalAlignment(VerticalTextSize, TextAlignment.End)), textPaint);
                }

                // Draws bottom value
                canvas.DrawText("0", new SKPoint(left, frame.Bottom), textPaint);

                if (VerticalLabelMode == LabelMode.StartEnd)
                {
                    return;
                }

                // Draws middle value
                canvas.DrawText((maximumValue / 2).ToString(), new SKPoint(left, frame.MidY.GetVerticalAlignment(VerticalTextSize, TextAlignment.Center)), textPaint);

                if (VerticalLabelMode == LabelMode.StartCenterEnd)
                {
                    return;
                }

                // Draws rest of values
                canvas.DrawText((maximumValue * .25).ToString(), new SKPoint(left, ((frame.Height * .75f) + frame.Top).GetVerticalAlignment(VerticalTextSize, TextAlignment.Center)), textPaint);
                canvas.DrawText((maximumValue * .75).ToString(), new SKPoint(left, ((frame.Height * .25f) + frame.Top).GetVerticalAlignment(VerticalTextSize, TextAlignment.Center)), textPaint);
            }
        }

        protected void DrawHorizontalLabels(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            if (HorizontalLabelMode == LabelMode.None)
            {
                return;
            }

            // Set y position where labels should be
            var y = frame.Bottom + HorizontalTextSize + ChartRectMargin.Bottom;

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                TextSize = HorizontalTextSize,
                Color = HorizontalTextColor.ToSKColor(),
                Typeface = FontTypeService.GetFontFamily(),
                TextAlign = SKTextAlign.Center
            })
            {
                // Draw min value
                if (!string.IsNullOrEmpty(MinLabel))
                {
                    canvas.DrawText(MinLabel, chart.Left, y, paint);
                }

                // Draw max value
                if (!string.IsNullOrEmpty(MaxLabel))
                {
                    canvas.DrawText(MaxLabel, chart.Right, y, paint);
                }

                if (HorizontalLabelMode == LabelMode.All)
                {
                    // Draw all labels
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

                        canvas.DrawText(entry.Label, point.X, y, paint);
                    }
                }

                // Draw horizontal unit
                if (!string.IsNullOrEmpty(HorizontalUnit))
                {
                    paint.TextAlign = SKTextAlign.Right;
                    canvas.DrawText(HorizontalUnit, chart.Right, DisplayHorizontalValuesBySlider ? y + HorizontalTextSize + ChartRectMargin.Bottom : y + HorizontalTextSize, paint);
                }
            }
        }

        protected void DrawSlider(SKCanvas canvas, SKRect frame, SKRect chart)
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

                DrawSliderHint(canvas, x, chart.GetInsideYValue(TouchedPoint.Y), 0, chart);
            }

            // Get items on x axis
            var valueItems = ChartEntries.GetChartValueItemFromX(x, frame, frame.GetItemWidth(MaxItems), UseExactValue);

            // Send selected items with command
            SelectedValuesCommand?.Execute(new SelectedChartValueItemArgs
            {
                ChartValueItems = valueItems,
                TouchedPoint = new SKPoint(x, TouchedPoint.Y)
            });

            if (valueItems?.Any() != true)
            {
                return;
            }

            // Draws circle on y axis //

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = SliderPointSize
            })
            {
                foreach (var item in valueItems)
                {
                    paint.Color = item.BackgroundColor.ToSKColor();

                    if (UseExactValue)
                    {
                        canvas.DrawCircle(x, ChartCalculator.CalculateYPosition(item.ChartValueItem, item.NextChartValueItem, x), SliderPointSize, paint);
                    }
                    else
                    {
                        canvas.DrawCircle(item.ChartValueItem.Point.X, item.ChartValueItem.Point.Y, SliderPointSize, paint);
                    }
                }
            }

            if (!DisplayHorizontalValuesBySlider)
            {
                return;
            }

            // Draws the horizontal value the slider is on //

            var entry = valueItems?.FirstOrDefault()?.ChartValueItem;

            if (string.IsNullOrEmpty(entry?.Label))
            {
                return;
            }

            // Draws background
            using (var paint = new SKPaint
            {
                Color = SliderColor.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 2f
            })
            {
                paint.PathEffect = SKPathEffect.CreateCorner(40);

                var bounds = paint.GetBounds(MaxLabel, x, frame.Bottom + HorizontalTextSize + ChartRectMargin.Bottom, padding: new SKRect(10, 8, 10, 5));
                canvas.DrawRect(bounds, paint);
            }

            // Draws text
            using (var paint = new SKPaint
            {
                IsAntialias = true,
                TextSize = HorizontalTextSize,
                Color = SliderDetailTextColor.ToSKColor(),
                Typeface = FontTypeService.GetFontFamily(),
                TextAlign = SKTextAlign.Center,
                FakeBoldText = true
            })
            {
                canvas.DrawText(entry.Label, x, frame.Bottom + HorizontalTextSize + ChartRectMargin.Bottom, paint);
            }
        }

        protected void DrawSliderHint(SKCanvas canvas, float x, float y, float width, SKRect frame)
        {
            if (!UseSliderHint)
            {
                return;
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = this.HintSliderColor.ToSKColor(),
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

        private float CalculateHeaderHeight()
        {
            return this.ChartRectMargin.Top;
        }

        private float CalculateFooterHeight()
        {
            var result = this.ChartRectMargin.Bottom + HorizontalTextSize;

            if (this.ChartEntries.SelectMany(x => x.Items).Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += HorizontalTextSize;

                if (!string.IsNullOrEmpty(HorizontalUnit))
                {
                    result += HorizontalTextSize + this.ChartRectMargin.Bottom;
                }
                else
                {
                    result += this.ChartRectMargin.Bottom;
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

            if (HorizontalLabelMode == LabelMode.None)
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
                paint.TextSize = VerticalTextSize;
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
                paint.TextSize = HorizontalTextSize;
                return this.ChartEntries.SelectMany(x => x.Items).Select(e =>
                {
                    if (string.IsNullOrEmpty(e.Label))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    var text = e.Label;
                    paint.MeasureText(text, ref bounds);

                    // Returns the biggest width of value or vertical unit bounds
                    return bounds;
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

        public static readonly BindableProperty SelectedValuesCommandProperty =
            BindableProperty.Create(
                "SelectedValuesCommand",
                typeof(ICommand),
                typeof(BaseChart),
                default(ICommand));

        public ICommand SelectedValuesCommand
        {
            get { return (ICommand)GetValue(SelectedValuesCommandProperty); }
            set { SetValue(SelectedValuesCommandProperty, value); }
        }

        protected ChartType ChartType { get; set; }
        public Color FrameColor { get; set; } = Color.Gray;
        public float FrameWidth { get; set; } = 3f;
        public LabelMode InsideFrame { get; set; } = LabelMode.All;

        public TextAlignment VerticalLabelAlignment { get; set; } = TextAlignment.Start;
        public LabelMode VerticalLabelMode { get; set; } = LabelMode.All;
        public LabelMode HorizontalLabelMode { get; set; } = LabelMode.All;
        public bool UseExactValue { get; set; }
        public bool UseSliderHint { get; set; }
        public bool DisplayHorizontalValuesBySlider { get; set; }
        public bool HideFrame { get; set; }
        public bool DashedFrame { get; set; }

        public bool IsSliderVisible { get; set; }
        public Color SliderColor { get; set; } = Color.Black;
        public Color SliderDetailTextColor { get; set; } = Color.White;
        public Color HintSliderColor { get; set; } = Color.Black;
        public float SliderWidth { get; set; } = 4f;
        public float SliderPointSize { get; set; } = 8f;

        protected SKRect ChartRectPadding => new SKRect((float)ChartPadding.Left, (float)ChartPadding.Top, (float)ChartPadding.Right, (float)ChartPadding.Bottom);
        protected SKRect ChartRectMargin => new SKRect((float)ChartMargin.Left, (float)ChartMargin.Top, (float)ChartMargin.Right, (float)ChartMargin.Bottom);

        public Thickness ChartPadding { get; set; } = new Thickness(0, 0, 0, 0);
        public Thickness ChartMargin { get; set; } = new Thickness(0, 0, 0, 0);

        public string VerticalUnit { get; set; }
        public string HorizontalUnit { get; set; }
        public Color VerticalTextColor { get; set; } = Color.Black;
        public Color HorizontalTextColor { get; set; } = Color.Black;
        public float HorizontalTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 42 : 32;
        public float VerticalTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 42 : 32;

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
