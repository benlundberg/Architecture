using Architecture.Core;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class CheckboxControl : StackLayout
    {
        public CheckboxControl()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.Start;
            this.VerticalOptions = LayoutOptions.Start;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (string.IsNullOrEmpty(CheckedImageSource) || string.IsNullOrEmpty(UncheckedImageSource))
            {
                InitializeDefaultView();
            }
            else
            {
                InitializeWithImageSourceView();
            }
        }

        public void InitializeDefaultView()
        {
            Grid rootGrid = new Grid()
            {
                HeightRequest = CheckboxHeight,
                WidthRequest = CheckboxWidth,
                Children =
                {
                    new BoxView() { BackgroundColor = CheckboxBackgroundColor }
                },
                Padding = new Thickness(2, 2, 2, 2),
                BackgroundColor = CheckboxForegroundColor
            };

            // Add tap
            this.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    IsChecked = !IsChecked;

                    if (IsChecked)
                    {
                        rootGrid.Children.Add(new BoxView() { BackgroundColor = CheckboxForegroundColor, Margin = new Thickness(4, 4, 4, 4) });
                    }
                    else
                    {
                        var toRemove = rootGrid.Children.LastOrDefault();
                        rootGrid.Children.Remove(toRemove);
                    }

                    CheckCommand?.Execute(null);
                })
            });

            // Add root
            this.Children.Add(rootGrid);

            if (!string.IsNullOrEmpty(Title))
            {
                this.Children.Add(new Label()
                {
                    Text = Title,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(FontSize, typeof(Label))
                });
            }

            ViewInitialized = true;
        }

        public void InitializeWithImageSourceView()
        {
            Grid rootGrid = new Grid()
            {
                HeightRequest = CheckboxHeight,
                WidthRequest = CheckboxWidth
            };

            Image backgroundImage = new Image()
            {
                Source = UncheckedImageSource
            };

            Image frontImage = new Image()
            {
                Source = UncheckedImageSource
            };

            rootGrid.Children.Add(backgroundImage);
            rootGrid.Children.Add(frontImage);

            // Add tap
            this.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    IsChecked = !IsChecked;
                    frontImage.Source = IsChecked ? CheckedImageSource : UncheckedImageSource;

                    CheckCommand?.Execute(null);
                })
            });

            // Add root
            this.Children.Add(rootGrid);

            if (!string.IsNullOrEmpty(Title))
            {
                this.Children.Add(new Label()
                {
                    Text = Title,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(FontSize, typeof(Label))
                });
            }

            ViewInitialized = true;
        }

        private static void CheckedChanged(BindableObject bindableObject, bool? oldValue, bool? newValue)
        {
            var checkBox = bindableObject as CheckboxControl;

            if (!checkBox.ViewInitialized)
            {
                if (string.IsNullOrEmpty(checkBox.CheckedImageSource) || string.IsNullOrEmpty(checkBox.UncheckedImageSource))
                {
                    checkBox.InitializeDefaultView();
                }
                else
                {
                    checkBox.InitializeWithImageSourceView();
                }
            }

            if (checkBox?.Children?.Any() == true)
            {
                try
                {
                    if (string.IsNullOrEmpty(checkBox.CheckedImageSource) || string.IsNullOrEmpty(checkBox.UncheckedImageSource))
                    {
                        if (newValue == true    )
                        {
                            (checkBox.Children.First() as Grid)?.Children.Add(new BoxView() { BackgroundColor = checkBox.CheckboxForegroundColor, Margin = new Thickness(8, 8, 8, 8) });
                        }
                        else
                        {
                            var toRemove = (checkBox.Children.First() as Grid)?.Children.LastOrDefault();
                            (checkBox.Children.First() as Grid)?.Children.Remove(toRemove);
                        }
                    }
                    else
                    {
                        ((checkBox.Children.First() as Grid)?.Children[1] as Image).Source = newValue == true ? checkBox.CheckedImageSource : checkBox.UncheckedImageSource;
                    }
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
            }
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(propertyName: "IsChecked",
            returnType: typeof(bool),
            declaringType: typeof(CheckboxControl),
            defaultValue: default(bool),
            propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                CheckedChanged(bindableObject, (bool?)oldValue, (bool?)newValue);
            });

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: "Title",
            returnType: typeof(string),
            declaringType: typeof(CheckboxControl),
            defaultValue: default(string));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty CheckCommandProperty = BindableProperty.Create(propertyName: "CheckCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(CheckboxControl),
            defaultValue: default(ICommand));

        public ICommand CheckCommand
        {
            get { return (ICommand)GetValue(CheckCommandProperty); }
            set { SetValue(CheckCommandProperty, value); }
        }

        public bool ViewInitialized { get; set; }
        public string CheckedImageSource { get; set; }
        public string UncheckedImageSource { get; set; }
        public NamedSize FontSize { get; set; }

        public Color CheckboxBackgroundColor { get; set; }
        public Color CheckboxForegroundColor { get; set; }

        private double checkboxHeight;
        public double CheckboxHeight
        {
            get { return checkboxHeight == 0 ? 20 : checkboxHeight; }
            set { checkboxHeight = value; }
        }

        private double checkboxWidth;
        public double CheckboxWidth
        {
            get { return checkboxWidth == 0 ? 20 : checkboxWidth; }
            set { checkboxWidth = value; }
        }
    }
}
