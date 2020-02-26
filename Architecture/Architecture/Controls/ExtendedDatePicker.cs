using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.Controls
{
    public class ExtendedDatePicker : Grid
    {
        protected override void OnParentSet()
        {
            base.OnParentSet();

            Init();
        }

        private void Init()
        {
            if (ShowLabel)
            {
                this.Children.Add(new Label
                {
                    Text = SelectedDate.ToShortDateString(),
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.Black
                });
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Children.Add(new CustomDatePicker_iOS(this));
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                this.Children.Add(new CustomDatePicker_Droid(this));
            }
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: "Title",
            returnType: typeof(string),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: default(string));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty HasDayProperty = BindableProperty.Create(
            propertyName: "HasDay",
            returnType: typeof(bool),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: true);

        public bool HasDay
        {
            get { return (bool)GetValue(HasDayProperty); }
            set { SetValue(HasDayProperty, value); }
        }

        public static readonly BindableProperty HasMonthProperty = BindableProperty.Create(
            propertyName: "HasMonth",
            returnType: typeof(bool),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: true);

        public bool HasMonth
        {
            get { return (bool)GetValue(HasMonthProperty); }
            set { SetValue(HasMonthProperty, value); }
        }

        public static readonly BindableProperty HasYearProperty = BindableProperty.Create(
            propertyName: "HasYear",
            returnType: typeof(bool),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: true);

        public bool HasYear
        {
            get { return (bool)GetValue(HasYearProperty); }
            set { SetValue(HasYearProperty, value); }
        }

        public static readonly BindableProperty MaxDateProperty = BindableProperty.Create(
            propertyName: "MaxDate",
            returnType: typeof(DateTime?),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: default(DateTime?));

        public DateTime? MaxDate
        {
            get { return (DateTime?)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public static readonly BindableProperty MinDateProperty = BindableProperty.Create(
            propertyName: "MinDate",
            returnType: typeof(DateTime?),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: default(DateTime?));

        public DateTime? MinDate
        {
            get { return (DateTime?)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(
            propertyName: "SelectedDate",
            returnType: typeof(DateTime),
            declaringType: typeof(ExtendedDatePicker),
            defaultValue: default(DateTime),
            propertyChanged: DatePropertyChanged);

        private static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ExtendedDatePicker picker))
            {
                return;
            }

            if (!(newValue is DateTime date))
            {
                return;
            }

            picker.DateChanged(date);
        }

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly BindableProperty SelectedDateChangedCommandProperty = BindableProperty.Create(
           propertyName: "SelectedDateChangedCommand",
           returnType: typeof(ICommand),
           declaringType: typeof(ExtendedDatePicker),
           defaultValue: default(ICommand));

        public ICommand SelectedDateChangedCommand
        {
            get { return (ICommand)GetValue(SelectedDateChangedCommandProperty); }
            set { SetValue(SelectedDateChangedCommandProperty, value); }
        }

        public void DateChanged(DateTime date)
        {
            SelectedDate = date;
            SelectedDateChangedCommand?.Execute(date);

            if (this.Children.Where(x => x is Label)?.FirstOrDefault() is Label label)
            {
                label.Text = date.ToShortDateString();
            }
        }

        public bool ShowLabel { get; set; } = true;
    }

    public class CustomDatePicker_iOS : Picker
    {
        public CustomDatePicker_iOS(ExtendedDatePicker datePicker)
        {
            this.DatePicker = datePicker;

            this.TextColor = Color.Transparent;
            this.BackgroundColor = Color.Transparent;
        }

        public ExtendedDatePicker DatePicker { get; set; }
    }

    public class CustomDatePicker_Droid : DatePicker
    {
        public CustomDatePicker_Droid(ExtendedDatePicker datePicker)
        {
            this.DatePicker = datePicker;

            if (DatePicker.MaxDate != null)
            {
                this.MaximumDate = DatePicker.MaxDate.Value;
            }

            if (DatePicker.MinDate != null)
            {
                this.MinimumDate = DatePicker.MinDate.Value;
            }

            this.TextColor = Color.Transparent;
            this.BackgroundColor = Color.Transparent;
            this.DateSelected += CustomDatePicker_Droid_DateSelected;
        }

        private void CustomDatePicker_Droid_DateSelected(object sender, DateChangedEventArgs e)
        {
            DatePicker?.DateChanged(e.NewDate);
        }

        public ExtendedDatePicker DatePicker { get; set; }
    }
}
