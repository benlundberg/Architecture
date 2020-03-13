using System.Collections;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class GridViewControl : Grid
    {
        static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is GridViewControl gridView))
            {
                return;
            }

            IList enumerable = gridView.ItemsSource as IList ?? gridView.ItemsSource.Cast<object>().ToArray();

            if (gridView == null)
            {
                return;
            }
            
            gridView.Children.Clear();
            gridView.ColumnDefinitions.Clear();

            if (enumerable?.Count <= 0)
            {
                return;
            }

            gridView.NumberOfColumns = gridView.NumberOfColumns > 0 ? gridView.NumberOfColumns : 3;

            for (int i = 0; i < gridView.NumberOfColumns; i++)
            {
                if (gridView.AutoSize)
                {
                    gridView.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }
                else
                {
                    gridView.ColumnDefinitions.Add(new ColumnDefinition());
                }
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

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(IEnumerable),
            declaringType: typeof(GridViewControl),
            defaultValue: null,
            propertyChanged: HandleBindingPropertyChangedDelegate);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty = BindableProperty.Create(
            propertyName: "ItemClickCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(GridViewControl),
            defaultValue: null);

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            propertyName: "ItemTemplate",
            returnType: typeof(DataTemplate),
            declaringType: typeof(GridViewControl),
            defaultValue: null);

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

        public int NumberOfColumns { get; set; }
        public bool AutoSize { get; set; }
    }
}
