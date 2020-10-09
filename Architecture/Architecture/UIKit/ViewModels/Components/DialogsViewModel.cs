using Architecture.Controls;
using Architecture.Core;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
{
    public class DialogsViewModel : BaseViewModel
    {
        private ICommand checkDialogCommand;
        public ICommand CheckDialogCommand => checkDialogCommand ?? (checkDialogCommand = new Command(async () =>
        {
            var isChecked = await ComponentContainer.Current.Resolve<IDialogService>().ShowCheckboxDialogAsync("Checkbox dialog", "This is a dialog with a checkbox", "Don't show again");
        
            IsChecked = isChecked ? Resources.Strings.Gen_Yes : Resources.Strings.Gen_No;
        }));

        private ICommand simpleDialogCommand;
        public ICommand SimpleDialogCommand => simpleDialogCommand ?? (simpleDialogCommand = new Command(async () =>
        {
            var option = await ComponentContainer.Current.Resolve<IDialogService>().ShowSimpleDialogAsync("Select", new string[] { "Option 1", "Option 2", "Option 3", "Option 4" });
        }));

        private ICommand alertDialogCommand;
        public ICommand AlertDialogCommand => alertDialogCommand ?? (alertDialogCommand = new Command(() =>
        {
            ShowAlert("This is an alert", "Alert dialog");
        }));

        private ICommand confirmDialogCommand;
        public ICommand ConfirmDialogCommand => confirmDialogCommand ?? (confirmDialogCommand = new Command(async () =>
        {
            var res = await ShowConfirmAsync("Will you confirm this dialog?", "Confirm dialog");

            ConfirmAnswer = res ? Resources.Strings.Gen_Yes : Resources.Strings.Gen_No;
        }));

        private ICommand showToastCommand;
        public ICommand ShowToastCommand => showToastCommand ?? (showToastCommand = new Command(() =>
        {
            ComponentContainer.Current.Resolve<IDialogService>().ShowToast("This is a toast message");
        }));

        private ICommand showSnackbarCommand;
        public ICommand ShowSnackbarCommand => showSnackbarCommand ?? (showSnackbarCommand = new Command(async () =>
        {
            await PopupNavigation.Instance.PushAsync(new SnackbarPopup(new SnackbarOption
            {
                ButtonText = "Okay",
                Command = new Command(() => { }),
                Duration = SnackbarDuration.Short,
                Message = "This is a snackbar message"
            }));
        }));

        private ICommand showLoadingSnackbarCommand;
        public ICommand ShowLoadingSnackbarCommand => showLoadingSnackbarCommand ?? (showLoadingSnackbarCommand = new Command(async () =>
        {
            var loading = new SnackbarLoadingPopup("Loading...");

            await PopupNavigation.Instance.PushAsync(loading);

            await Task.Delay(TimeSpan.FromSeconds(2));

            await loading.HideAsync();
        }));

        private ICommand showLoadingCommand;
        public ICommand ShowLoadingCommand => showLoadingCommand ?? (showLoadingCommand = new Command(async () =>
        {
            var loading = new LoadingPopup("Loading...");

            await PopupNavigation.Instance.PushAsync(loading);

            await Task.Delay(TimeSpan.FromSeconds(2));

            await loading.HideAsync();
        }));

        private ICommand showNotificationCommand;
        public ICommand ShowNotificationCommand => showNotificationCommand ?? (showNotificationCommand = new Command(async (param) =>
        {
            switch (int.Parse(param.ToString()))
            {
                case 1:
                    await PopupNavigation.Instance.PushAsync(new NotificationPopup(new NotificationOption
                    {
                        ButtonText = "Read more",
                        Command = new Command(() =>
                        {

                        }),
                        Message = "This is just a regular notification",
                        MessageTitle = "This is the title",
                        NotificationDuration = NotificationDuration.UntilDissmissed,
                        NotificationGrade = NotificationGrade.Regular
                    }));
                    break;
                case 2:
                    await PopupNavigation.Instance.PushAsync(new NotificationPopup(new NotificationOption
                    {
                        MessageTitle = "Success!",
                        Message = "You did a good thing!",
                        NotificationDuration = NotificationDuration.Short,
                        NotificationGrade = NotificationGrade.Success
                    }));
                    break;
                case 3:
                    await PopupNavigation.Instance.PushAsync(new NotificationPopup(new NotificationOption
                    {
                        Message = "Warning warning warning!",
                        NotificationDuration = NotificationDuration.Short,
                        NotificationGrade = NotificationGrade.Warning
                    }));
                    break;
                case 4:
                    await PopupNavigation.Instance.PushAsync(new NotificationPopup(new NotificationOption
                    {
                        MessageTitle = "Alert, error!",
                        Message = "Something terrible have happned! Damn it!",
                        NotificationDuration = NotificationDuration.Short,
                        NotificationGrade = NotificationGrade.Error
                    }));
                    break;
                default:
                    break;
            }
        }));

        private ICommand showPushNotificationCommand;
        public ICommand ShowPushNotificationCommand => showPushNotificationCommand ?? (showPushNotificationCommand = new Command(async (param) =>
        {
            notificationCount++;
            
            string title = $"Local Notification #{notificationCount}";
            string message = $"You have now received {notificationCount} notifications!";

            ComponentContainer.Current.Resolve<INotificationService>().ScheduleNotification(title, message);
        }));

        public string IsChecked { get; set; }
        public string ConfirmAnswer { get; set; }

        private int notificationCount;
    }
}
