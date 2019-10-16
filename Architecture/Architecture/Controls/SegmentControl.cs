using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class SegmentControl : Grid
    {
        public SegmentControl()
        {
            this.ItemsSource = new ObservableCollection<SegmentControlItem>();
            this.ColumnSpacing = 0;
            this.RowSpacing = 0;
        }

        private static void SegmentSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (!(bindableObject is SegmentControl view))
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

            view.InitView(view, newItems);
        }

        private void InitView(SegmentControl view, ObservableCollection<SegmentControlItem> newItems)
        {
            view.Children.Clear();
            view.ColumnDefinitions.Clear();
            view.RowDefinitions.Clear();

            if (newItems.Any() != true)
            {
                return;
            }

            if (view.Orientation == ItemsLayoutOrientation.Horizontal)
            {
                for (int i = 0; i < newItems.Count; i++)
                {
                    view.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
            else
            {
                for (int i = 0; i < newItems.Count; i++)
                {
                    view.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
            }

            int indexPos = 0;

            foreach (var item in newItems)
            {
                var segmentItemControl = view.GetView(item);

                if (view.Orientation == ItemsLayoutOrientation.Horizontal)
                {
                    SetColumn(segmentItemControl, indexPos);
                }
                else
                {
                    SetRow(segmentItemControl, indexPos);
                }

                view.Children.Add(segmentItemControl);

                indexPos++;
            }
        }

        private View GetView(SegmentControlItem item)
        {
            Grid segmentItemControl = new Grid()
            {
                Children =
                {
                    new BoxView()
                    {
                        BackgroundColor = item.Tag == StartTag ? SelectedBackgroundColor : SegmentBackgroundColor
                    },
                    new Label()
                    {
                        Text = item.Text,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = TextHorizontalOption,
                        HorizontalTextAlignment = TextAlignment,
                        FontSize = Device.GetNamedSize(FontSize, typeof(Label)),
                        TextColor = item.Tag == StartTag ? SelectedTextColor :  SegmentTextColor,
                        Margin = new Thickness(8, 8, 8, 8),
                        InputTransparent = true
                    }
                },
                Padding = new Thickness(1, 1, 1, 1),
                BackgroundColor = SelectedBackgroundColor,
            };

            item.IsSelected = item.Tag == StartTag;

            segmentItemControl.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TapGestureCommand,
                CommandParameter = item
            });

            return segmentItemControl;
        }

        private void NewItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InitView(this, ItemsSource);
        }

        private void UpdateSelectedSegmentLayout(SegmentControlItem item)
        {
            // Unmark old value
            var oldItem = ItemsSource.FirstOrDefault(x => x.IsSelected);

            if (oldItem != null)
            {
                oldItem.IsSelected = false;

                Grid oldSelectedView = (Grid)Children.FirstOrDefault(x => (x as Grid)?.Children?.Any(c => (c as Label)?.Text == oldItem.Text) == true);

                if (oldSelectedView != null)
                {
                    oldSelectedView.Children[0].BackgroundColor = SegmentBackgroundColor;
                    (oldSelectedView.Children[1] as Label).TextColor = SegmentTextColor;
                }
            }

            // Mark new value
            item.IsSelected = true;

            Grid newSelectedView = (Grid)Children.FirstOrDefault(x => (x as Grid)?.Children?.Any(c => (c as Label)?.Text == item.Text) == true);

            if (newSelectedView != null)
            {
                newSelectedView.Children[0].BackgroundColor = SelectedBackgroundColor;
                (newSelectedView.Children[1] as Label).TextColor = SelectedTextColor;
            }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(ObservableCollection<SegmentControlItem>),
            declaringType: typeof(SegmentControl),
            defaultValue: default(ObservableCollection<SegmentControlItem>),
            propertyChanged: SegmentSourceChanged);

        public ObservableCollection<SegmentControlItem> ItemsSource
        {
            get { return (ObservableCollection<SegmentControlItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
            propertyName: "ValueChangedCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(SegmentControl),
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

            ValueChangedCommand?.Execute(SelectedSegment.Tag);
        }));

        public static readonly BindableProperty StartTagProperty = BindableProperty.Create(
            propertyName: "StartTag",
            returnType: typeof(object),
            declaringType: typeof(SegmentControl),
            defaultValue: default);

        public object StartTag
        {
            get { return GetValue(StartTagProperty); }
            set { SetValue(StartTagProperty, value); }
        }

        public SegmentControlItem SelectedSegment { get; private set; }
        public Color SegmentTextColor { get; set; } = Application.Current.PrimaryColor();
        public Color SegmentBackgroundColor { get; set; } = Color.White;
        public Color SelectedTextColor { get; set; } = Color.White;
        public Color SelectedBackgroundColor { get; set; } = Application.Current.PrimaryColor();
        public NamedSize FontSize { get; set; }
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
        public LayoutOptions TextHorizontalOption { get; set; } = LayoutOptions.Center;
        public ItemsLayoutOrientation Orientation { get; set; } = ItemsLayoutOrientation.Horizontal;
    }

    public class SegmentControlItem
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public object Tag { get; set; }
    }
}
