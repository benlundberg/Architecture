using System.Threading.Tasks;
using Architecture.Core;
using UIKit;

namespace Architecture.iOS
{
    public class DialogService_iOS : IDialogService
    {
        public async Task<bool> ShowCheckboxDialogAsync(string title, string content, string checkboxTitle, string confirm = "Ok")
        {
            UIAlertController actionSheetAlert = UIAlertController.Create(title, content, UIAlertControllerStyle.ActionSheet);

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            // Add Actions
            actionSheetAlert.AddAction(UIAlertAction.Create(confirm, UIAlertActionStyle.Default, (action) => taskCompletionSource.TrySetResult(false)));

            actionSheetAlert.AddAction(UIAlertAction.Create(checkboxTitle, UIAlertActionStyle.Default, (action) => taskCompletionSource.TrySetResult(true)));

            var window = UIApplication.SharedApplication.KeyWindow;

            var vc = window.RootViewController;

            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = vc.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            vc.PresentViewController(actionSheetAlert, true, null);

            var res = await taskCompletionSource.Task;
            return res;
        }

        public async Task<string> ShowSimpleDialogAsync(string title, string[] items, string close = null)
        {
            var res = await App.Current.MainPage.DisplayActionSheet(title, close, null, items);

            return res;
        }

        public void ShowToast(string text, bool shortLength = true)
        {
            // Not supported on iOS
        }
    }
}