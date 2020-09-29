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
        public BaseChart()
        {
            this.FrameWidth = 2f;
            this.HorizontalTextSize = 14f;
            this.VerticalTextSize = 14f;
            this.ChartMargin = new Thickness(8, 0, 16, 0);
            this.SliderDetailCornerRadius = 2f;
            this.SliderDetailPadding = new Thickness(4, 0, 4, 1);
            this.SliderWidth = 2f;
            this.SliderPointSize = 4f;
        }

        protected SKRect CreateFrame(SKImageInfo info)
        {
            float leftPadding = CalculateLeftWidth(MeasureVerticalLabels());
            float rightPadding = CalculateRightWidth(MeasureHorizontalLabels());
            float bottomPadding = CalculateFooterHeight();
            float topPadding = CalculateHeaderHeight();

            return new SKRect(leftPadding, topPadding, info.Width - rightPadding, info.Height - bottomPadding);
        }

        protected SKRect CreateChart(SKRect frame)
        {
            // Padding is calculated with a percentage
            var left = frame.Width * (ChartRectPadding.Left / 100);
            var top = frame.Height * (ChartRectPadding.Top / 100);
            var right = frame.Width * (ChartRectPadding.Right / 100);
            var bottom = frame.Height * (ChartRectPadding.Bottom / 100);

            return new SKRect(frame.Left + left, frame.Top + top, frame.Right - right, frame.Bottom - bottom);
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
            }
        }

        protected void DrawInnerFrame(SKCanvas canvas, SKRect frame)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = this.InnerFrameColor.ToSKColor(),
                StrokeWidth = this.FrameWidth,
                IsAntialias = true
            })
            {
                if (DashedFrame)
                {
                    paint.PathEffect = SKPathEffect.CreateDash(new float[] { 12, 12 }, 0);
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

            float left = 0;

            // Calculates where left is for all vertical labels
            switch (VerticalLabelAlignment)
            {
                case TextAlignment.Start:
                    left = frame.Left - ChartRectMargin.Left;
                    break;
                case TextAlignment.Center:
                    left = ChartRectMargin.Left / 2;
                    break;
                case TextAlignment.End:
                    left = frame.Left + (ChartRectMargin.Left / 2);
                    break;
            }

            using (var textPaint = new SKPaint
            {
                IsAntialias = true,
                TextSize = VerticalTextSize,
                Color = VerticalTextColor.ToSKColor(),
                Typeface = FontTypeService.GetFontFamily(GetType().Assembly),
                TextAlign = VerticalLabelAlignment == TextAlignment.Start ? SKTextAlign.Right : SKTextAlign.Left
            })
            {
                if (!string.IsNullOrEmpty(VerticalUnit))
                {
                    // Draws vertical unit and maximum value
                    canvas.DrawText(VerticalUnit, new SKPoint(left, (0f).GetVerticalAlignment(VerticalTextSize, TextAlignment.End)), textPaint);
                }

                // Draws maximum value
                canvas.DrawText(maximumValue.ToRoundedString(), new SKPoint(left, frame.Top.GetVerticalAlignment(VerticalTextSize, TextAlignment.End)), textPaint);

                // Draws bottom value
                canvas.DrawText("0", new SKPoint(left, frame.Bottom), textPaint);

                if (VerticalLabelMode == LabelMode.StartEnd)
                {
                    return;
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
            var y = frame.Bottom + HorizontalTextSize + (HorizontalTextSize / 4);

            using (var paint = new SKPaint
            {
                IsAntialias = true,
                TextSize = HorizontalTextSize,
                Color = HorizontalTextColor.ToSKColor(),
                Typeface = FontTypeService.GetFontFamily(GetType().Assembly),
                TextAlign = SKTextAlign.Center
            })
            {
                // Draw min value
                if (!string.IsNullOrEmpty(MinLabel))
                {
                    paint.TextAlign = ChartType == ChartType.Bar || HorizontalLabelMode == LabelMode.All ? SKTextAlign.Center : SKTextAlign.Left;
                    canvas.DrawText(MinLabel, chart.Left, y, paint);
                }

                // Draw max value
                if (!string.IsNullOrEmpty(MaxLabel) && MaxLabel != MinLabel)
                {
                    paint.TextAlign = ChartType == ChartType.Bar || HorizontalLabelMode == LabelMode.All ? SKTextAlign.Center : SKTextAlign.Right;
                    canvas.DrawText(MaxLabel, chart.Right, y, paint);
                }

                paint.TextAlign = SKTextAlign.Center;

                if (HorizontalLabelMode == LabelMode.All)
                {
                    // Draw all labels
                    var items = ChartValuesDistinct;

                    if (items?.Any() != true)
                    {
                        return;
                    }

                    for (int i = 0; i < items.Count(); i++)
                    {
                        var entry = items.ElementAt(i);
                        var point = entry.Point;

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
                    paint.TextAlign = ChartType == ChartType.Bar || HorizontalLabelMode == LabelMode.All ? SKTextAlign.Center : SKTextAlign.Right;
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
            using (var boundPaint = new SKPaint
            {
                Color = SliderColor.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = SliderWidth
            })
            {
                var bounds = boundPaint.GetBounds(
                    entry.Label,
                    x,
                    frame.Bottom + HorizontalTextSize + (HorizontalTextSize / 4),
                    padding: InternalSliderDetailPadding);

                if (bounds.Right > frame.Right)
                {
                    bounds.Left = frame.Right - bounds.Width + SliderWidth;
                    bounds.Right = frame.Right + SliderWidth;
                }
                else if (bounds.Left < frame.Left)
                {
                    bounds.Right = frame.Left + bounds.Width - SliderWidth;
                    bounds.Left = frame.Left - SliderWidth;
                }

                canvas.DrawRoundRect(new SKRoundRect(bounds, SliderDetailCornerRadius), boundPaint);

                // Draws text
                using (var textPaint = new SKPaint
                {
                    IsAntialias = true,
                    TextSize = HorizontalTextSize,
                    Color = SliderDetailTextColor.ToSKColor(),
                    Typeface = FontTypeService.GetFontFamily(GetType().Assembly),
                    TextAlign = SKTextAlign.Center,
                    FakeBoldText = true
                })
                {
                    canvas.DrawText(entry.Label, bounds.MidX, bounds.MidY + (bounds.Height / 4), textPaint);
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
            var result = this.ChartRectMargin.Bottom + (float)InternalSliderDetailPadding.Bottom + HorizontalTextSize;

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

            if (HorizontalLabelMode != LabelMode.All)
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
                    result += (maxValueWidth / 2);
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

        public static readonly BindableProperty SelectedTagProperty =
            BindableProperty.Create(
                "SelectedTag",
                typeof(string),
                typeof(BaseChart),
                default(string));

        public string SelectedTag
        {
            get { return (string)GetValue(SelectedTagProperty); }
            set { SetValue(SelectedTagProperty, value); }
        }


        protected ChartType ChartType { get; set; }

        public LabelMode InsideFrame { get; set; } = LabelMode.None;
        public Color FrameColor { get; set; } = Color.Gray;
        public Color InnerFrameColor { get; set; } = Color.LightGray;
        public bool HideFrame { get; set; }
        public bool DashedFrame { get; set; }

        public Color ChartBackgroundColor { get; set; } = Color.WhiteSmoke;
        public bool HasBackground { get; set; } = true;

        private float frameWidth;
        public float FrameWidth
        {
            get { return frameWidth; }
            set { frameWidth = value.ToDpiAdjusted(); }
        }

        public TextAlignment VerticalLabelAlignment { get; set; } = TextAlignment.Start;
        public LabelMode VerticalLabelMode { get; set; } = LabelMode.All;
        public LabelMode HorizontalLabelMode { get; set; } = LabelMode.StartEnd;

        /// <summary>
        /// Displays the horizontal value by slider
        /// </summary>
        public bool DisplayHorizontalValuesBySlider { get; set; } = true;

        /// <summary>
        /// Hide or show slider
        /// </summary>
        public bool IsSliderVisible { get; set; } = true;
        public Color SliderColor { get; set; } = Color.Black;
        public Color SliderDetailTextColor { get; set; } = Color.White;

        private float sliderWidth;
        public float SliderWidth
        {
            get { return sliderWidth; }
            set { sliderWidth = value.ToDpiAdjusted(); }
        }

        private float sliderPointSize;
        public float SliderPointSize
        {
            get { return sliderPointSize; }
            set { sliderPointSize = value.ToDpiAdjusted(); }
        }

        protected SKRect InternalSliderDetailPadding { get; private set; }

        private Thickness sliderDetailPadding;
        public Thickness SliderDetailPadding
        {
            get { return sliderDetailPadding; }
            set
            {
                sliderDetailPadding = value;

                InternalSliderDetailPadding = new SKRect(((float)SliderDetailPadding.Left).ToDpiAdjusted(), ((float)SliderDetailPadding.Top).ToDpiAdjusted(), ((float)SliderDetailPadding.Right).ToDpiAdjusted(), ((float)SliderDetailPadding.Bottom).ToDpiAdjusted());
            }
        }

        private float sliderDetailCornerRadius;
        public float SliderDetailCornerRadius
        {
            get { return sliderDetailCornerRadius; }
            set { sliderDetailCornerRadius = value.ToDpiAdjusted(); }
        }

        protected SKRect ChartRectPadding { get; private set; }

        protected SKRect ChartRectMargin { get; private set; }

        private Thickness chartPadding;
        public Thickness ChartPadding
        {
            get { return chartPadding; }
            set
            {
                chartPadding = new Thickness(
                    value.Left * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density,
                    value.Top * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density,
                    value.Right * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density,
                    value.Bottom * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density);

                ChartRectPadding = new SKRect((float)ChartPadding.Left, (float)ChartPadding.Top, (float)ChartPadding.Right, (float)ChartPadding.Bottom);
            }
        }

        private Thickness chartMargin;
        public Thickness ChartMargin
        {
            get { return chartMargin; }
            set
            {
                chartMargin = new Thickness(
                    value.Left * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density,
                    value.Top * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density,
                    value.Right * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density,
                    value.Bottom * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density);

                ChartRectMargin = new SKRect((float)ChartMargin.Left, (float)ChartMargin.Top, (float)ChartMargin.Right, (float)ChartMargin.Bottom);
            }
        }

        public string VerticalUnit { get; set; }
        public string HorizontalUnit { get; set; }
        public Color VerticalTextColor { get; set; } = Color.Black;
        public Color HorizontalTextColor { get; set; } = Color.Black;

        private float horizontalTextSize;
        public float HorizontalTextSize
        {
            get { return horizontalTextSize; }
            set { horizontalTextSize = value.ToDpiAdjusted(); }
        }

        private float verticalTextSize;
        public float VerticalTextSize
        {
            get { return verticalTextSize; }
            set { verticalTextSize = value.ToDpiAdjusted(); }
        }

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

        public float StrokeDashFirst { get; set; } = 12f;
        public float StrokeDashSecond { get; set; } = 20f;
        public float StrokeDashPhase { get; set; } = 20f;

        protected IList<Tuple<object, float>> ChartValueItemsXPoints { get; set; }
        private IEnumerable<ChartValueItem> ChartValuesDistinct => ChartEntries.Where(c => c.IsVisible && c.Items?.Count > 1)?.SelectMany(c => c.Items)?.GroupBy(c => c.Tag.ToString())?.Select(c => c.First())?.OrderBy(c => c.Tag.ToString());
    }
}
