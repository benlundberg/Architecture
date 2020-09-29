using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls.Charts
{
    public class ChartValueItemParam
    {
        public ChartValueItemParam(ChartValueItem chartValueItem, ChartValueItem nextChartValueItem, ChartItem parent)
        {
            this.ChartValueItem = chartValueItem;
            this.NextChartValueItem = nextChartValueItem;
            this.BackgroundColor = parent.Color;
            this.TextColor = parent.TextColor;
            this.Parent = parent;
        }

        public ChartValueItem ChartValueItem { get; set; }
        public ChartValueItem NextChartValueItem { get; set; }
        public ChartItem Parent { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
    }

    public static class ChartItemsExtensions
    {
        public static IList<ChartValueItemParam> GetChartValueItemFromX(this IList<ChartItem> chartItems, float xPosition, SKRect frame, float itemWidth)
        {
            var items = new List<ChartValueItemParam>();

            if (chartItems?.Any(x => x.IsVisible) != true)
            {
                return items;
            }

            foreach (var chartEntry in chartItems.Where(x => x.Items?.Any() == true && x.IsVisible))
            {
                var valueItems = chartEntry.Items;

                ChartValueItem item = null;

                if (true)
                {
                    // Order list and takes the first that's lower then X value
                    item = valueItems.OrderByDescending(c => c.Point.X).FirstOrDefault(c => c.Point.X <= xPosition);
                }
                else
                {
                    // Create a bound and get item inside this bound
                    foreach (var valueItem in valueItems)
                    {
                        SKRect rect = new SKRect((xPosition - (itemWidth / 2)), 0, (xPosition + (itemWidth / 2)), frame.Bottom + 2);

                        if (rect.Contains(valueItem.Point))
                        {
                            item = valueItem;
                        }
                    }
                }

                // No item found so check if we touched left of the frame
                if (item == null)
                {
                    item = valueItems.FirstOrDefault();

                    if ((xPosition <= frame.Left && item.Point.X == frame.Left))
                    {
                        items.Add(new ChartValueItemParam(item, null, chartEntry));
                    }

                    continue;
                }

                // Get the index of the found item
                int index = valueItems.ToList().IndexOf(item);

                // It's the last item in the list so add just the this entry
                if (index + 1 >= valueItems.Count())
                {
                    if ((xPosition >= frame.Right && item.Point.X == frame.Right) || item.Point.X >= (xPosition - (itemWidth / 2)))
                    {
                        items.Add(new ChartValueItemParam(item, null, chartEntry));
                    }

                    continue;
                }

                // Takes the next item in the list
                var nextItem = valueItems.ToList()[index + 1];

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
