using Architecture.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Architecture
{
    public class DataTableViewModel : BaseViewModel
    {
        public DataTableViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            TableItems = new ObservableCollection<TableItem>
            {
                new TableItem("Order ID")
                {
                    TextAlignment = TextAlignment.Start,
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("#12314") { TextAlignment = TextAlignment.Start },
                        new TableContentItem("#12314") { TextAlignment = TextAlignment.Start },
                        new TableContentItem("#12314") { TextAlignment = TextAlignment.Start },
                        new TableContentItem("#12314") { TextAlignment = TextAlignment.Start },
                        new TableContentItem("#12314") { TextAlignment = TextAlignment.Start }
                    }
                },
                new TableItem("City")
                {
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("New York"),
                        new TableContentItem("Chicago"),
                        new TableContentItem("New York"),
                        new TableContentItem("Oregon"),
                        new TableContentItem("Los Angeles")
                    }
                },
                new TableItem("Time")
                {
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("5 days"),
                        new TableContentItem("5 days"),
                        new TableContentItem("6 days"),
                        new TableContentItem("4 days"),
                        new TableContentItem("3 days")
                    }
                },
                new TableItem("Weight")
                {
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("2 kg"),
                        new TableContentItem("3 kg"),
                        new TableContentItem("0.5 kg"),
                        new TableContentItem("1 kg"),
                        new TableContentItem("7 kg")
                    }
                },
                new TableItem("Price")
                {
                    TextAlignment = TextAlignment.End,
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("$ 162.00") { TextAlignment = TextAlignment.End },
                        new TableContentItem("$ 12.00") { TextAlignment = TextAlignment.End },
                        new TableContentItem("$ 250.00") { TextAlignment = TextAlignment.End },
                        new TableContentItem("$ 124.00") { TextAlignment = TextAlignment.End },
                        new TableContentItem("$ 112.50") { TextAlignment = TextAlignment.End }
                    }
                }
            };
        }

        public ObservableCollection<TableItem> TableItems { get; private set; }
    }
}
