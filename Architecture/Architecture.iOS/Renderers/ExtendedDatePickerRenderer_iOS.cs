using System;
using System.Collections.ObjectModel;
using System.Linq;
using Architecture.Controls;
using Architecture;
using Architecture.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDatePicker_iOS), typeof(ExtendedDatePickerRenderer_iOS))]
namespace Architecture.iOS
{
    public class ExtendedDatePickerRenderer_iOS : PickerRenderer
    {
        public ExtendedDatePickerRenderer_iOS()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;

            Years = new ObservableCollection<YearModel>();

            var startDate = DateTime.Today.AddYears(-30);

            for (int i = 0; i <= 30; i++)
            {
                var yearDate = startDate.AddYears(i);

                YearModel yearModel = new YearModel
                {
                    Year = yearDate.Year.ToString(),
                    Months = new ObservableCollection<MonthModel>()
                };

                for (int y = 1; y <= 12; y++)
                {
                    MonthModel monthModel = new MonthModel
                    {
                        Month = y.ToString(),
                        MonthName = new DateTime(yearDate.Year, y, 1).GetMonthName(),
                        Days = new ObservableCollection<DayModel>()
                    };

                    var daysInMonth = DateTime.DaysInMonth(yearDate.Year, y);

                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        DayModel dayModel = new DayModel
                        {
                            Day = d.ToString()
                        };

                        monthModel.Days.Add(dayModel);
                    }

                    yearModel.Months.Add(monthModel);
                }

                Years.Add(yearModel);
            }

            var pickerSource = new PickerSource(this);

            UIPickerView view = new UIPickerView
            {
                Model = pickerSource,
            };

            pickerSource.DayComponentIndex = CustomDatePicker.DatePicker.HasDay ? 0 : -1;
            pickerSource.YearComponentIndex = (int)pickerSource.GetComponentCount(view) - 1;

            if (!CustomDatePicker.DatePicker.HasDay && CustomDatePicker.DatePicker.HasMonth)
            {
                pickerSource.MonthComponentIndex = 0;
            }
            else if (CustomDatePicker.DatePicker.HasMonth)
            {
                pickerSource.MonthComponentIndex = 1;
            }
            else
            {
                pickerSource.MonthComponentIndex = -1;
            }

            if (CustomDatePicker.DatePicker.HasYear)
            {
                view.Select(pickerSource.SelectedYear, pickerSource.YearComponentIndex, false);
            }

            if (CustomDatePicker.DatePicker.HasMonth)
            {
                view.Select(pickerSource.SelectedMonth, pickerSource.MonthComponentIndex, false);
            }

            if (CustomDatePicker.DatePicker.HasDay)
            {
                view.Select(pickerSource.SelectedDay, pickerSource.DayComponentIndex, false);
            }

