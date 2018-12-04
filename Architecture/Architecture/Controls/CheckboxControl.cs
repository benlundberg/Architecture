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
            // Set the view to horizontal and view up to the right
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.Start;
            this.VerticalOptions = LayoutOptions.Start;
        }

        /// <summary>
        /// When parent is set the view is initialized
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();

            // Check if there is an image source, otherwise we setup a default checkbox
            if (string.IsNullOrEmpty(CheckedImageSource) || string.IsNullOrEmpty(UncheckedImageSource))
            {
                InitializeDefaultView();
            }
            else
            {
                InitializeWithImageSourceView();
            }
        }

        /// <summary>
        /// Initializes the default view
        /// </summary>
        public void InitializeDefaultView()
        {
            // Return if view is already initialized
            if (ViewInitialized)
            {
                return;
            }

            // The grid who is in the bottom root
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

            // Add tap to set check or unchecked
            this.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    IsChecked = !IsChecked;

                    if (IsChecked)
                    {
                        // If is checked, we add a boxview inside the root grid
                        rootGrid.Children.Add(new BoxView()
                        {
                            BackgroundColor = CheckboxForegroundColor,
                            Margin = new Thickness(4, 4, 4, 4)
                        });
                    }
                    else
                    {
                        // If unchecked, we remove that boxview inside the root grid
                        var toRemove = rootGrid.Children.LastOrDefault();
                        rootGrid.Children.Remove(toRemove);
                    }

                    // If the user has a binding on command we invoke it here
                    CheckChangedCommand?.Execute(null);
                })
            });

            // Add root
            this.Children.Add(rootGrid);

            // Add a title label if there is one
            if (!string.IsNullOrEmpty(Title))
            {
                this.Children.Add(new Label()
                {
                    Text = Title,
                    TextColor = TextColor,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(FontSize, typeof(Label))
                });
            }

            ViewInitialized = true;
        }

        /// <summary>
        /// Initializes the view with image source view.
        /// </summary>
        public void InitializeWithImageSourceView()
        {
            if (ViewInitialized)
            {
                return;
            }

            Grid rootGrid = new Grid()
            {
                HeightRequest = CheckboxHeight,
                WidthRequest = CheckboxWidth
            };

            // For better rendering on Android, we always have the unchecked
            // image in the root
            Image backgroundImage = new Image()
            {
                Source = UncheckedImageSource
            };

            Image frontImage = new Image()
            {
                Source = UncheckedImageSource
            };

            // Add both images
            rootGrid.Children.Add(backgroundImage);
            rootGrid.Children.Add(frontImage);

            // Add tap command
            this.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    IsChecked = !IsChecked;
                    frontImage.Source = IsChecked ? CheckedImageSource : UncheckedImageSource;

                    CheckChangedCommand?.Execute(null);
                })
            });

            // Add root
            this.Children.Add(rootGrid);

            // Add a title label if there is one
            if (!string.IsNullOrEmpty(Title))
            {
                this.Children.Add(new Label()
                {
                    Text = Title,
                    TextColor = TextColor,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(FontSize, typeof(Label))
                });
            }

            ViewInitialized = true;
        }

        /// <summary>
        /// Invoked when there is a change for is checked
        /// </summary>
        private static void CheckedChanged(BindableObject bindableObject, bool? oldValue, bool? newValue)
        {
            // Check to see that it's the correct control (it will always be correct)
            if (!(bindableObject is CheckboxControl checkbox))
            {
                return;
            }

            // Fallback to really know if the view is initialized
            if (!checkbox.ViewInitialized)
            {
                if (string.IsNullOrEmpty(checkbox.CheckedImageSource) || string.IsNullOrEmpty(checkbox.UncheckedImageSource))
                {
                    checkbox.InitializeDefaultView();
                }
                else
                {
                    checkbox.InitializeWithImageSourceView();
                }
            }

            // Check for children and change the view
            if (checkbox?.Children?.Any() == true)
            {
                try
                {
                    if (string.IsNullOrEmpty(checkbox.CheckedImageSource) || string.IsNullOrEmpty(checkbox.UncheckedImageSource))
                    {
                        if (newValue == true)
                        {
                            (checkbox.Children.First() as Grid)?.Children.Add(new BoxView() { BackgroundColor = checkbox.CheckboxForegroundColor, Margin = new Thickness(8, 8, 8, 8) });
                        }
                        else
                        {
                            var toRemove = (checkbox.Children.First() as Grid)?.Children.LastOrDefault();
                            (checkbox.Children.First() as Grid)?.Children.Remove(toRemove);
                        }
                    }
                    else
                    {
                        ((checkbox.Children.First() as Grid)?.Children[1] as Image).Source = newValue == true ? checkbox.CheckedImageSource : checkbox.UncheckedImageSource;
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

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: "Title",
            returnType: typeof(string),
            declaringType: typeof(CheckboxControl),
            defaultValue: default(string));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty CheckChangedCommandProperty = BindableProperty.Create(
            propertyName: "CheckChangedCommand",
            returnType: typeof(ICommand),
            declaringType: typeof(CheckboxControl),
            defaultValue: default(ICommand));

        public ICommand CheckChangedCommand
        {
            get { return (ICommand)GetValue(CheckChangedCommandProperty); }
            set { SetValue(CheckChangedCommandProperty, value); }
        }

        public bool ViewInitialized { get; set; }
        public string CheckedImageSource { get; set; }
        public string UncheckedImageSource { get; set; }
        public NamedSize FontSize { get; set; }

        public Color CheckboxBackgroundColor { get; set; }
        public Color CheckboxForegroundColor { get; set; }
        public Color TextColor { get; set; }

        private double checkboxHeight;
        public double CheckboxHeight
        {
            get { return checkboxHeight == 0 ? string.IsNullOrEmpty(CheckedImageSource) ? 20 : 32 : checkboxHeight; }
            set { checkboxHeight = value; }
        }

        private double checkboxWidth;
        public double CheckboxWidth
        {
            get { return checkboxWidth == 0 ? string.IsNullOrEmpty(CheckedImageSource) ? 20 : 32 : checkboxWidth; }
            set { checkboxWidth = value; }
        }
    }
}
