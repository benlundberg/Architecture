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

        /// <summary>
        /// When parent is set the view is initialized
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();
            InitializeDefaultView();
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
                Children =
                {
                    new Label()
                    {
                        FontSize = Device.GetNamedSize(CheckSize, typeof(Label)),
                        TextColor = this.TextColor,
                        FontFamily = this.FontRegularFamily,
                        VerticalOptions = LayoutOptions.Center,
                        Text = UncheckedFont
                    }
                }
            };

            // Add tap to set check or unchecked
            this.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    IsChecked = !IsChecked;

                    if (IsChecked)
                    {
                        // If is checked, we add a check font
                        rootGrid.Children.Add(new Label()
                        {
                            FontSize = Device.GetNamedSize(CheckSize, typeof(Label)),
                            TextColor = this.TextColor,
                            FontFamily = this.FontFamily,
                            VerticalOptions = LayoutOptions.Center,
                            Text = CheckedFont
                        });
                    }
                    else if (rootGrid.Children.Count > 1)
                    {
                        // If unchecked, we remove that check font
                        var toRemove = rootGrid.Children.LastOrDefault();
                        rootGrid.Children.Remove(toRemove);
                    }

                    // If there is a binding on command we invoke it here
                    CheckChangedCommand?.Execute(null);
                })
            });

            // Add root
            this.Children.Add(rootGrid);


            // Add a title label if there is one
            if (!string.IsNullOrEmpty(Title))
            {
                var header = new Label()
                {
                    Text = Title,
                    TextColor = TextColor,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(FontSize, typeof(Label))
                };

                if (FlowRightToLeft)
                {
                    this.Children.Insert(0, header);
                }
                else
                {
                    this.Children.Add(header);
                }

            }

            ViewInitialized = true;
        }

        /// <summary>
        /// Invoked when there is a change for is checked
        /// </summary>
        private static void CheckedChanged(BindableObject bindableObject, bool? oldValue, bool? newValue)
        {
            if (!(bindableObject is CheckboxControl checkbox))
            {
                return;
            }

            // Fallback to really know if the view is initialized
            if (!checkbox.ViewInitialized)
            {
                checkbox.InitializeDefaultView();
            }

            // Check for children and change the view
            if (checkbox?.Children?.Any() == true)
            {
                try
                {
                    if (newValue == true)
                    {
                        (checkbox.Children.First() as Grid)?.Children.Add(new Label()
                        {
                            FontSize = Device.GetNamedSize(checkbox.CheckSize, typeof(Label)),
                            TextColor = checkbox.TextColor,
                            FontFamily = checkbox.FontFamily,
                            VerticalOptions = LayoutOptions.Center,
                            Text = checkbox.CheckedFont
                        });
                    }
                    else if ((checkbox.Children.First() as Grid)?.Children?.Count > 1)
                    {
                        var toRemove = (checkbox.Children.First() as Grid)?.Children.LastOrDefault();
                        (checkbox.Children.First() as Grid)?.Children.Remove(toRemove);
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
        public NamedSize FontSize { get; set; }
        public NamedSize CheckSize { get; set; }
        public Color CheckboxColor { get; set; }
        public Color TextColor { get; set; }
        public bool IsCircle { get; set; } = true;
        public bool IsSolid { get; set; } = true;
        public bool FlowRightToLeft { get; set; }

        private string CheckedFont => IsCircle ? "\uf058" : "\uf14a";
        private string UncheckedFont => IsCircle ? "\uf111" : "\uf0c8";
        private string FontFamily => IsSolid ? (OnPlatform<string>)Application.Current.Resources["FontAwesomeSolid"] : (OnPlatform<string>)Application.Current.Resources["FontAwesomeRegular"];
        private string FontRegularFamily => (OnPlatform<string>)Application.Current.Resources["FontAwesomeRegular"];
    }
}
