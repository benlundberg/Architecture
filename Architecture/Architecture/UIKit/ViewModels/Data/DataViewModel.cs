using Architecture.Controls.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class DataViewModel : BaseViewModel
    {
        public DataViewModel()
        {
            DataSources = new ObservableCollection<DataSourceItem>
            {
                new DataSourceItem
                {
                    Content = "",
                    Id = 0,
                    IsSelected = true,
                    Title = "Income",
                    BackgroundColor = App.Current.PrimaryColor(),
                    SelectedBackgroundColor = App.Current.PrimaryColor()
                },
                new DataSourceItem
                {
                    Content = "",
                    Id = 1,
                    IsSelected = true,
                    Title = "Spent",
                    BackgroundColor = App.Current.AccentColor(),
                    SelectedBackgroundColor = App.Current.AccentColor()
                }
            };

            LoadChartEntries();
        }

        private void LoadChartEntries()
        {
            Random random = new Random();

            DateTime dateTime = new DateTime(year: 2019, month: 1, day: 1);

            var items1 = new List<ChartValueItem>();
            var items2 = new List<ChartValueItem>();

            BarItems = new ObservableCollection<BarChartItem>();

            for (int i = 0; i < 10; i++)
            {
                var date = dateTime.AddYears(i);

                var value = random.Next(20, 90);

                var barChartItem = new BarChartItem
                {
                    Label = date.Year.ToString(),
                    Items = new List<BarChartSubItem>()
                };

                if (DataSources.FirstOrDefault(x => x.Id == 0)?.IsSelected == true)
                {
                    barChartItem.Items.Add(new BarChartSubItem
                    {
                        Id = 0,
                        ItemColor = App.Current.PrimaryColor(),
                        Value = value,
                        YValue = (300 / 100) * value
                    });
                }

                items1.Add(new ChartValueItem
                {
                    Label = date.Year.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                value = random.Next(20, 90);

                if (DataSources.FirstOrDefault(x => x.Id == 1)?.IsSelected == true)
                {
                    barChartItem.Items.Add(new BarChartSubItem
                    {
                        Id = 1,
                        ItemColor = App.Current.AccentColor(),
                        Value = value,
                        YValue = (300 / 100) * value
                    });
                };

                items2.Add(new ChartValueItem
                {
                    Label = date.Year.ToString(),
                    Value = value,
                    Tag = date.ToString()
                });

                BarItems.Add(barChartItem);
            }

            ChartEntries = new ObservableCollection<ChartItem>()
            {
                new ChartItem
                {
                    Id = 0,
                    Items = items1,
                    Color = App.Current.PrimaryColor(),
                    PointColor = App.Current.PrimaryColor(),
                    IsVisible = DataSources.FirstOrDefault(x => x.Id == 0)?.IsSelected == true
                },
                new ChartItem
                {
                    Id = 1,
                    Items = items2,
                    Color = App.Current.AccentColor(),
                    UseDashedEffect = true,
                    PointColor = App.Current.AccentColor(),
                    IsVisible = DataSources.FirstOrDefault(x => x.Id == 1)?.IsSelected == true
                }
            };
        }

        private ICommand selectedChartEntriesChangedCommand;
        public ICommand SelectedChartEntriesChangedCommand => selectedChartEntriesChangedCommand ?? (selectedChartEntriesChangedCommand = new Command((param) =>
        {
            if (!(param is SelectedChartValueItemArgs args))
            {
                return;
            }

            foreach (var item in DataSources)
            {
                var selectedItem = args.ChartValueItems.FirstOrDefault(x => x.Parent.IsVisible && x.Parent.Id == item.Id);

                if (selectedItem == null)
                {
                    item.Content = "";
                }
                else
                {
                    item.Content = selectedItem.ChartValueItem.Value.ToString() + " $";
                }
            }
        }));

        private DataSourceItem selectedDataSource;
        public DataSourceItem SelectedDataSource
        {
            get { return selectedDataSource; }
            set
            {
                selectedDataSource = value;

                if (selectedDataSource != null)
                {
                    DataSourceSelected(value);

                    SelectedDataSource = null;
                }
            }
        }

        private BarChartItem selectedBarChartItem;
        public BarChartItem SelectedBarChartItem
        {
            get { return selectedBarChartItem; }
            set 
            { 
                selectedBarChartItem = value;

                if (selectedBarChartItem == null)
                {
                    return;
                }

                foreach (var item in BarItems)
                {
                    item.LabelDisplay = "";
                }

                selectedBarChartItem.LabelDisplay = selectedBarChartItem.Label;

                foreach (var item in DataSources)
                {
                    var selectedItem = selectedBarChartItem.Items.FirstOrDefault(x => x.Id == item.Id);

                    if (selectedItem == null)
                    {
                        item.Content = "";
                    }
                    else
                    {
                        item.Content = selectedItem.Value.ToString() + " $";
                    }
                }

                SelectedBarChartItem = null;
            }
        }


        private void DataSourceSelected(DataSourceItem value)
        {
            value.IsSelected = !value.IsSelected;

            value.BackgroundColor = value.IsSelected ? value.SelectedBackgroundColor : App.Current.Get<Color>("Gray");

            LoadChartEntries();
        }

        public ObservableCollection<ChartItem> ChartEntries { get; private set; }
        public ObservableCollection<DataSourceItem> DataSources { get; private set; }
        public ObservableCollection<BarChartItem> BarItems { get; private set; }
    }

    public class DataSourceItem : BaseItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Color BackgroundColor { get; set; }
        public Color SelectedBackgroundColor { get; set; }
        public bool IsSelected { get; set; }
    }

    public class BarChartItem : BaseItemViewModel
    {
        public string Label { get; set; }
        public string LabelDisplay { get; set; }
        public List<BarChartSubItem> Items { get; set; }
    }

    public class BarChartSubItem : BaseItemViewModel
    {
        public int Id { get; set; }
        public Color ItemColor { get; set; }
        public float Value { get; set; }
        public double YValue { get; set; }
    }
}
