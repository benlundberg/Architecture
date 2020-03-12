using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Architecture.Controls.Charts
{
    public class ChartValueItemParam
    {
        public ChartValueItemParam(ChartValueItem chartValueItem, ChartValueItem nextChartValueItem, ChartItem parent)
        {
            this.ChartValueItem = chartValueItem;
            this.NextChartValueItem = nextChartValueItem;
            this.Color = parent.Color.ToSKColor();
            this.Parent = parent;
        }

        public ChartValueItem ChartValueItem { get; set; }
        public ChartValueItem NextChartValueItem { get; set; }
        public ChartItem Parent { get; set; }
        public SKColor Color { get; set; }
    }

    public static class ChartItemsExtensions
    {
        public static IList<ChartValueItemParam> GetChartValueItemFromX(this IList<ChartItem> chartItems, float xPosition, SKRect frame, int maxItems, bool takeClosest = true)
        {
            var items = new List<ChartValueItemParam>();

            if (chartItems?.Any(x => x.IsVisible) != true)
            {
                return items;
            }

            foreach (var chartEntry in chartItems.Where(x => x.Items?.Any() == true && x.IsVisible))
            {
                ChartValueItem item = null;

                if (!takeClosest)
                {
                    // Create a bound and get item inside this bound
                    foreach (var valueItem in chartEntry.Items)
                    {
                        SKRect rect = new SKRect((xPosition - (frame.GetItemWidth(maxItems) / 2)), 0, (xPosition + (frame.GetItemWidth(maxItems) / 2)), frame.Bottom + 2);

                        if (rect.Contains(valueItem.Point))
                        {
                            item = valueItem;
                        }
                    }
                }
                else
                {
                    // Order list and takes the first that's lower then X value
                    item = chartEntry.Items.OrderByDescending(c => c.Point.X).FirstOrDefault(c => c.Point.X <= xPosition);
                }

                // No item found so check if we touched left of the frame
                if (item == null)
                {
                    item = chartEntry.Items.FirstOrDefault();

                    if ((xPosition <= frame.Left && item.Point.X == frame.Left))
                    {
                        items.Add(new ChartValueItemParam(item, null, chartEntry));
                    }

                    continue;
                }

                // Get the index of the found item
                int index = chartEntry.Items.IndexOf(item);

                // It's the last item in the list so add just the this entry
                if (index + 1 >= chartEntry.Items.Count)
                {
                    if ((xPosition >= frame.Right && item.Point.X == frame.Right) || item.Point.X >= (xPosition - (frame.GetItemWidth(maxItems) / 2)))
                    {
                        items.Add(new ChartValueItemParam(item, null, chartEntry));
                    }

                    continue;
                }

                // Takes the next item in the list
                var nextItem = chartEntry.Items[index + 1];

                if (item != null)
                {
                    // Add current item, next item and parents line color
                    items.Add(new ChartValueItemParam(item, nextItem, chartEntry));
                }
            }

            return items;
        }
    }
}
