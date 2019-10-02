using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class TableControl : Grid
    {
        public TableControl()
        {
            this.ColumnSpacing = 0;
            this.RowSpacing = 0;
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

            view.Children.Clear();

            if (newItems.Any() != true)
            {
                return;
            }

            var numRows = newItems.Max(x => x.ContentItems.Count());
            var numCols = newItems.Count;

            // Create rows
            for (int i = 0; i <= numRows; i++)
            {
                view.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // Create columns
            for (int i = 0; i < numCols; i++)
            {
                view.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < newItems.Count; i++)
            {
                var item = newItems[i];

                var header = new Frame
                {
                    BackgroundColor = item.HeaderBackground,
                    BorderColor = view.UseBorder ? view.BorderColor : Color.Transparent,
                    CornerRadius = 0,
                    Padding = 4,
                    Content = new Label 
                    { 
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = item.HeaderColor,
                        Text = item.Header 
                    }
                };
                
                SetColumn(header, i);

                for (int y = 0; y < item.ContentItems.Count; y++)
                {
                    var contentItem = item.ContentItems[y];

                    var content = new Frame
                    {
                        BackgroundColor = !view.UseSecondBackgroundColor ? contentItem.Background : (y % 2) == 0 ? contentItem.Background : contentItem.SecondBackground,
                        BorderColor = view.UseBorder ? view.BorderColor : Color.Transparent,
                        CornerRadius = 0,
                        Padding = 4,
                        Content = new Label 
                        {
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = contentItem.TextColor,
                            Text = contentItem.Text 
                        }
                    };

                    SetRow(content, y + 1);
                    SetColumn(content, i);

                    view.Children.Add(content);
                }

                view.Children.Add(header);
            }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(IList<TableItem>),
            declaringType: typeof(TableControl),
            defaultValue: default(IList<TableItem>),
            propertyChanged: TableControlSourceChanged);

        public IList<TableItem> ItemsSource
        {
            get { return (IList<TableItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public bool UseBorder { get; set; } = true;
        public bool UseSecondBackgroundColor { get; set; } = true;
        public Color BorderColor { get; set; } = Color.Black;
    }

    public class TableItem
    {
        public string Header { get; set; }
        public Color HeaderColor { get; set; } = Color.White;
        public Color HeaderBackground { get; set; } = Color.Gray;
        public IList<TableContentItem> ContentItems { get; set; }
    }

    public class TableContentItem
    {
        public string Text { get; set; }
        public Color TextColor { get; set; } = Color.Black;
        public Color Background { get; set; } = Color.White;
        public Color SecondBackground { get; set; } = Color.LightGray;
    }
}
