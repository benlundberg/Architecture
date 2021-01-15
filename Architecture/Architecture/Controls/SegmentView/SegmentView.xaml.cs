using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    public enum SegmentedControlMode
    {
        Text,
        Rectangle,
        Round
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentView : ContentView
    {
        public SegmentView()
        {
            InitializeComponent();

            ItemsSource = new ObservableCollection<SegmentControlItem>();
        }

        private void Init()
        {
            SegmentItemsGrid.Children.Clear();
            SegmentItemsGrid.ColumnDefinitions.Clear();
            SegmentItemsGrid.ColumnSpacing = 0;

            if (SegmentedControlMode == SegmentedControlMode.Round)
            {
                RootFrame.CornerRadius = 16f;
                RootFrame.BackgroundColor = Color.White;
            }
            else if (SegmentedControlMode == SegmentedControlMode.Text)
            {
                RootFrame.BackgroundColor = Color.Transparent;
            }
            else if (SegmentedControlMode == SegmentedControlMode.Rectangle)
            {
                RootFrame.CornerRadius = 0f;
                RootFrame.BackgroundColor = Color.White;
            }

            if (ItemsSource?.Any() != true)
            {
                return;
            }

            for (int i = 0; i < ItemsSource.Count; i++)
            {
                var item = ItemsSource[i];

                // Check if selected
                item.IsSelected = item.IsSelected || (SelectedTag != null && SelectedTag?.ToString() == item.Tag?.ToString());

                var view = GetView(item, i);

                // If selected we display content
                if (item.IsSelected)
                {
                    SelectedSegment = item;
                    SelectedTag = item.Tag;

                    this.MainContent = item.Content;
                }

                SegmentItemsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = SegmentRowWidth });

                SegmentItemsGrid.Children.Add(view);
            }
        }

        private View GetView(SegmentControlItem item, int index)
        {
            // Get view
            View view;

            if (SectionTemplate != null && SelectedSectionTemplate != null)
            {
                view = item.IsSelected ? SelectedSectionTemplate.CreateContent() as View : SectionTemplate.CreateContent() as View;
            }
            else
            {
                if (SegmentedControlMode == SegmentedControlMode.Text)
                {
                    view = new StackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                FontSize = 14,
                                FontFamily = App.Current.Get<string>("SegmentControlFont"),
                                Opacity = item.IsSelected ? 1 : 0.6,
                                Text = item.Text,
                                Padding = new Thickness(14, 14, 14, 0),
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                TextColor = item.IsSelected ? App.Current.PrimaryColor() : Color.Black,
                                MaxLines = 1,
                                LineBreakMode = LineBreakMode.TailTruncation
                            },
                            new BoxView
                            {
                                Color = item.IsSelected ? App.Current.PrimaryColor() : Color.Black,
                                HeightRequest = item.IsSelected ? 1.33 : 1,
                                Opacity = item.IsSelected ? 1 : 0.12,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            }
                        }
                    };
                }
                else if (SegmentedControlMode == SegmentedControlMode.Rectangle)
                {
                    view = new Grid
                    {
                        Children =
                        {
                            new BoxView
                            {
                                Color = item.IsSelected ? App.Current.PrimaryColor() : Color.White,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            },
                            new Label
                            {
                                FontSize = 14,
                                Padding = new Thickness(14),
                                FontFamily = App.Current.Get<string>("SegmentControlFont"),
                                Text = item.Text,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                TextColor = item.IsSelected ? Color.White : Color.Black,
                                MaxLines = 1,
                                LineBreakMode = LineBreakMode.TailTruncation
                            }
                        }
                    };
                }
                else
                {
                    view = new Grid
                    {
                        Children =
                        {
                            new BoxView
                            {
                                CornerRadius = new CornerRadius(16),
                                Color = item.IsSelected ? App.Current.PrimaryColor() : Color.White,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            },
                            new Label
                            {
                                FontSize = 14,
                                FontFamily = App.Current.Get<string>("SegmentControlFont"),
                                Text = item.Text,
                                Padding = new Thickness(14),
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                TextColor = item.IsSelected ? Color.White : Color.Black,
                                MaxLines = 1,
                                LineBreakMode = LineBreakMode.TailTruncation
                            }
                        }
                    };
                }
            }

            view.BindingContext = item;

            // Adding tap gesture
            view.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TapGestureCommand,
                CommandParameter = item
            });

            Grid.SetColumn(view, index);

            return view;
        }

        private void UpdateSelectedSegmentLayout(SegmentControlItem item)
        {
            // Get old item
            var oldItem = ItemsSource.FirstOrDefault(x => x.IsSelected);

            var selectedIndex = ItemsSource.IndexOf(item);
            var unselectIndex = oldItem == null ? -1 : ItemsSource.IndexOf(oldItem);

            if (unselectIndex != -1)
            {
                oldItem.IsSelected = false;

                SegmentItemsGrid.Children.RemoveAt(unselectIndex);
                SegmentItemsGrid.Children.Insert(unselectIndex, GetView(oldItem, unselectIndex));
            }

            item.IsSelected = true;

            SegmentItemsGrid.Children.RemoveAt(selectedIndex);
            SegmentItemsGrid.Children.Insert(selectedIndex, GetView(item, selectedIndex));

            Device.BeginInvokeOnMainThread(() =>
            {
                this.MainContent = item.Content;
            });
        }

        private static void SegmentSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is SegmentView view))
            {
                return;
            }

            if (!(newValue is ObservableCollection<SegmentControlItem> newItems))
            {
                return;
            }

            if (oldValue is ObservableCollection<SegmentControlItem> oldItems)
            {
                // Unsubscribe
                oldItems.CollectionChanged -= view.NewItems_CollectionChanged;
            }

            newItems.CollectionChanged += view.NewItems_CollectionChanged;

            view.Init();
        }

        private void NewItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Init();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(ObservableCollection<SegmentControlItem>),
            declaringType: typeof(SegmentView),
            defaultValue: default(ObservableCollection<SegmentControlItem>),
            propertyChanged: SegmentSourceChanged);

        public ObservableCollection<SegmentControlItem> ItemsSource
        {
            get { return (ObservableCollection<SegmentControlItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedTagProperty = BindableProperty.Create(
            propertyName: "SelectedTag",
            returnType: typeof(object),
            declaringType: typeof(SegmentView),
            defaultValue: default,
            propertyChanged: SelectedTagChanged);

        private static void SelectedTagChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is SegmentView view))
            {
                return;
            }

            if (newValue == null)
            {
                return;
            }

            var selectedItem = view.ItemsSource?.FirstOrDefault(x => x.Tag?.ToString() == newValue?.ToString());

            view.TapGestureCommand?.Execute(selectedItem);
        }

        public object SelectedTag
        {
            get { return GetValue(SelectedTagProperty); }
            set { SetValue(SelectedTagProperty, value); }
        }

        public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
            propertyName: "ValueChangedCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(SegmentView),
            defaultValue: default(ICommand));

        public ICommand ValueChangedCommand
        {
            get { return (ICommand)GetValue(ValueChangedCommandProperty); }
            set { SetValue(ValueChangedCommandProperty, value); }
        }

        private ICommand tapGestureCommand;
        public ICommand TapGestureCommand => tapGestureCommand ?? (tapGestureCommand = new Command<object>((param) =>
        {
            if (!(param is SegmentControlItem segmentControlItem))
            {
                return;
            }

            if (SelectedSegment?.Tag == segmentControlItem.Tag)
            {
                return;
            }

            UpdateSelectedSegmentLayout(segmentControlItem);

            SelectedSegment = segmentControlItem;

            SelectedTag = SelectedSegment.Tag;

            ValueChangedCommand?.Execute(SelectedSegment.Tag);
        }));

        public View MainContent { get; set; }
        public SegmentControlItem SelectedSegment { get; private set; }

        public DataTemplate SelectedSectionTemplate { get; set; }
        public DataTemplate SectionTemplate { get; set; }

        public SegmentedControlMode SegmentedControlMode { get; set; }
        public Thickness SegmentMargin { get; set; } = new Thickness(24, 16);
        public GridLength SegmentRowWidth { get; set; } = GridLength.Star;
        public LayoutOptions SegmentHorizontalOptions { get; set; } = LayoutOptions.Fill;
    }

    public class SegmentControlItem
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public object Tag { get; set; }
        public View Content { get; set; }
    }
}