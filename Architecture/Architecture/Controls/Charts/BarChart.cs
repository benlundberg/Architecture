using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Linq;

namespace Architecture.Controls.Charts
{
    public class BarChart : BaseChart
    {
        public BarChart()
        {
            ChartType = ChartType.Bar;

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

                foreach (var entry in ChartEntries.Where(x => x.IsVisible).OrderByDescending(x => x.Items.Count()))
                {
                    CalculatePoints(entry.Items, frame, chart);
                }

                DrawBars(canvas, frame, chart);
            }

            DrawHorizontalLabels(canvas, frame, chart);
        }

        private void DrawBars(SKCanvas canvas, SKRect frame, SKRect chart)
        {
            var itemWidth = (MaxItems >= 12 ? chart.GetItemWidth(MaxItems) : chart.GetItemWidth(12)) / 2;
            var count = ChartEntries.Count(x => x.IsVisible);

            // Regular bar width
            var barWidth = (itemWidth / count) - 10;

            // Selected bar width
            var selectedValueItems = ChartEntries.GetChartValueItemFromX(chart.GetInsideXValue(TouchedPoint.X), chart, MaxItems, false);
            var selectedTags = selectedValueItems?.Select(x => x.ChartValueItem.Tag);

            int index = 0;

            foreach (var item in ChartEntries.Where(x => x.IsVisible))
            {
                foreach (var valueItem in item.Items.Where(x => selectedTags?.Contains(x.Tag) != true))
                {
                    // Draw not selected bars //

                    canvas.DrawBar(
                        valueItem.Point.X,
                        valueItem.Point.Y,
                        chart.Bottom,
                        barWidth,
                        BarMargin,
                        count,
                        index,
                        IsSliderVisible ? item.Color.ToSKColor().AsTransparency() : item.Color.ToSKColor(),
                        item.UseDashedEffect);
                }

                index++;
            }

            if (!IsSliderVisible)
            {
                return;
            }

            count = selectedValueItems?.Count() ?? 1;
            barWidth = (itemWidth / selectedValueItems?.Count() ?? 1) + 10;

            index = 0;

            foreach (var item in ChartEntries.Where(x => x.IsVisible))
            {
                var valueItems = item.Items.Where(x => selectedTags?.Contains(x.Tag) == true);

                if (valueItems?.Any() != true)
                {
                    continue;
                }

                foreach (var valueItem in valueItems)
                {
                    canvas.DrawBar(
                        valueItem.Point.X,
                        valueItem.Point.Y,
                        chart.Bottom,
                        barWidth,
                        BarMargin,
                        count,
                        index,
                        item.Color.ToSKColor(),
                        item.UseDashedEffect);

                    string text = Math.Round(double.Parse(valueItem.Value.ToString()), 0, MidpointRounding.AwayFromZero).ToString() + " " + this.VerticalUnit;

                    canvas.DrawSliderValue(
                        text,
                        frame.GetInsideXValue(valueItem.Point.X),
                        frame.Top,
                        SliderDetailTextSize,
                        SKColors.White,
                        item.Color.ToSKColor(),
                        SliderDetailPadding,
                        SliderDetailMargin,
                        MaxValue + " " + this.VerticalUnit,
                        selectedValueItems.Count,
                        index,
                        SliderDetailOrientation,
                        frame,
                        item.UseDashedEffect);
                }

                index++;
            }


            float hintX = selectedValueItems?.FirstOrDefault()?.ChartValueItem?.Point.X ?? 0;
            float hintY = selectedValueItems?.OrderByDescending(x => x.ChartValueItem.Point.Y)?.FirstOrDefault()?.ChartValueItem?.Point.Y ?? 0;

            if (hintX != 0 && hintY != 0)
            {
                DrawDragHintGraphic(
                    canvas,
                    hintX,
                    hintY + ((frame.Bottom - hintY) / 2),
                    (barWidth * count),
                    frame);
            }
        }

        public int BarMargin { get; set; }
    }
}
