using System.Collections;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class HorizontalView : ScrollView
    {
        public HorizontalView()
        {
            this.Orientation = ScrollOrientation.Horizontal;
            this.VerticalOptions = LayoutOptions.Start;
        }

        static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as HorizontalView;

            IList enumerable = view.ItemsSource as IList ?? view.ItemsSource.Cast<object>().ToArray();

            if (view == null || enumerable?.Count <= 0)
            {
                return;
            }

            StackLayout stack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start
            };

            foreach (var item in enumerable)
            {
                var content = view.ItemTemplate.CreateContent() as View;
                content.BindingContext = item;

                TapGestureRecognizer tap = new TapGestureRecognizer
                {
                    Command = view.TapGestureCommand,
                    CommandParameter = item
                };

                content.GestureRecognizers.Add(tap);

                stack.Children.Add(content);
            }

            view.Content = stack;
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(HorizontalView), null, propertyChanged: HandleBindingPropertyChangedDelegate);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemClickCommandProperty =
            BindableProperty.Create("ItemClickCommand", typeof(ICommand), typeof(HorizontalView), null);

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(HorizontalView), null);

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
    }
}
