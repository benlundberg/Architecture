using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class RatingControl : StackLayout
    {
        public RatingControl()
        {
            Orientation = StackOrientation.Horizontal;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            Children.Clear();

            for (int i = 0; i < RateMaximum; i++)
            {
                var label = new Label
                {
                    Text = Text,
                    FontFamily = FontFamily,
                    FontSize = Device.GetNamedSize(FontSize, typeof(Label)),
                    TextColor = i <= Rating ? SolidColor : Color
                };

                if (!IsLocked)
                {
                    label.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = Command,
                        CommandParameter = i
                    });

                    var panGesture = new PanGestureRecognizer();
                    panGesture.PanUpdated += PanGesture_PanUpdated;

                    label.GestureRecognizers.Add(panGesture);
                }

                Children.Add(label);
            }

            if (Rating > 0)
            {
                Command?.Execute(Rating - 1);
            }
        }

        private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs eventArgs)
        {
            double width = (Children.First() as Label)?.Width ?? 0;

            if (width <= 0)
            {
                return;
            }

            if (eventArgs.TotalX > (width * 4))
            {
                Command?.Execute(4);
            }
            else if (eventArgs.TotalX > (width * 3))
            {
                Command?.Execute(3);
            }
            else if (eventArgs.TotalX > (width * 2))
            {
                Command?.Execute(2);
            }
            else if (eventArgs.TotalX > width)
            {
                Command?.Execute(1);
            }
            else if (eventArgs.TotalX > 0)
            {
                Command?.Execute(0);
            }
        }

        private ICommand command;
        public ICommand Command => command ?? (command = new Command((param) =>
        {
            if (!int.TryParse(param?.ToString(), out int rating))
            {
                return;
            }

            if (Children?.Any() != true)
            {
                return;
            }

            for (int i = 0; i < RateMaximum; i++)
            {
                var label = Children[i] as Label;

                label.TextColor = i <= rating ? SolidColor : Color;
            }

            this.Rating = (rating + 1);
        }));

        public static readonly BindableProperty RatingProperty = BindableProperty.Create(
            propertyName: "Rating",
            returnType: typeof(int),
            declaringType: typeof(RatingControl),
            defaultValue: default(int),
            propertyChanged: RatingChanged);

        private static void RatingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RatingControl view))
            {
                return;
            }

            if (!(int.TryParse(newValue?.ToString(), out int rate)))
            {
                return;
            }

            view.Command?.Execute((rate - 1));
        }

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public int RateMaximum { get; set; } = 5;
        public string Text { get; set; } = "\uf005";
        public NamedSize FontSize { get; set; } = NamedSize.Medium;
        public string FontFamily { get; set; } = App.Current.FontAwesomeSolid();
        public Color SolidColor { get; set; } = App.Current.AccentColor();
        public Color Color { get; set; } = Color.LightGray;
        public bool IsLocked { get; set; }
    }
}
