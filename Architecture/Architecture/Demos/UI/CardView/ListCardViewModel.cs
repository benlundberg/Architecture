using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture.Demos
{
    public class ListCardViewModel : BaseViewModel
    {
        public override void Appearing()
        {
            base.Appearing();

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

        private void ItemSelected(int id)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (IsBusy)
                {
                    return;
                }

                try
                {
                    IsBusy = true;
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
                finally
                {
                    IsBusy = false;
                }
            });
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

                Items = new ObservableCollection<ListCardItemViewModel>();

                for (int i = 0; i < 21; i++)
                {
                    Items.Add(new ListCardItemViewModel
                    {
                        Id = i,
                        Title = $"Title for item {i}",
                        SubTitle = $"Subtitle for item {i}"
                    });
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

        private ListCardItemViewModel selectedItem;
        public ListCardItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                if (selectedItem != null)
                {
                    ItemSelected(selectedItem.Id);

                    SelectedItem = null;
                }
            }
        }

        public ObservableCollection<ListCardItemViewModel> Items { get; private set; }
        public bool IsRefreshing { get; set; }
    }

    public class ListCardItemViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
