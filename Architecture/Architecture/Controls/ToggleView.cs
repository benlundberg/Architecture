using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class ToggleView : ContentView
    {
        protected override void OnParentSet()
        {
            base.OnParentSet();

            SetView();
        }

        private void SetView()
        {
            this.Content = IsChecked ? CheckedView : UncheckedView;
            
            this.Content.GestureRecognizers.Clear();

            this.Content.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = ViewTappedCommand
            });
        }

        private static void IsCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ToggleView view))
            {
                return;
            }

            if (!(newValue is bool isChecked))
            {
                return;
            }

            view.SetView();

            view.CheckedChangedCommand?.Execute(isChecked);
        }

        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create(
                propertyName: "IsChecked",
                returnType: typeof(bool),
                declaringType: typeof(ToggleView),
                defaultValue: false,
                propertyChanged: IsCheckedPropertyChanged);

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly BindableProperty CheckedChangedCommandProperty =
            BindableProperty.Create(
                propertyName: "CheckedChangedCommand",
                returnType: typeof(ICommand),
                declaringType: typeof(ToggleView),
                defaultValue: default(ICommand));

        public ICommand CheckedChangedCommand
        {
            get { return (ICommand)GetValue(CheckedChangedCommandProperty); }
            set { SetValue(CheckedChangedCommandProperty, value); }
        }

        public View CheckedView { get; set; }
        public View UncheckedView { get; set; }

        private ICommand viewTappedCommand;
        public ICommand ViewTappedCommand => viewTappedCommand ?? (viewTappedCommand = new Command(() =>
        {
            IsChecked = !IsChecked;
        }));
    }
}
