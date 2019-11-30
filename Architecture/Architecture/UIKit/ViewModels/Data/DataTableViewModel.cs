using Architecture.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Architecture
{
    public class DataTableViewModel : BaseViewModel
    {
        public DataTableViewModel()
        {
            LoadTableItems();
        }

        private void LoadTableItems()
        {
            TableItems = new ObservableCollection<TableItem>
            {
                new TableItem("Order ID")
                {
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("#12314"),
                        new TableContentItem("#12314"),
                        new TableContentItem("#12314"),
                        new TableContentItem("#12314"),
                        new TableContentItem("#12314")
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
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("$ 162.00"),
                        new TableContentItem("$ 12.00"),
                        new TableContentItem("$ 250.00"),
                        new TableContentItem("$ 124.00"),
                        new TableContentItem("$ 112.50")
                    }
                }
            };
        }

        public ObservableCollection<TableItem> TableItems { get; private set; }
    }
}
