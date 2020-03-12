using Architecture.Controls;
using Architecture.Controls.Charts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Architecture
{
    public class TaskOverviewViewModel : BaseViewModel
    {
        public TaskOverviewViewModel()
        {
            LoadTableData();
            LoadChartEntries();

            PickerValues = new List<string>()
            {
                "Confirmed",
                "Ordered",
                "Delivered"
            };
        }

        private void LoadTableData()
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

        private void LoadChartEntries()
        {
            Random random = new Random();

            DateTime dateTime = new DateTime(year: 2019, month: 1, day: 1);

            var items1 = new List<ChartValueItem>();
            var items2 = new List<ChartValueItem>();
            var items3 = new List<ChartValueItem>();
            var items4 = new List<ChartValueItem>();

            for (int i = 0; i < 12; i++)
            {
                var date = dateTime.AddMonths(i);

                var value = random.Next(20, 90);

                items1.Add(new ChartValueItem
                {
                    Label = date.Month.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                items2.Add(new ChartValueItem
                {
                    Label = date.Month.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                items3.Add(new ChartValueItem
                {
                    Label = date.Month.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                items4.Add(new ChartValueItem
                {
                    Label = date.Month.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });
            }

            ChartEntries = new ObservableCollection<ChartItem>()
            {
                new ChartItem
                {
                    Id = 0,
                    Items = items1,
                    Color = App.Current.DarkPrimaryColor(),
                    PointColor = App.Current.DarkPrimaryColor(),
                },
                //new ChartItem
                //{
                //    Id = 1,
                //    Items = items2,
                //    Color = SKColors.CornflowerBlue,
                //    PointColor = SKColors.CornflowerBlue
                //},
                //new ChartItem
                //{
                //    Id = 2,
                //    Items = items3,
                //    Color = SKColors.Violet,
                //    PointColor = SKColors.Violet
                //},
                new ChartItem
                {
                    Id = 2,
                    Items = items4,
                    Color = App.Current.AccentColor(),
                    UseDashedEffect = true,
                    PointColor = App.Current.AccentColor(),
                }
            };
        }

        public string SelectedItem { get; set; }
        public List<string> PickerValues { get; private set; }
        public ObservableCollection<TableItem> TableItems { get; private set; }
        public ObservableCollection<ChartItem> ChartEntries { get; private set; }

        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public bool HasYear { get; set; } = true;
        public bool HasMonth { get; set; } = true;
        public bool HasDay { get; set; } = true;
    }
}
