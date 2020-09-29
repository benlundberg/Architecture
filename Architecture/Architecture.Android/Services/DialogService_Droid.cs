using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Architecture.Core;

namespace Architecture.Droid
{
    public class DialogAdapter : ArrayAdapter<string>
    {
        public DialogAdapter(Context context, int resource, int textViewResourceId, string[] objects) : base(context, resource, textViewResourceId, objects)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = base.GetView(position, convertView, parent);

            var typedVal = new TypedValue();

            var activity = Xamarin.Essentials.Platform.CurrentActivity;

            if (activity.Theme.ResolveAttribute(Resource.Attribute.dialogPreferredPadding, typedVal, true))
            {
                var padding = TypedValue.ComplexToDimensionPixelSize(typedVal.Data, activity.Resources.DisplayMetrics);
                view.SetPadding(padding, 0, padding, 0);
            }

            TextView textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            textView.SetTextSize(ComplexUnitType.Sp, 16);

            textView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.abc_ic_star_black_16dp, 0, 0, 0);

            int dp5 = (int)(5 * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density + 0.5f);
            textView.CompoundDrawablePadding = dp5;

            return view;
        }
    }

    public class DialogService_Droid : IDialogService
    {
        public async Task<bool> ShowCheckboxDialogAsync(string title, string content, string checkboxTitle, string confirm = "Ok")
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            LayoutInflater layoutInflater = LayoutInflater.From(activity);
            var view = layoutInflater.Inflate(Resource.Layout.CheckboxDialog, null);

            var cbxContent = view.FindViewById<TextView>(Resource.Id.cbxContent);
            cbxContent.Text = content;

            var cbxSkip = view.FindViewById<CheckBox>(Resource.Id.skip);
            cbxSkip.Text = checkboxTitle;

            var typedVal = new TypedValue();

            if (activity.Theme.ResolveAttribute(Resource.Attribute.dialogPreferredPadding, typedVal, true))
            {
                var padding = TypedValue.ComplexToDimensionPixelSize(typedVal.Data, activity.Resources.DisplayMetrics);
                cbxContent.SetPadding(0, padding / 2, 0, padding / 2);
                view.SetPadding(padding, 0, padding, 0);
            }

            var alert = new AlertDialog.Builder(activity, Resource.Style.AppTheme_AlertDialog)
                .SetTitle(title)
                .SetView(view)
                .SetCancelable(false)
                .SetPositiveButton(confirm, (sender, args) =>
                {
                    taskCompletionSource.TrySetResult(cbxSkip.Checked);
                });

            alert.Show();

            var res = await taskCompletionSource.Task;
            return res;
        }

        public async Task<string> ShowSimpleDialogAsync(string title, string[] items, string close = null)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;

            var listAdapter = new DialogAdapter(activity, Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, items);

            TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();

            var alert = new AlertDialog.Builder(activity, Resource.Style.AppTheme_AlertDialog)
                .SetTitle(title)
                .SetCancelable(false)
                .SetAdapter(listAdapter, (sender, args) =>
                {

                })
                .SetItems(items, (sender, args) =>
                {
                    var index = args.Which;
                    var item = items[index];

                    taskCompletionSource.TrySetResult(item);
                });

            if (!string.IsNullOrEmpty(close))
            {
                alert.SetNegativeButton(close, (sender, args) =>
                {
                    taskCompletionSource.TrySetResult(null);
                });
            }

            alert.Show();

            var res = await taskCompletionSource.Task;
            return res;
        }
    }
}