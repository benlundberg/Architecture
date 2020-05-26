using System.Threading.Tasks;
using Android.App;
using Android.Content;
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

            TextView textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);

            textView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.abc_ic_star_black_16dp, 0, 0, 0);

            int dp5 = (int)(5 * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density + 0.5f);
            textView.CompoundDrawablePadding = dp5;

            return view;
        }
    }

    public class DialogService_Droid : IDialogService
    {
        public async Task<string> ShowSimpleDialog(string title, string[] items, string confirm)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;

            var listAdapter = new DialogAdapter(activity, Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, items);

            TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();

            var alert = new AlertDialog.Builder(activity)
                .SetTitle(title)
                .SetAdapter(listAdapter, (sender, args) =>
                {

                })
                .SetItems(items, (sender, args) =>
                {
                    var index = args.Which;
                    var item = items[index];

                    taskCompletionSource.TrySetResult(item);
                });

            alert.Show();

            var res = await taskCompletionSource.Task;
            return res;
        }
    }
}