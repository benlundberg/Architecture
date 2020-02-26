using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentView : ContentView
    {
        public SegmentView()
        {
            InitializeComponent();
            ItemsSource = new ObservableCollection<SegmentControlItem>();
        }

        private void InitView()
        {
            if (UserBorder)
            {
                segmentControlFrame.Margin = 20;
                segmentControlFrame.CornerRadius = 10;
            }
            else
            {
                segmentControlFrame.BackgroundColor = Color.Transparent;
                segmentControlFrame.BorderColor = Color.Transparent;
                segmentControlFrame.CornerRadius = 0;
                segmentControlFrame.HasShadow = false;
            }

            segmentControlItems.Children.Clear();
            segmentControlItems.ColumnDefinitions.Clear();

            if (ItemsSource?.Any() != true)
            {
                return;
            }

            for (int i = 0; i < ItemsSource.Count; i++)
            {
                if (UserBorder)
                {
                    segmentControlItems.ColumnDefinitions.Add(new ColumnDefinition());
                }
                else
                {
                    segmentControlItems.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }

                var item = ItemsSource[i];

                var view = GetView(item);

                Grid.SetColumn(view, i);

                segmentControlItems.Children.Add(view);
            }
        }

        private View GetView(SegmentControlItem item)
        {
            // Check if item should be selected
            var isSelected =
                item.IsSelected ||
                (SelectedTag != null && SelectedTag?.ToString() == item.Tag?.ToString());

            Grid view = new Grid()
            {
                Padding = new Thickness(1)
            };

            if (UserBorder)
            {
                view.Children.Add(new BoxView
                {
                    BackgroundColor = isSelected ? SelectedBackgroundColor : SegmentBackgroundColor
                });
            }

            var label = new Label
            {
                Text = item.Text,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = TextAlignment,
                FontSize = Device.GetNamedSize(FontSize, typeof(Label)),
                TextColor = isSelected ? SelectedTextColor : SegmentTextColor,
                Margin = new Thickness(8, 8, 8, 8),
                InputTransparent = true
            };

            if (!UserBorder)
            {
                view.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                view.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                BoxView boxView = new BoxView
                {
                    HeightRequest = 4,
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.Fill,
                    Color = isSelected ? SelectedBackgroundColor : Color.Transparent
                };

                Grid.SetRow(boxView, 1);
                Grid.SetRow(label, 0);

                view.Children.Add(boxView);
            }

            view.Children.Add(label);

            item.IsSelected = isSelected;

            if (item.IsSelected)
            {
                this.MainContent = item.Content;
            }

            view.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TapGestureCommand,
                CommandParameter = item
            });

            return view;
        }

        private void UpdateSelectedSegmentLayout(SegmentControlItem item)
        {
            // Unmark old value
            var oldItem = ItemsSource.FirstOrDefault(x => x.IsSelected);

            if (oldItem != null)
            {
                oldItem.IsSelected = false;

                Grid oldSelectedView = (Grid)segmentControlItems.Children.FirstOrDefault(x => (x as Grid)?.Children?.Any(c => (c as Label)?.Text == oldItem.Text) == true);

                if (oldSelectedView != null)
                {
                    if (UserBorder)
                    {
                        oldSelectedView.Children[0].BackgroundColor = SegmentBackgroundColor;
                        (oldSelectedView.Children[1] as Label).TextColor = SegmentTextColor;
                    }
                    else
                    {
                        (oldSelectedView.Children[0] as BoxView).Color = Color.Transparent;
                        (oldSelectedView.Children[1] as Label).TextColor = SegmentTextColor;
                    }
                }
            }

            // Mark new value
            item.IsSelected = true;

            Grid newSelectedView = (Grid)segmentControlItems.Children.FirstOrDefault(x => (x as Grid)?.Children?.Any(c => (c as Label)?.Text == item.Text) == true);

            if (newSelectedView != null)
            {
                if (UserBorder)
                {
                    newSelectedView.Children[0].BackgroundColor = SelectedBackgroundColor;
                    (newSelectedView.Children[1] as Label).TextColor = SelectedTextColor;
                }
                else
                {
                    (newSelectedView.Children[0] as BoxView).Color = SelectedBackgroundColor;
                    (newSelectedView.Children[1] as Label).TextColor = SelectedTextColor;
                }
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await MainContent.FadeTo(0, length: 150);

                this.MainContent = item.Content;

                await MainContent.FadeTo(1, length: 150);
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

            view.InitView();
        }

        private void NewItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InitView();
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

            ValueChangedCommand?.Execute(SelectedSegment.Tag);
        }));

        public View MainContent { get; set; }
        public SegmentControlItem SelectedSegment { get; private set; }
        public Color SegmentTextColor { get; set; } = Application.Current.AccentColor();
        public Color SegmentBackgroundColor { get; set; } = Color.White;
        public Color SelectedTextColor { get; set; } = Color.White;
        public Color SelectedBackgroundColor { get; set; } = Application.Current.AccentColor();
        public NamedSize FontSize { get; set; } = NamedSize.Medium;
        public LayoutOptions TextAlignment { get; set; } = LayoutOptions.Center;
        public bool UserBorder { get; set; } = true;
    }

    public class SegmentControlItem
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public object Tag { get; set; }
        public View Content { get; set; }
    }
}