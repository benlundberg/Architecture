using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class SegmentControl : StackLayout
    {
        public SegmentControl()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.Center;
            this.VerticalOptions = LayoutOptions.Center;
            this.Spacing = 0;
        }

        private static void SegmentSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (bindableObject is SegmentControl segmentControl)
            {
                if (segmentControl.SegmentControlSource?.Any() != true)
                {
                    return;
                }

                for (int i = 0; i < segmentControl.SegmentControlSource.Count; i++)
                {
                    Grid segmentItemControl = new Grid()
                    {
                        Children =
                        {
                            new BoxView()
                            {
                                BackgroundColor = segmentControl.SegmentBackgroundColor
                            },
                            new Label()
                            {
                                Text = segmentControl.SegmentControlSource[i].Text,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                FontSize = Device.GetNamedSize(segmentControl.FontSize, typeof(Label)),
                                TextColor =  segmentControl.SegmentTextColor,
                                Margin = new Thickness(8, 8, 8, 8),
                                InputTransparent = true
                            }
                        },
                        Padding = new Thickness(1, 1, 1, 1),
                        BackgroundColor = segmentControl.SelectedBackgroundColor
                    };

                    segmentItemControl.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = segmentControl.TapGestureCommand,
                        CommandParameter = i
                    });

                    segmentControl.Children.Add(segmentItemControl);
                }

                segmentControl.UpdateSelectedSegmentLayout(0, 0);
            }
        }

        private void UpdateSelectedSegmentLayout(int oldValue, int newValue)
        {
            // Unmark old value
            if (Children[oldValue] is Grid oldSelected)
            {
                oldSelected.Children[0].BackgroundColor = SegmentBackgroundColor;
                (oldSelected.Children[1] as Label).TextColor = SegmentTextColor;
            }

            // Mark new value
            if (Children[newValue] is Grid newSelected)
            {
                newSelected.Children[0].BackgroundColor = SelectedBackgroundColor;
                (newSelected.Children[1] as Label).TextColor = SelectedTextColor;
            }
        }

        public static readonly BindableProperty SegmentControlSourceProperty = BindableProperty.Create(
            propertyName: "SegmentControlSource",
            returnType: typeof(ObservableCollection<SegmentedControlOption>),
            declaringType: typeof(SegmentControl),
            defaultValue: default(ObservableCollection<SegmentedControlOption>),
            propertyChanged: SegmentSourceChanged);

        public ObservableCollection<SegmentedControlOption> SegmentControlSource
        {
            get { return (ObservableCollection<SegmentedControlOption>)GetValue(SegmentControlSourceProperty); }
            set { SetValue(SegmentControlSourceProperty, value); }
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
            if (SelectedSegment == (int)param)
            {
                return;
            }

            UpdateSelectedSegmentLayout(SelectedSegment, (int)param);

            SelectedSegment = (int)param;

            ValueChangedCommand?.Execute(SegmentControlSource[SelectedSegment]?.Tag);
        }));

        public int SelectedSegment { get; private set; }

        public Color SegmentTextColor { get; set; }
        public Color SegmentBackgroundColor { get; set; }
        public Color SelectedTextColor { get; set; }
        public Color SelectedBackgroundColor { get; set; }

        public NamedSize FontSize { get; set; }
    }

    public class SegmentedControlOption : View
    {
        public static BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(string),
            declaringType: typeof(SegmentedControlOption),
            defaultValue: default(string));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public object Tag { get; set; }
    }
}
