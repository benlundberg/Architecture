using Architecture.Controls;
using Architecture.Core;
using Rg.Plugins.Popup.Animations;
using System;
using System.Collections.Generic;
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
            var snackbarPopup = new SnackbarPopup(new SnackbarOption
            {
                ButtonText = "Okay",
                Command = new Command(() => { }),
                Duration = SnackbarDuration.Short,
                Message = "This is a snackbar message"
            });

            await snackbarPopup.ShowAsync();
        }));

        private ICommand showLoadingSnackbarCommand;
        public ICommand ShowLoadingSnackbarCommand => showLoadingSnackbarCommand ?? (showLoadingSnackbarCommand = new Command(async () =>
        {
            var loading = new SnackbarLoadingPopup();

            await loading.ShowAsync();

            await Task.Delay(TimeSpan.FromSeconds(2));

            await loading.HideAsync();
        }));

        private ICommand showLoadingCommand;
        public ICommand ShowLoadingCommand => showLoadingCommand ?? (showLoadingCommand = new Command(async () =>
        {
            var loading = new LoadingPopup("Loading...");

            await loading.ShowAsync();

            await Task.Delay(TimeSpan.FromSeconds(2));

            await loading.HideAsync();
        }));

        private ICommand showNotificationCommand;
        public ICommand ShowNotificationCommand => showNotificationCommand ?? (showNotificationCommand = new Command(async (param) =>
        {
            NotificationPopup notificationPopup = null;

            switch (int.Parse(param.ToString()))
            {
                case 1:
                    notificationPopup = new NotificationPopup(new NotificationOption
                    {
                        ButtonText = "Read more",
                        Command = new Command(() =>
                        {

                        }),
                        Message = "This is just a regular notification",
                        MessageTitle = "This is the title",
                        NotificationDuration = NotificationDuration.UntilDissmissed,
                        NotificationGrade = NotificationGrade.Regular
                    });
                    break;
                case 2:
                    notificationPopup = new NotificationPopup(new NotificationOption
                    {
                        MessageTitle = "Success!",
                        Message = "You did a good thing!",
                        NotificationDuration = NotificationDuration.Short,
                        NotificationGrade = NotificationGrade.Success
                    });
                    break;
                case 3:
                    notificationPopup = new NotificationPopup(new NotificationOption
                    {
                        Message = "Warning warning warning!",
                        NotificationDuration = NotificationDuration.Short,
                        NotificationGrade = NotificationGrade.Warning
                    });
                    break;
                case 4:
                    notificationPopup = new NotificationPopup(new NotificationOption
                    {
                        MessageTitle = "Alert, error!",
                        Message = "Something terrible have happned! Damn it!",
                        NotificationDuration = NotificationDuration.Short,
                        NotificationGrade = NotificationGrade.Error
                    });
                    break;
                default:
                    break;
            }

            await notificationPopup.ShowAsync();
        }));

        private ICommand showPushNotificationCommand;
        public ICommand ShowPushNotificationCommand => showPushNotificationCommand ?? (showPushNotificationCommand = new Command((param) =>
        {
            notificationCount++;
            
            string title = $"Local Notification #{notificationCount}";
            string message = $"You have now received {notificationCount} notifications!";

            ComponentContainer.Current.Resolve<INotificationService>().ScheduleNotification(title, message);
        }));

        private ICommand showInputPopupCommand;
        public ICommand ShowInputPopupCommand => showInputPopupCommand ?? (showInputPopupCommand = new Command(async (param) =>
        {
            InputPopup inputPopup = new InputPopup(new ValidatableObject<string>() { Value = "Some already written text" }, "Words");

            var text = await inputPopup.ShowAsync();

            ShowAlert(text, "Input");
        }));

        private ICommand showInputValidationPopupCommand;
        public ICommand ShowInputValidationPopupCommand => showInputValidationPopupCommand ?? (showInputValidationPopupCommand = new Command(async (param) =>
        {
            InputPopup inputPopup = new InputPopup(new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need to provide a value")
            }), ""
            , new InputOption
            {
                Animation = new MoveAnimation(Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom, Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom),
                Placeholder = "Super placeholder",
                PlaceholderColor = App.Current.PrimaryColor(),
                VerticalPosition = LayoutOptions.End,
                Keyboard = Keyboard.Numeric,
                MaxLength = 4
            });

            var text = await inputPopup.ShowAsync();

            ShowAlert(text, "Input");
        }));


        public string IsChecked { get; set; }
        public string ConfirmAnswer { get; set; }

        private int notificationCount;
    }
}
