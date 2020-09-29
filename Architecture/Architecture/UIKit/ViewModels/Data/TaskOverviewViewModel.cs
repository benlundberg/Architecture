using Architecture.Controls;
using Architecture.Controls.Charts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class TaskOverviewViewModel : BaseViewModel
    {
        public TaskOverviewViewModel()
        {
            PickerValues = new List<string>()
            {
                "Confirmed",
                "Ordered",
                "Delivered"
            };

            TableFilters = new ObservableCollection<TableFilterViewModel>
            {
                new TableFilterViewModel { Id = 1, IsActive = true, Text = "City" },
                new TableFilterViewModel { Id = 2, IsActive = true, Text = "Weight" }
            };

            LoadTableData();
            LoadChartEntries();
        }

        private void LoadTableData()
        {
            var items = new List<TableItem>();

            items.Add(new TableItem("Order ID")
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
            });

            if (TableFilters.FirstOrDefault(x => x.Id == 1).IsActive)
            {
                items.Add(new TableItem("City")
                {
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("New York"),
                        new TableContentItem("Chicago"),
                        new TableContentItem("New York"),
                        new TableContentItem("Oregon"),
                        new TableContentItem("Los Angeles")
                    }
                });
            }

            items.Add(new TableItem("Time")
            {
                ContentItems = new List<TableContentItem>
                {
                    new TableContentItem("5 days"),
                    new TableContentItem("5 days"),
                    new TableContentItem("6 days"),
                    new TableContentItem("4 days"),
                    new TableContentItem("3 days")
                }
            });

            if (TableFilters.FirstOrDefault(x => x.Id == 2).IsActive)
            {
                items.Add(new TableItem("Weight")
                {
                    ContentItems = new List<TableContentItem>
                    {
                        new TableContentItem("2 kg"),
                        new TableContentItem("3 kg"),
                        new TableContentItem("0.5 kg"),
                        new TableContentItem("1 kg"),
                        new TableContentItem("7 kg")
                    }
                });
            }

            items.Add(new TableItem("Price")
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
            });

            TableItems = new ObservableCollection<TableItem>(items);
        }

        private void LoadChartEntries()
        {
            Random random = new Random();

            DateTime dateTime = new DateTime(year: 2019, month: 1, day: 1);

            var items1 = new List<ChartValueItem>();
            var items2 = new List<ChartValueItem>();
            var items3 = new List<ChartValueItem>();
            var items4 = new List<ChartValueItem>();

            for (int i = 0; i < 10; i++)
            {
                var date = dateTime.AddYears(i);

                var value = random.Next(20, 90);

                items1.Add(new ChartValueItem
                {
                    Label = date.Year.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                items2.Add(new ChartValueItem
                {
                    Label = date.Year.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                items3.Add(new ChartValueItem
                {
                    Label = date.Year.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                items4.Add(new ChartValueItem
                {
                    Label = date.Year.ToString(),
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
                //    Color = Color.CornflowerBlue,
                //    PointColor = Color.CornflowerBlue
                //},
                //new ChartItem
                //{
                //    Id = 2,
                //    Items = items3,
                //    Color = Color.Violet,
                //    PointColor = Color.Violet
                //},
                new ChartItem
                {
                    Id = 3,
                    Items = items4,
                    Color = App.Current.AccentColor(),
                    UseDashedEffect = true,
                    PointColor = App.Current.AccentColor(),
                }
            };
        }

        private ICommand tableFilterChangedCommand;
        public ICommand TableFilterChangedCommand => tableFilterChangedCommand ?? (tableFilterChangedCommand = new Command(() =>
        {
            LoadTableData();
        }));

        private ICommand selectedChartEntriesChangedCommand;
        public ICommand SelectedChartEntriesChangedCommand => selectedChartEntriesChangedCommand ?? (selectedChartEntriesChangedCommand = new Command((param) =>
        {
            if (!(param is SelectedChartValueItemArgs args))
            {
                return;
            }

            if (args.ChartValueItems?.Any() != true)
            {
                SelectedChartEntries = new ObservableCollection<ChartEntryViewModel>();
            }
            else if (SelectedChartEntries?.Any() == true)
            {
                foreach (var item in args.ChartValueItems)
                {
                    var val = SelectedChartEntries.FirstOrDefault(x => x.Id == item.Parent.Id);

                    if (val != null)
                    {
                        val.Value = item.ChartValueItem.Value.ToString() + " tasks";
                    }
                    else
                    {
                        SelectedChartEntries.Add(new ChartEntryViewModel
                        {
                            BackgroundColor = item.BackgroundColor,
                            Id = item.Parent.Id,
                            TextColor = item.TextColor,
                            Value = item.ChartValueItem.Value.ToString() + " tasks"
                        });
                    }
                }
            }
            else
            {
                SelectedChartEntries = args.ChartValueItems == null ? new ObservableCollection<ChartEntryViewModel>() : new ObservableCollection<ChartEntryViewModel>(args.ChartValueItems.Select(x => new ChartEntryViewModel
                {
                    Id = x.Parent.Id,
                    BackgroundColor = x.BackgroundColor,
                    TextColor = x.TextColor,
                    Value = x.ChartValueItem.Value.ToString() + " tasks"
                }));
            }
        }));

        public string SelectedItem { get; set; }
        public List<string> PickerValues { get; private set; }

        public ObservableCollection<TableItem> TableItems { get; private set; }
        public ObservableCollection<TableFilterViewModel> TableFilters { get; set; }

        public ObservableCollection<ChartItem> ChartEntries { get; private set; }
        public ObservableCollection<ChartEntryViewModel> SelectedChartEntries { get; private set; }

        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public bool HasYear { get; set; } = true;
        public bool HasMonth { get; set; } = true;
        public bool HasDay { get; set; } = true;
    }

    public class ChartEntryViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TableFilterViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
