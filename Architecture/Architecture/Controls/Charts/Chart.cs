using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Architecture.Controls
{
    public class Chart : SKCanvasView
    {
        protected IList<Tuple<ChartEntryItem, ChartEntryItem, SKColor>> GetChartEntriesFromXPoint(float x)
        {
            var items = new List<Tuple<ChartEntryItem, ChartEntryItem, SKColor>>();

            foreach (var chartEntry in ChartEntries)
            {
                var item = chartEntry.Items.OrderByDescending(c => c.Point.X).FirstOrDefault(c => c.Point.X <= x);

                if (item == null)
                {
                    continue;
                }

                int index = chartEntry.Items.IndexOf(item);

                if (index + 1 >= chartEntry.Items.Count)
                {
                    continue;
                }

                var nextItem = chartEntry.Items[index + 1];

                if (item != null)
                {
                    items.Add(new Tuple<ChartEntryItem, ChartEntryItem, SKColor>(item, nextItem, chartEntry.Color));
                }
            }

            return items;
        }

        protected SKPoint[] CalculatePoints(IList<ChartEntryItem> items, SKRect frame, SKRect chart)
        {
            var result = new List<SKPoint>();

            var width = chart.Width;

            var height = frame.Height;

            var valueY = MaxValue / height;

            var itemWidth = width / (items.Count - 1);

            for (int i = 0; i < items.Count(); i++)
            {
                var entry = items.ElementAt(i);

                var x = chart.Left + (i * itemWidth);

                var y = frame.Top + (height - (entry.Value / valueY)); 

                var point = new SKPoint(x, y);
                result.Add(point);

                entry.Point = point;
            }

            return result.ToArray();
        }

        public SKSize CalculateChartSize(SKRect chart)
        {
            var width = chart.Width - ChartPadding.Left - ChartPadding.Right;
            var height = chart.Height - ChartPadding.Top - ChartPadding.Bottom;

            return new SKSize(width, height);
        }

        protected float CalculateFooterHeight()
        {
            var result = this.ChartMargin.Bottom;

            if (this.ChartEntries.SelectMany(x => x.Items).Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += VerticalLabelTextSize + this.ChartMargin.Bottom;
            }

            return result;
        }

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

        public SKColor FrameColor { get; set; } = SKColors.Gray;
        public float FrameWidth { get; set; } = 2f;

        public SKColor SliderColor { get; set; } = SKColors.Gray;
        public SKColor HintSliderColor { get; set; } = SKColors.Gray;
        public float SliderWidth { get; set; } = 2f;

        public SKRect ChartPadding { get; set; } = new SKRect(0, 40, 0, 40);
        public SKRect ChartMargin { get; set; } = new SKRect(20, 80, 20, 20);

        public string VerticalUnit { get; set; }
        public string HorizontalUnit { get; set; }
        public SKColor VerticalLabelColor { get; set; } = SKColors.Black;
        public SKColor HorizontalLabelColor { get; set; } = SKColors.Black;
        public float HorizontalLabelTextSize { get; set; } = 16;
        public float VerticalLabelTextSize { get; set; } = 16;

        public IList<ChartEntry> ChartEntries { get; set; }
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
                if (!this.ChartEntries.Any())
                {
                    return 0;
                }

                var maxChartEntryValue = this.ChartEntries.SelectMany(x => x.Items).Max(x => x.Value);

                if (this.InternalMaxValue == null)
                {
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
    }
}
