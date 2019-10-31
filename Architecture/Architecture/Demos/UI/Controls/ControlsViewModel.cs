using Architecture.Controls;
using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture.Demos
{
    public class ControlsViewModel : BaseViewModel
    {
        public override void Appearing()
        {
            base.Appearing();

            ExecuteIfConnected(() =>
            {
                LoadItems();
            }, showAlert: true);
        }

        private void LoadItems()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));

                    var items = new ObservableCollection<ListItemViewModel>();

                    for (int i = 0; i < 21; i++)
                    {
                        items.Add(new ListItemViewModel
                        {
                            Id = i,
                            Title = $"Title for item {i}",
                            SubTitle = $"Subtitle for item {i}"
                        });
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Items = new ObservableCollection<ListItemViewModel>(items);
                    });

                    var tableItems = new ObservableCollection<TableItem>();

                    for (int i = 0; i <= 5; i++)
                    {
                        var tableItem = new TableItem($"Header {i}")
                        {
                            ContentItems = new List<TableContentItem>()
                        };

                        for (int x = 0; x <= 10; x++)
                        {
                            tableItem.ContentItems.Add(new TableContentItem($"Content {x}"));
                        }

                        tableItems.Add(tableItem);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TableItems = new ObservableCollection<TableItem>(tableItems);
                    });
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsBusy = false;
                    });
                }
            });
        }

        public ObservableCollection<ListItemViewModel> Items { get; private set; }
        public ObservableCollection<TableItem> TableItems { get; private set; }
    }
}
