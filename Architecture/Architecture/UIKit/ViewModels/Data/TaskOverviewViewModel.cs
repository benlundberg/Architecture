using Architecture.Controls;
using Architecture.Controls.Charts;
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
                new TableFilterViewModel { Id = "City", IsVisible = true, Text = "City" },
                new TableFilterViewModel { Id = "Time", IsVisible = true, Text = "Time" },
                new TableFilterViewModel { Id = "Weight", IsVisible = true, Text = "Weight" },
                new TableFilterViewModel { Id = "Price", IsVisible = true, Text = "Price" }
            };

            LoadTableData();
            LoadChartEntries();
        }

        private void LoadTableData()
        {
            var items = new List<TableItemViewModel>()
            {
                new TableItemViewModel
                {
                    OrderID = "#12314",
                    City = "New York",
                    Price = "$ 165.00",
                    Time = "11 days",
                    Weight = "5 kg"
                },
                new TableItemViewModel
                {
                    OrderID = "#12314",
                    City = "Los Angeles",
                    Price = "$ 322.00",
                    Time = "4 days",
                    Weight = "1 kg"
                },
                new TableItemViewModel
                {
                    OrderID = "#12314",
                    City = "Chicago",
                    Price = "$ 162.00",
                    Time = "5 days",
                    Weight = "2 kg"
                },
                new TableItemViewModel
                {
                    OrderID = "#12314",
                    City = "Vancouver",
                    Price = "$ 32.00",
                    Time = "5 days",
                    Weight = "2 kg"
                },
                new TableItemViewModel
                {
                    OrderID = "#12314",
                    City = "Oregon",
                    Price = "$ 1622.00",
                    Time = "2 days",
                    Weight = "12 kg"
                },
            };

            TableItems = new ObservableCollection<TableItemViewModel>(items);
        }

        private void LoadChartEntries()
        {
            Random random = new Random();

            DateTime dateTime = new DateTime(year: 2019, month: 1, day: 1);

            var items1 = new List<ChartValueItem>();
            var items2 = new List<ChartValueItem>();

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
                new ChartItem
                {
                    Id = 1,
                    Items = items2,
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

        public ObservableCollection<TableItemViewModel> TableItems { get; private set; }
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

    public class TableFilterViewModel : TableFilterItem, INotifyPropertyChanged
    {
        public string Text { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TableItemViewModel : BaseItemViewModel
    {
        public string OrderID { get; set; }
        public string City { get; set; }
        public string Time { get; set; }
        public string Weight { get; set; }
        public string Price { get; set; }
    }
}
