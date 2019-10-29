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
        public ControlsViewModel()
        {
            if (NetStatusHelper.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await LoadItemsAsync();
                });
            }
            else
            {
                ShowNoNetworkError();
            }
        }

        private async Task LoadItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                await Task.Delay(TimeSpan.FromSeconds(1.5));

                Items = new ObservableCollection<ListItemViewModel>();

                for (int i = 0; i < 21; i++)
                {
                    Items.Add(new ListItemViewModel
                    {
                        Id = i,
                        Title = $"Title for item {i}",
                        SubTitle = $"Subtitle for item {i}"
                    });
                }

                TableItems = new ObservableCollection<TableItem>();

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

                    TableItems.Add(tableItem);
                }
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<ListItemViewModel> Items { get; private set; }
        public ObservableCollection<TableItem> TableItems { get; private set; }
    }
}
