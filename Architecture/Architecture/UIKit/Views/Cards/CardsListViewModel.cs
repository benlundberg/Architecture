using Architecture.Core;
using Architecture.UIKit.Services;
using Architecture.UIKit.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class CardsListViewModel : BaseViewModel
    {
        public CardsListViewModel()
        {
            Query = new ValidatableObject<string>();

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

        private ICommand showSearchCommand;
        public ICommand ShowSearchCommand => showSearchCommand ?? (showSearchCommand = new Command(() =>
        {
            SearchIsVisible = !SearchIsVisible;

            if (!SearchIsVisible)
            {
                Query.Value = string.Empty;
            }
        }));

        private ICommand searchTextChangedCommand;
        public ICommand SearchTextChangedCommand => searchTextChangedCommand ?? (searchTextChangedCommand = new Command(() =>
        {
            if (string.IsNullOrEmpty(Query.Value))
            {
                Items = UnfilteredItems;
            }

            var filteredItems = new ObservableCollection<ListItemViewModel>(UnfilteredItems.Where(x => x.Title.ToLower().Contains(Query?.Value?.ToLower()) || x.SubTitle.ToLower().Contains(Query?.Value?.ToLower())));

            Items = filteredItems;
        }));

        private ICommand favoriteClickedCommand;
        public ICommand FavoriteClickedCommand => favoriteClickedCommand ?? (favoriteClickedCommand = new Command(async (param) =>
        {
            if (!(param is ListItemViewModel item))
            {
                return;
            }

            item.IsFavorite = !item.IsFavorite;
        }));

        private ICommand sortClickedCommand;
        public ICommand SortClickedCommand => sortClickedCommand ?? (sortClickedCommand = new Command(async (param) =>
        {
            SortView sortView = new SortView();

            sortOptions = await sortView.ShowAsync(sortOptions);

            if (sortOptions.Alphabetic)
            {
                Items = new ObservableCollection<ListItemViewModel>(Items.OrderBy(x => x.Title));
            }
            else if (sortOptions.Date)
            {
                Items = new ObservableCollection<ListItemViewModel>(Items.OrderBy(x => x.Id));
            }
            else if (sortOptions.Favorite)
            {
                Items = new ObservableCollection<ListItemViewModel>(Items.OrderByDescending(x => x.IsFavorite));
            }
            else if (sortOptions.Rating)
            {
                Items = new ObservableCollection<ListItemViewModel>(Items.OrderByDescending(x => x.Rating));
            }
        }));

        private ICommand filterClickedCommand;
        public ICommand FilterClickedCommand => filterClickedCommand ?? (filterClickedCommand = new Command(async (param) =>
        {
            SearchIsVisible = false;
            Query.Value = string.Empty;

            FilterView filterView = new FilterView();

            filterOptions = await filterView.ShowAsync(filterOptions);

            var filteredItems = new ObservableCollection<ListItemViewModel>(UnfilteredItems);

            if (filterOptions.OnlyShowFavorites)
            {
                filteredItems = new ObservableCollection<ListItemViewModel>(filteredItems.Where(x => x.IsFavorite));
            }

            filteredItems = new ObservableCollection<ListItemViewModel>(filteredItems.Where(x => x.Rating >= filterOptions.RangeLowerValue && x.Rating <= filterOptions.RangeUpperValue));

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
        public ValidatableObject<string> Query { get; set; }
        public bool IsRefreshing { get; set; }
        public bool SearchIsVisible { get; set; }
        public bool FilterIsActive { get; set; }
        public bool SortingIsActive { get; set; }

        private SortOptionsViewModel sortOptions = new SortOptionsViewModel();
        private FilterOptionsViewModel filterOptions = new FilterOptionsViewModel();
    }
}
