using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class TableControl : Grid
    {
        public TableControl()
        {
            ItemTemplates = new ObservableCollection<TableViewItem>();
        }

        private void Init()
        {
            IList enumerable = ItemsSource as IList ?? ItemsSource?.Cast<object>()?.ToArray();

            // Register for itemsource change
            AddCollectionChanged(enumerable);

            // Clear added items if no items exists anymore
            if (!(enumerable?.Count > 0))
            {
                // Clear items
                if (Children.LastOrDefault() is ScrollView scroll)
                {
                    scroll.Content = null;
                }

                return;
            }

            this.ColumnSpacing = 0;
            this.RowSpacing = 0;

            this.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            this.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

            var headerGrid = new Grid()
            {
                ColumnSpacing = 0
            };

            // Adding headers
            foreach (var templateView in ItemTemplates)
            {
                if (FilterItemsSource?.Any() == true && FilterItemsSource?.Any(x => x.Id == templateView.Id && !x.IsVisible) == true)
                {
                    continue;
                }

                var headerView = GetView(templateView.HeaderTemplateSource);

                SetColumn(headerView, headerGrid.ColumnDefinitions.Count);

                headerGrid.Children.Add(headerView);

                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = ScrollOrientation == ScrollOrientation.Vertical ? GridLength.Star : GridLength.Auto });
            }

            var itemsGrid = new Grid()
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                ColumnDefinitions = headerGrid.ColumnDefinitions
            };

            // Adding items
            foreach (var item in enumerable)
            {
                var properties = item.GetType().GetProperties();

                var columnPosition = 0;

                foreach (var property in properties)
                {
                    var columnView = ItemTemplates.FirstOrDefault(x => x.Id == property.Name);

                    if (FilterItemsSource?.Any() == true && FilterItemsSource?.Any(x => x.Id == columnView.Id && !x.IsVisible) == true)
                    {
                        continue;
                    }

                    var itempropertyView = GetView(columnView.ItemTemplateSource, item);

                    itempropertyView.BindingContext = item;

                    SetRow(itempropertyView, itemsGrid.RowDefinitions.Count);
                    SetColumn(itempropertyView, columnPosition);

                    itempropertyView.BackgroundColor = StripedBackground ? (itemsGrid.RowDefinitions.Count % 2 == 0 ? SecondBackground : itempropertyView.BackgroundColor) : itempropertyView.BackgroundColor;

                    itemsGrid.Children.Add(itempropertyView);

                    columnPosition++;
                }

                itemsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            Children.Add(headerGrid);

            var scrollview = new ScrollView
            {
                Content = itemsGrid,
                Orientation = ScrollOrientation
            };

            SetRow(scrollview, 1);

            Children.Add(scrollview);
        }

        private View GetView(DataTemplate templateView, object item = null)
        {
            if (templateView == null)
            {
                return new Label
                {
                    Text = item?.ToString(),
                    Padding = new Thickness(10),
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
            }
            else if (templateView is DataTemplateSelector selector)
            {
                var template = selector.SelectTemplate(item, this);
                return template.CreateContent() as View;
            }
            else
            {
                return templateView.CreateContent() as View;
            }
        }

        private void AddCollectionChanged(IEnumerable list)
        {
            if (list is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged -= ItemsSourceCollectionChanged;
                collection.CollectionChanged += ItemsSourceCollectionChanged;
            }
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Init();
        }

        private static void ItemsSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (!(bindableObject is TableControl view))
            {
                return;
            }

            view.Init();
        }

        public static readonly BindableProperty FilterItemsSourceProperty = BindableProperty.Create(
            "FilterItemsSource",
            typeof(IEnumerable<TableFilterItem>),
            typeof(TableControl),
            null);

        public IEnumerable<TableFilterItem> FilterItemsSource
        {
            get { return (IEnumerable<TableFilterItem>)GetValue(FilterItemsSourceProperty); }
            set { SetValue(FilterItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(TableControl),
            null,
            propertyChanged: ItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private ICommand filterChangedCommand;
        public ICommand FilterChangedCommand => filterChangedCommand ?? (filterChangedCommand = new Command(() =>
        {
            Init();
        }));

        private ICommand tapGestureCommand;
        public ICommand TapGestureCommand => tapGestureCommand ?? (tapGestureCommand = new Command((param) =>
        {
            //if (!(param is TableItem item))
            //{
            //    return;
            //}

            //if (item.IsOrderedAscending)
            //{
            //    item.ContentItems = item.ContentItems.OrderByDescending(x => x.Text)?.ToList();
            //}
            //else
            //{
            //    item.ContentItems = item.ContentItems.OrderBy(x => x.Text)?.ToList();
            //}
        }));

        public ObservableCollection<TableViewItem> ItemTemplates { get; set; }
        public bool StripedBackground { get; set; } = false;
        public Color SecondBackground { get; set; } = App.Current.LightGrayColor();
        public ScrollOrientation ScrollOrientation { get; set; } = ScrollOrientation.Vertical;
    }

    public class TableViewItem
    {
        public string Id { get; set; }
        public DataTemplate HeaderTemplateSource { get; set; }
        public DataTemplate ItemTemplateSource { get; set; }
    }

    public class TableFilterItem
    {
        public string Id { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
