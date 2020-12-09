using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class TilesMenu : Grid
    {
        public TilesMenu()
        {
        }

        static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is TilesMenu view))
            {
                return;
            }

            if (view == null)
            {
                return;
            }

            view.Children.Clear();
            view.ColumnDefinitions.Clear();

            if (view.ItemsSource?.Count() <= 0)
            {
                return;
            }

            view.NumberOfColumns = view.NumberOfColumns > 0 ? view.NumberOfColumns : 3;

            for (int i = 0; i < view.NumberOfColumns; i++)
            {
                if (view.AutoSize)
                {
                    view.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }
                else
                {
                    view.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }

            int indexCol = 0;
            int indexRow = 0;
            int index = 0;

            foreach (var item in view.ItemsSource)
            {
                var content = new Grid
                {
                    HeightRequest = item.Height,
                    BackgroundColor = item.Background
                };

                if (item.HasImageBackground)
                {
                    content.Children.Add(new Image
                    {
                        Aspect = Aspect.AspectFill,
                        Source = item.ImageBackground
                    });

                    content.Children.Add(new Grid
                    {
                        BackgroundColor = Color.Black,
                        Opacity = 0.4
                    });
                }

                if (item.HasIcon)
                {
                    content.Children.Add(new Label
                    {
                        Text = item.IconSource,
                        TextColor = item.IconColor,
                        FontFamily = item.IconFontFamily,
                        FontSize = item.IconFontSize,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    });
                }

                if (!string.IsNullOrEmpty(item.Title))
                {
                    content.Children.Add(new Label
                    {
                        Text = item.Title,
                        FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                        TextColor = item.TitleColor,
                        VerticalOptions = LayoutOptions.End,
                        Margin = new Thickness(10)
                    });
                }

                if (item.HasNotification)
                {
                    content.Children.Add(new Frame
                    {
                        Padding = new Thickness(1),
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.Start,
                        HeightRequest = 24,
                        WidthRequest = 24,
                        CornerRadius = 12,
                        Margin = new Thickness(10),
                        BorderColor = item.NotificationBackground,
                        BackgroundColor = item.NotificationBackground,
                        Content = new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                            TextColor = item.NotificationTextColor,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Text = item.NotificationText
                        }
                    });
                }

                if (item.InfoText?.Any() == true)
                {
                    StackLayout infoStack = new StackLayout()
                    {
                        Spacing = 10,
                        Margin = new Thickness(8)
                    };

                    foreach (var infoText in item.InfoText)
                    {
                        infoStack.Children.Add(new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                            Text = infoText,
                            TextColor = item.InfoTextColor
                        });
                    }

                    content.Children.Add(infoStack);
                }

                TapGestureRecognizer tap = new TapGestureRecognizer
                {
                    Command = view.TapGestureCommand,
                    CommandParameter = item
                };

                content.GestureRecognizers.Add(tap);

                SetColumn(content, indexCol);
                SetRow(content, indexRow);

                view.Children.Add(content);

                if (index == view.ItemsSource.Count())
                {
                    continue;
                }

                if (indexCol == (view.NumberOfColumns - 1))
                {
                    indexCol = 0;
                    indexRow++;
                    view.RowDefinitions.Add(new RowDefinition());
                }
                else
                {
                    indexCol++;
                }

                index++;
            }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(ObservableCollection<TileItem>),
            declaringType: typeof(TilesMenu),
            defaultValue: null,
            propertyChanged: HandleBindingPropertyChangedDelegate);

        public ObservableCollection<TileItem> ItemsSource
        {
            get { return (ObservableCollection<TileItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty = BindableProperty.Create(
            propertyName: "ItemClickCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(TilesMenu),
            defaultValue: null);

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
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

        public int NumberOfColumns { get; set; }
        public bool AutoSize { get; set; }
    }

    public class TileItem
    {
        public object Tag { get; set; }
        public int Height { get; set; } = 150;
        public Color Background { get; set; } = Color.Transparent;
        public string ImageBackground { get; set; }
        public bool HasImageBackground => !string.IsNullOrEmpty(ImageBackground);
        public string Title { get; set; }
        public Color TitleColor { get; set; } = Color.White;
        public string IconSource { get; set; }
        public string IconFontFamily { get; set; } = App.Current.FontAwesomeSolid();
        public bool HasIcon => !string.IsNullOrEmpty(IconSource);
        public double IconFontSize { get; set; } = 48;
        public Color IconColor { get; set; } = Color.White;
        public string NotificationText { get; set; }
        public Color NotificationBackground { get; set; } = App.Current.AccentColor();
        public Color NotificationTextColor { get; set; } = Color.White;
        public bool HasNotification => !string.IsNullOrEmpty(NotificationText);
        public IList<string> InfoText { get; set; }
        public Color InfoTextColor { get; set; } = Color.White;
    }
}
