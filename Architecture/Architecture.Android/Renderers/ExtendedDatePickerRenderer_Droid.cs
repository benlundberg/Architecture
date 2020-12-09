using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Architecture.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker_Droid), typeof(Architecture.Droid.ExtendedDatePickerRenderer_Droid))]
namespace Architecture.Droid
{
    public class ExtendedDatePickerRenderer_Droid : DatePickerRenderer
    {
        public ExtendedDatePickerRenderer_Droid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gradientDrawable = new GradientDrawable();
                gradientDrawable.SetStroke(0, global::Android.Graphics.Color.LightGray);
                Control.SetBackground(gradientDrawable);
            }
        }

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var datePickerDialog = base.CreateDatePickerDialog(year, month, day);

            if (!(Element is CustomDatePicker_Droid picker))
            {
                return datePickerDialog;
            }

            var customPicker = picker.DatePicker;

            if (customPicker == null)
            {
                return datePickerDialog;
            }
            
            datePickerDialog.SetTitle(customPicker.Title);

            try
            {
                int daySpinnerId = Resources.GetIdentifier("day", "id", "android");

                if (daySpinnerId != 0)
                {
                    global::Android.Views.View daySpinner = datePickerDialog.DatePicker.FindViewById(daySpinnerId);

                    if (daySpinner != null)
                    {
                        daySpinner.Visibility = customPicker.HasDay ? global::Android.Views.ViewStates.Visible : global::Android.Views.ViewStates.Gone;
                    }
                }

                int monthSpinnerId = Resources.GetIdentifier("month", "id", "android");

                if (monthSpinnerId != 0)
                {
                    global::Android.Views.View monthSpinner = datePickerDialog.DatePicker.FindViewById(monthSpinnerId);

                    if (monthSpinner != null)
                    {
                        monthSpinner.Visibility = customPicker.HasMonth ? global::Android.Views.ViewStates.Visible : global::Android.Views.ViewStates.Gone;
                    }
                }

                int yearSpinnerId = Resources.GetIdentifier("year", "id", "android");

                if (yearSpinnerId != 0)
                {
                    global::Android.Views.View yearSpinner = datePickerDialog.DatePicker.FindViewById(yearSpinnerId);

                    if (yearSpinner != null)
                    {
                        yearSpinner.Visibility = customPicker.HasYear ? global::Android.Views.ViewStates.Visible : global::Android.Views.ViewStates.Gone;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return datePickerDialog;
        }
    }
}