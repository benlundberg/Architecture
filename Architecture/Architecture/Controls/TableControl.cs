using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class TableControl : HorizontalView
    {
        public TableControl()
        {
        }

        private static void TableControlSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (!(bindableObject is TableControl view))
            {
                return;
            }

            if (!(newValue is IList<TableItem> newItems))
            {
                return;
            }

            if (newItems.Any() != true)
            {
                return;
            }

            var grid = new Grid()
            {
                ColumnSpacing = 0,
                RowSpacing = 0
            };

            var numRows = newItems.Max(x => x.ContentItems.Count());
            var numCols = newItems.Count;

            // Create rows
            for (int i = 0; i <= numRows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // Create columns
            for (int i = 0; i < numCols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            }

            for (int i = 0; i < newItems.Count; i++)
            {
                var item = newItems[i];

                var header = new Frame
                {
                    BackgroundColor = item.HeaderBackground,
                    BorderColor = view.UseBorder ? view.BorderColor : Color.Transparent,
                    CornerRadius = 0,
                    Padding = item.Padding,
                    Content = new Label 
                    { 
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = item.TextAlignment,
                        TextColor = item.HeaderColor,
                        Text = item.Header,
                        FontSize = Device.GetNamedSize(item.FontSize, typeof(Label))
                    }
                };

                header.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = view.TapGestureCommand,
                    CommandParameter = item
                });

                Grid.SetColumn(header, i);

                for (int y = 0; y < item.ContentItems.Count; y++)
                {
                    var contentItem = item.ContentItems[y];

                    var content = new Frame
                    {
                        BackgroundColor = !view.UseSecondBackgroundColor ? contentItem.Background : (y % 2) == 0 ? contentItem.Background : contentItem.SecondBackground,
                        BorderColor = view.UseBorder ? view.BorderColor : Color.Transparent,
                        CornerRadius = 0,
                        Padding = item.Padding,
                        Content = new Label 
                        {
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = contentItem.TextAlignment,
                            TextColor = contentItem.TextColor,
                            Text = contentItem.Text,
                            FontSize = Device.GetNamedSize(contentItem.FontSize, typeof(Label))
                        }
                    };

                    Grid.SetRow(content, y + 1);
                    Grid.SetColumn(content, i);

                    grid.Children.Add(content);
                }

                grid.Children.Add(header);
            }

            view.Content = grid;
        }

        public static readonly BindableProperty TableSourceProperty = BindableProperty.Create(
            propertyName: "TableSource",
            returnType: typeof(IList<TableItem>),
            declaringType: typeof(TableControl),
            defaultValue: default(IList<TableItem>),
            propertyChanged: TableControlSourceChanged);

        public IList<TableItem> TableSource
        {
            get { return (IList<TableItem>)GetValue(TableSourceProperty); }
            set { SetValue(TableSourceProperty, value); }
        }

        private ICommand tapGestureCommand;
        public ICommand TapGestureCommand => tapGestureCommand ?? (tapGestureCommand = new Command((param) =>
        {
            if (!(param is TableItem item))
            {
                return;
            }

            if (item.IsOrderedAscending)
            {
                item.ContentItems = item.ContentItems.OrderByDescending(x => x.Text)?.ToList();
            }
            else
            {
                item.ContentItems = item.ContentItems.OrderBy(x => x.Text)?.ToList();
            }
        }));

        public bool UseBorder { get; set; } = true;
        public bool UseSecondBackgroundColor { get; set; } = true;
        public Color BorderColor { get; set; } = Color.Black;
    }

    public class TableItem
    {
        public TableItem(string header)
        {
            Header = header;
        }

        public string Header { get; set; }
        public bool IsOrderedAscending { get; set; }
        public NamedSize FontSize { get; set; }
        public Color HeaderColor { get; set; } = Color.Black;
        public Color HeaderBackground { get; set; } = App.Current.GrayColor();
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
        public Thickness Padding { get; set; } = new Thickness(6, 14);
        public IList<TableContentItem> ContentItems { get; set; }
    }

    public class TableContentItem
    {
        public TableContentItem(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
        public Thickness Padding { get; set; } = new Thickness(6, 14);
        public NamedSize FontSize { get; set; }
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;
        public Color TextColor { get; set; } = Color.Black;
        public Color Background { get; set; } = Color.White;
        public Color SecondBackground { get; set; } = App.Current.LightGrayColor();
    }
}