            Control.InputView = view;
        }

        public ObservableCollection<YearModel> Years { get; private set; }

        public void SelectedPropertyChanged(string newValue)
        {
            if (DateTime.TryParse(newValue, out DateTime res))
            {
                CustomDatePicker?.DatePicker?.DateChanged(res);
            }
        }

        public CustomDatePicker_iOS CustomDatePicker => Element as CustomDatePicker_iOS;
    }

    public class PickerSource : UIPickerViewModel
    {
        public static int DisplayWidth = (int)UIScreen.MainScreen.Bounds.Width;

        private ExtendedDatePickerRenderer_iOS PickerView { get; }

        public int SelectedYear { get; internal set; }
        public int SelectedMonth { get; internal set; }
        public int SelectedDay { get; internal set; }

        public string SelectedItem { get; internal set; }

        public PickerSource(ExtendedDatePickerRenderer_iOS pickerView)
        {
            this.PickerView = pickerView;

            if (this.PickerView.Element is CustomDatePicker_iOS customDatePicker)
            {
                var year = pickerView.Years.FirstOrDefault(x => x.Year == customDatePicker.DatePicker.SelectedDate.Year.ToString());
                var month = year.Months.FirstOrDefault(x => x.Month == customDatePicker.DatePicker.SelectedDate.Month.ToString());
                var day = month.Days.FirstOrDefault(x => x.Day == customDatePicker.DatePicker.SelectedDate.Day.ToString());

                SelectedYear = pickerView.Years.IndexOf(year);
                SelectedMonth = year.Months.IndexOf(month);
                SelectedDay = month.Days.IndexOf(day);
            }
            else
            {
                SelectedYear = 0;
                SelectedMonth = 0;
                SelectedDay = 0;
            }
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            int componentsCount = 0;

            if (this.PickerView.Element is CustomDatePicker_iOS customDatePicker)
            {
                if (customDatePicker.DatePicker.HasDay)
                {
                    componentsCount++;
                }

                if (customDatePicker.DatePicker.HasMonth)
                {
                    componentsCount++;
                }

                if (customDatePicker.DatePicker.HasYear)
                {
                    componentsCount++;
                }

                return componentsCount;
            }

            return 3;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            if (component == YearComponentIndex)
            {
                return this.PickerView.Years.Count;
            }
            else if (component == MonthComponentIndex)
            {
                return this.PickerView.Years[SelectedYear].Months.Count;
            }
            else
            {
                return this.PickerView.Years[SelectedYear].Months[SelectedMonth].Days.Count;
            }
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (component == YearComponentIndex)
            {
                return this.PickerView.Years[(int)row].Year;
            }
            else if (component == MonthComponentIndex)
            {
                return this.PickerView.Years[SelectedYear].Months[(int)row].MonthName;
            }
            else
            {
                return this.PickerView.Years[SelectedYear].Months[SelectedMonth].Days[(int)row].Day;
            }
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (component == YearComponentIndex)
            {
                SelectedYear = (int)pickerView.SelectedRowInComponent(YearComponentIndex);

                if (this.PickerView.CustomDatePicker?.DatePicker?.HasDay == true)
                {
                    pickerView.ReloadComponent(DayComponentIndex);
                }

                if (this.PickerView.CustomDatePicker?.DatePicker?.HasMonth == true)
                {
                    pickerView.ReloadComponent(MonthComponentIndex);
                }
            }
            else if (component == MonthComponentIndex)
            {
                SelectedMonth = (int)pickerView.SelectedRowInComponent(MonthComponentIndex);

                if (this.PickerView.CustomDatePicker?.DatePicker?.HasDay == true)
                {
                    pickerView.ReloadComponent(DayComponentIndex);
                }
            }
            else
            {
                SelectedDay = (int)pickerView.SelectedRowInComponent(DayComponentIndex);
            }

            YearModel yearModel = this.PickerView.Years[SelectedYear];

            if (yearModel.Months.Count <= 0)
            {
                return;
            }

            MonthModel monthModel = yearModel.Months[SelectedMonth];

            if (monthModel.Days.Count <= 0)
            {
                return;
            }

            SelectedDay = SelectedDay >= monthModel.Days.Count ? monthModel.Days.Count - 1 : SelectedDay;

            DayModel dayModel = monthModel.Days[SelectedDay];

            SelectedItem = yearModel.Year + "-" + monthModel.Month + "-" + dayModel.Day;

            if (!string.IsNullOrEmpty(SelectedItem))
            {
                this.PickerView.SelectedPropertyChanged(SelectedItem);
            }
        }

        public override nfloat GetComponentWidth(UIPickerView pickerView, nint component)
        {
            var screenWidth = DisplayWidth;

            if (component == 0)
            {
                return screenWidth * 0.3f;
            }
            else if (component == 1)
            {
                return screenWidth * 0.3f;
            }
            else
            {
                return screenWidth * 0.3f;
            }
        }

        public int YearComponentIndex { get; set; }
        public int MonthComponentIndex { get; set; }
        public int DayComponentIndex { get; set; }
    }

    public class YearModel
    {
        public string Year { get; set; }
        public ObservableCollection<MonthModel> Months { get; set; }
    }

    public class MonthModel
    {
        public string Month { get; set; }
        public string MonthName { get; set; }
        public ObservableCollection<DayModel> Days { get; set; }
    }

    public class DayModel
    {
        public string Day { get; set; }
    }
}
