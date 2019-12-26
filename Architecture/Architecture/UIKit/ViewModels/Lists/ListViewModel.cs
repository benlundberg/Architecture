using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel()
        {
            // We load items on appearing, if we have connection
            ExecuteIfConnected(() =>
            {
                LoadItems();

            }, showAlert: true);
        }

        private ICommand refreshListCommand;
        public ICommand RefreshListCommand => refreshListCommand ?? (refreshListCommand = new Command(async () =>
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));

                int listCount = Items.Count;

                for (int i = 0; i < 5; i++)
                {
                    Items.Add(new ListItemViewModel
                    {
                        Id = i,
                        Title = $"Title for item {listCount + i}",
                        SubTitle = $"Subtitle for item {listCount + i}"
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
            }
            finally
            {
                IsRefreshing = false;
            }
        }));

        private ICommand segmentValueChangedCommand;
        public ICommand SegmentValueChangedCommand => segmentValueChangedCommand ?? (segmentValueChangedCommand = new Command((param) =>
        {
            if (!int.TryParse(param?.ToString(), out int tag))
            {
                return;
            }

            IsImageCellVisible = tag == 2;
        }));

        private ICommand addItemCommand;
        public ICommand AddItemCommand => addItemCommand ?? (addItemCommand = new Command(async () =>
        {
            try
            {
                await EditItemAsync(new ListItemViewModel { Id = Items.Count });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
            }
            finally
            {
                IsRefreshing = false;
            }
        }));

        private ICommand deleteItemCommand;
        public ICommand DeleteItemCommand => deleteItemCommand ?? (deleteItemCommand = new Command((param) =>
        {
            try
            {
                if (!(param is ListItemViewModel item))
                {
                    return;
                }

                Items.Remove(item);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
            }
        }));

        private ICommand showSearchCommand;
        public ICommand ShowSearchCommand => showSearchCommand ?? (showSearchCommand = new Command(() =>
        {
            SearchIsVisible = !SearchIsVisible;

            if (!SearchIsVisible)
            {
                Query = string.Empty;
            }
        }));

        private ICommand searchTextChangedCommand;
        public ICommand SearchTextChangedCommand => searchTextChangedCommand ?? (searchTextChangedCommand = new Command(() =>
        {
            if (string.IsNullOrEmpty(Query))
            {
                Items = UnfilteredItems;
            }

            var filteredItems = new ObservableCollection<ListItemViewModel>(UnfilteredItems.Where(x => x.Title.ToLower().Contains(Query?.ToLower()) || x.SubTitle.ToLower().Contains(Query?.ToLower())));

            Items = filteredItems;
        }));

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

                    var item = Items.FirstOrDefault(x => x.Id == id);

                    await EditItemAsync(item);
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

        private async Task EditItemAsync(ListItemViewModel listItem)
        {
            //await Navigation.PushModalAsync(new NavigationPage(ViewContainer.Current.CreatePage(new ItemViewModel(listItem, (returnItem, addItem) =>
            //{
            //    if (addItem)
            //    {
            //        Items.Add(returnItem);
            //    }
            //    else
            //    {
            //        var itemToRemove = Items.FirstOrDefault(x => x.Id == returnItem.Id);
            //        Items.Remove(itemToRemove);
            //    }
            //}))));
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
                    // THIS WOULD BE WHERE WE GET ITEMS FROM SERVICE

                    var items = new ObservableCollection<ListItemViewModel>();

                    // We load items in a list first, this because we want to avoid heavy load on UI properties
                    for (int i = 0; i <= 20; i++)
                    {
                        items.Add(new ListItemViewModel
                        {
                            Id = i,
                            Title = $"Title for item {i}",
                            SubTitle = $"Subtitle for item {i}"
                        });
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Items = new ObservableCollection<ListItemViewModel>(items);
                        UnfilteredItems = Items;
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

        private ListItemViewModel selectedItem;
        public ListItemViewModel SelectedItem
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

        public ObservableCollection<ListItemViewModel> Items { get; private set; }
        public ObservableCollection<ListItemViewModel> UnfilteredItems { get; private set; }
        public string Query { get; set; }
        public bool IsRefreshing { get; set; }
        public bool IsImageCellVisible { get; set; }
        public bool SearchIsVisible { get; set; }
    }

    public class ListItemViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageSource => "http://lorempixel.com/400/400/nature/" + Id;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
