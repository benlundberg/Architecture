using System.Windows.Input;
using Android.App;
using Android.Widget;
using Architecture.Core;

namespace Architecture.Droid
{
	public class ToastHelper_Droid : IToastHelper
	{
		public void DisplayToast(string text, ToastTime toastTime = ToastTime.SHORT, bool snackbar = false, ICommand command = null)
		{
			Toast.MakeText(Application.Context, text, toastTime == ToastTime.SHORT ? ToastLength.Short : ToastLength.Long).Show();
		}
	}
}
