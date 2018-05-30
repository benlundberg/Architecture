using System.Collections;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class GridView : Grid
    {
        public GridView()
        {
        }

        static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            var gridView = bindable as GridView;

            IList enumerable = gridView.ItemsSource as IList ?? gridView.ItemsSource.Cast<object>().ToArray();

            if (gridView == null || enumerable?.Count <= 0)
            {
                return;
            }

            gridView.Children.Clear();

            gridView.NumberOfColumns = gridView.NumberOfColumns > 0 ? gridView.NumberOfColumns : 3;

            for (int i = 0; i < (gridView.NumberOfColumns - 1); i++)
            {
                gridView.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int indexCol = 0;
            int indexRow = 0;
            int index = 0;

            foreach (var item in enumerable)
            {
                var content = gridView.ItemTemplate.CreateContent() as View;
                content.BindingContext = item;

                TapGestureRecognizer tap = new TapGestureRecognizer
                {
                    Command = gridView.TapGestureCommand,
                    CommandParameter = item
                };

                content.GestureRecognizers.Add(tap);

                SetColumn(content, indexCol);
                SetRow(content, indexRow);

                gridView.Children.Add(content);

                if (index == enumerable.Count)
                {
                    continue;
                }

                if (indexCol == (gridView.NumberOfColumns - 1))
                {
                    indexCol = 0;
                    indexRow++;
                    gridView.RowDefinitions.Add(new RowDefinition());
                }
                else
                {
                    indexCol++;
                }

                index++;
            }
        }

        /* Bindable properties */

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(GridView), null, propertyChanged: HandleBindingPropertyChangedDelegate);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty =
            BindableProperty.Create("ItemClickCommand", typeof(ICommand), typeof(GridView), null);

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(GridView), null);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /* Properties */

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

        public int NumberOfColumns
        {
            get;
            set;
        }
    }
}
