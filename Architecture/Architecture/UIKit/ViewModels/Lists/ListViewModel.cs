using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Architecture.Core;
using Architecture.UIKit.Services;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var loadingPopup = new Controls.LoadingPopup(Resources.Strings.Gen_Loading);

                try
                {
                    await loadingPopup.ShowAsync();

                    await LoadItemsAsync();
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
                finally
                {
                    await loadingPopup.HideAsync();
                }
            });
        }

        private ICommand refreshListCommand;
        public ICommand RefreshListCommand => refreshListCommand ?? (refreshListCommand = new Command(async () =>
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));

                var items = UIKitService.GetListItems(5);

                items.ForEach(i => Items.Add(i));
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

        private ICommand addItemCommand;
        public ICommand AddItemCommand => addItemCommand ?? (addItemCommand = new Command(() =>
        {
            try
            {
                
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
            Device.BeginInvokeOnMainThread(() =>
            {
                if (IsBusy)
                {
                    return;
                }

                try
                {
                    IsBusy = true;

                    var item = Items.FirstOrDefault(x => x.Id == id);
                    
                    ShowAlert(item.SubTitle, item.Title);
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
            try
            {
                var items = new ObservableCollection<ListItemViewModel>(UIKitService.GetListItems(20));
                
                await Task.Delay(TimeSpan.FromSeconds(1));

                Items = new ObservableCollection<ListItemViewModel>(items);
                UnfilteredItems = Items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public bool SearchIsVisible { get; set; }
    }
}
