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
                    canvas.DrawText(VerticalUnit, new SKPoint(left, (0f).GetVerticalAlignment(VerticalTextSize, TextAlignment.End)), textPaint);
                }

                // Draws maximum value
                canvas.DrawText(maximumValue.ToString(), new SKPoint(left, frame.Top.GetVerticalAlignment(VerticalTextSize, TextAlignment.End)), textPaint);

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
            var y = frame.Bottom + HorizontalTextSize + (HorizontalTextSize / 2);

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
                if (!string.IsNullOrEmpty(MaxLabel) && MinLabel != MaxLabel)
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

        protected void DrawHorizontalLabel(ChartValueItem entry, SKCanvas canvas, SKRect frame, SKRect chart)
        {
            if (!DisplayHorizontalValuesBySlider)
            {
                return;
            }

            // Draws the horizontal value the slider is on //

            if (string.IsNullOrEmpty(entry?.Label))
            {
                return;
            }

            float x = chart.GetInsideXValue(TouchedPoint.X);

            if (ChartType == ChartType.Bar)
            {
                x = entry.Point.X;
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
                var bounds = paint.GetBounds(MaxLabel, x, frame.Bottom + HorizontalTextSize + ChartRectMargin.Bottom + (float)SliderDetailPadding.Top + (float)SliderDetailPadding.Bottom, padding: new SKRect((float)SliderDetailPadding.Left, (float)SliderDetailPadding.Top, (float)SliderDetailPadding.Right, (float)SliderDetailPadding.Bottom));

                paint.PathEffect = SKPathEffect.CreateCorner(bounds.Width / 2);

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
                canvas.DrawText(entry.Label, x, frame.Bottom + HorizontalTextSize + ChartRectMargin.Bottom + (float)SliderDetailPadding.Top + (float)SliderDetailPadding.Bottom, paint);
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

        protected SKPoint[] CalculatePoints(IEnumerable<ChartValueItem> valueItems, SKRect frame, SKRect chart)
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

                if (ChartValueItemsXPoints.FirstOrDefault(p => p.Item1.ToString() == entry.Tag.ToString())?.Item2 > 0)
                {
                    x = ChartValueItemsXPoints.FirstOrDefault(p => p.Item1.ToString() == entry.Tag.ToString()).Item2;
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
            return this.ChartRectMargin.Top + (!string.IsNullOrEmpty(VerticalUnit) ? VerticalTextSize : 0f);
        }

        private float CalculateFooterHeight()
        {
            var result = this.ChartRectMargin.Bottom + (float)SliderDetailPadding.Bottom + HorizontalTextSize;

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

            if (valueLabelSizes?.Any() != true)
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
        public float HintSize { get; set; } = 50f;
        public Thickness SliderDetailPadding { get; set; } = new Thickness(14, 8);

        protected SKRect ChartRectPadding => new SKRect((float)ChartPadding.Left, (float)ChartPadding.Top, (float)ChartPadding.Right, (float)ChartPadding.Bottom);
        protected SKRect ChartRectMargin => new SKRect((float)ChartMargin.Left, (float)ChartMargin.Top, (float)ChartMargin.Right, (float)ChartMargin.Bottom);

        public Thickness ChartPadding { get; set; } = new Thickness(0, 0, 0, 0);
        public Thickness ChartMargin { get; set; } = new Thickness(0, 0, 0, 0);

        public string VerticalUnit { get; set; }
        public string HorizontalUnit { get; set; }
        public Color VerticalTextColor { get; set; } = Color.Black;
        public Color HorizontalTextColor { get; set; } = Color.Black;
        public float HorizontalTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 36f : 38f;
        public float VerticalTextSize { get; set; } = Device.RuntimePlatform == Device.Android ? 36f : 38;

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
                if (this.ChartEntries?.Where(x => x.IsVisible)?.SelectMany(x => x.Items)?.Any() != true)
                {
                    return 100;
                }

                var maxChartEntryValue = this.ChartEntries.Where(x => x.IsVisible).SelectMany(x => x.Items).Max(x => x.Value);

                if (this.InternalMaxValue == null)
                {
                    maxChartEntryValue *= 1.20f;

                    // Max value is beeing calculated for at least 100 values over top value
                    var max = Math.Round(maxChartEntryValue / 10d, 0, MidpointRounding.AwayFromZero) * 10;

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

        public float StrokeDashFirst { get; set; }
        public float StrokeDashSecond { get; set; }
        public float StrokeDashPhase { get; set; }

        private IList<Tuple<object, float>> ChartValueItemsXPoints { get; set; }
        private IEnumerable<ChartValueItem> ChartValuesDistinct => 
            BlockCount <= 0 ? 
            ChartEntries.Where(c => c.IsVisible && c.Items?.Count > 1)?.SelectMany(c => c.Items)?.GroupBy(c => c.Tag.ToString())?.Select(c => c.First())?.OrderBy(c => c.Tag.ToString()) : 
            ChartEntries.Where(c => c.IsVisible)?.SelectMany(c => c.Items)?.GroupBy(c => c.Tag.ToString())?.Select(c => c.First())?.OrderBy(c => c.Tag.ToString()).Skip(BlockStartIndex).Take(BlockCount);

        public int BlockStartIndex
        {
            get { return (int)GetValue(BlockStartIndexProperty); }
            set { SetValue(BlockStartIndexProperty, value); }
        }

        public static readonly BindableProperty BlockStartIndexProperty =
            BindableProperty.Create(
                "BlockStartIndex",
                typeof(int),
                typeof(BaseChart),
                default(int),
                propertyChanged: BlockStartIndexPropertyChanged);

        private static void BlockStartIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is BaseChart view))
            {
                return;
            }

            view.InvalidateSurface();
        }

        public int BlockCount
        {
            get { return (int)GetValue(BlockCountProperty); }
            set { SetValue(BlockCountProperty, value); }
        }

        public static readonly BindableProperty BlockCountProperty =
            BindableProperty.Create(
                "BlockCount",
                typeof(int),
                typeof(BaseChart),
                default(int));
    }
}
