using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class ItemsControl : ScrollView
    {
        private void Init()
        {
            this.Orientation = this.ItemsOrientation == StackOrientation.Horizontal ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical;
            
            IList enumerable = ItemsSource as IList ?? ItemsSource?.Cast<object>()?.ToArray();

            // Register for itemsource change
            AddCollectionChanged(enumerable);

            // Clear added items if no items exists anymore
            if (!(enumerable?.Count > 0))
            {
                if (Content is Grid grid)
                {
                    grid.Children.Clear();
                }
                else if (Content is StackLayout stack)
                {
                    stack.Children.Clear();
                }

                return;
            }

            View content;

            if (NumberOfColumns > 0) // Grid view
            {
                content = new Grid()
                {
                    RowSpacing = this.RowSpacing,
                    ColumnSpacing = this.ColumnSpacing
                };

                var grid = content as Grid;

                for (int i = 0; i < NumberOfColumns; i++)
                {
                    if (AutoSize)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    }
                    else
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }
            }
            else // Plain list
            {
                content = new StackLayout
                {
                    Orientation = ItemsOrientation,
                    Spacing = this.Spacing
                };
            }

            int indexCol = 0;
            int indexRow = 0;
            int index = 0;

            foreach (var item in enumerable)
            {
                // Create the item view from template or template selector
                View itemContent;
                if (ItemTemplate is DataTemplateSelector selector)
                {
                    var template = selector.SelectTemplate(item, this);
                    itemContent = template.CreateContent() as View;
                }
                else
                {
                    itemContent = ItemTemplate.CreateContent() as View;
                }

                // Bind item view to item
                itemContent.BindingContext = item;

                // Add tap gesture
                itemContent.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = this.TapGestureCommand,
                    CommandParameter = item
                });

                if (content is Grid grid)
                {
                    Grid.SetColumn(itemContent, indexCol);
                    Grid.SetRow(itemContent, indexRow);

                    grid.Children.Add(itemContent);

                    if (index == enumerable.Count)
                    {
                        continue;
                    }

                    if (indexCol == (NumberOfColumns - 1))
                    {
                        indexCol = 0;
                        indexRow++;
                        grid.RowDefinitions.Add(new RowDefinition());
                    }
                    else
                    {
                        indexCol++;
                    }
                }
                else if (content is StackLayout stack)
                {
                    if (HasStripedBackground)
                    {
                        itemContent.BackgroundColor = index % 2 == 0 ? ItemsBackground : ItemsBackgroundAlternative;
                    }

                    stack.Children.Add(itemContent);
                }

                index++;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.FadeTo(0);

                this.Content = content;

                await this.FadeTo(1);
            });
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

        private static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ItemsControl view))
            {
                return;
            }

            view.Init();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(ItemsControl),
            null,
            propertyChanged: HandleBindingPropertyChangedDelegate);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty = BindableProperty.Create(
            "ItemClickCommand",
            typeof(ICommand),
            typeof(ItemsControl),
            null);

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(ItemsControl),
            null);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private ICommand tapGestureCommand;
        private ICommand TapGestureCommand
        {
            get
            {
                return tapGestureCommand ?? (tapGestureCommand = new Command<object>((item) =>
                {
                    if (this.ItemClickCommand?.CanExecute(item) == true)
                    {
                        this.ItemClickCommand.Execute(item);
                    }
                }));
            }
        }

        public double Spacing { get; set; }
        public StackOrientation ItemsOrientation { get; set; }
        public bool HasStripedBackground { get; set; }
        public Color ItemsBackground { get; set; } = Color.White;
        public Color ItemsBackgroundAlternative { get; set; } = App.Current.LightGrayColor();
        
        // For grid layout //
        public int NumberOfColumns { get; set; }
        public bool AutoSize { get; set; }
        public double RowSpacing { get; set; }
        public double ColumnSpacing { get; set; }
    }
}
