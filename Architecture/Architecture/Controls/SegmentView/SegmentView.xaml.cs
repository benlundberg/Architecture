﻿using System.Collections.ObjectModel;
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

        private void Init()
        {
            SegmentItems.Children.Clear();
            SegmentItems.RowDefinitions.Clear();
            SegmentItems.ColumnDefinitions.Clear();
            SegmentItems.ColumnSpacing = this.SectionSpacing;
            SegmentItems.RowSpacing = this.SectionSpacing;

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
                    this.MainContent = item.Content;
                }

                if (SectionOrientation == StackOrientation.Horizontal)
                {
                    SegmentItems.ColumnDefinitions.Add(new ColumnDefinition() { Width = SectionHorizontalayout.Alignment == LayoutAlignment.Fill ? GridLength.Star : GridLength.Auto });
                }
                else
                {
                    SegmentItems.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }

                SegmentItems.Children.Add(view);
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
                if (SelectedSectionBackground == Color.Transparent)
                {
                    view = new StackLayout
                    {
                        Padding = SectionPadding,
                        Children =
                        {
                            new Label
                            {
                                FontSize = FontSize,
                                Opacity = item.IsSelected ? 1 : 0.8,
                                Text = item.Text,
                                HorizontalOptions = SectionTextHorizontalLayout,
                                TextColor = item.IsSelected ? SelectedSectionTextColor : SectionTextColor
                            },
                            new BoxView
                            {
                                Color = item.IsSelected ? SelectedSectionTextColor : Color.Transparent,
                                HeightRequest = 3,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            }
                        }
                    };
                }
                else
                {
                    view = new Grid
                    {
                        Padding = SectionPadding,
                        BackgroundColor = item.IsSelected ? SelectedSectionBackground : SectionBackground,
                        Children =
                        {
                            new Label
                            {
                                HorizontalOptions = SectionTextHorizontalLayout,
                                FontSize = FontSize,
                                Text = item.Text,
                                TextColor = item.IsSelected ? SelectedSectionTextColor : SectionTextColor
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

            if (SectionOrientation == StackOrientation.Horizontal)
            {
                Grid.SetColumn(view, index);
            }
            else
            {
                Grid.SetRow(view, index);
            }

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

                SegmentItems.Children.RemoveAt(unselectIndex);
                SegmentItems.Children.Insert(unselectIndex, GetView(oldItem, unselectIndex));
            }

            item.IsSelected = true;

            SegmentItems.Children.RemoveAt(selectedIndex);
            SegmentItems.Children.Insert(selectedIndex, GetView(item, selectedIndex));

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

            ValueChangedCommand?.Execute(SelectedSegment.Tag);
        }));

        public View MainContent { get; set; }
        public SegmentControlItem SelectedSegment { get; private set; }

        public DataTemplate SelectedSectionTemplate { get; set; }
        public DataTemplate SectionTemplate { get; set; }

        public StackOrientation SectionOrientation { get; set; } = StackOrientation.Horizontal;
        public LayoutOptions SectionHorizontalayout { get; set; } = LayoutOptions.Center;
        public double SectionSpacing { get; set; }

        public Color SectionControlColor { get; set; } = Color.Transparent;
        public Thickness SectionControlMargin { get; set; }
        public Thickness SectionControlPadding { get; set; }
        public float SectionControlCornerRadius { get; set; } = 0;

        public Thickness SectionPadding { get; set; } = new Thickness(10);
        public double FontSize { get; set; } = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

        public Color SectionBackground { get; set; } = Color.Transparent;
        public Color SectionTextColor { get; set; }

        public Color SelectedSectionBackground { get; set; } = Color.Transparent;
        public Color SelectedSectionTextColor { get; set; }

        public LayoutOptions SectionTextHorizontalLayout { get; set; } = LayoutOptions.Center;
    }

    public class SegmentControlItem
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public object Tag { get; set; }
        public View Content { get; set; }
    }
}