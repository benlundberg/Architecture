using Architecture.Controls;
using Architecture.UIKit.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ICommand syncDataCommand;
        public ICommand SyncDataCommand => syncDataCommand ?? (syncDataCommand = new Command(async () =>
        {
            var loadingPopup = new LoadingPopup("Syncing data");

            await loadingPopup.ShowAsync();

            await Task.Delay(TimeSpan.FromSeconds(2));

            await loadingPopup.HideAsync();

            var success = new NotificationPopup(new NotificationOption
            {
                MessageTitle = "Sync done",
                Message = "Your data is now synced and up to date",
                NotificationGrade = NotificationGrade.Success,
                NotificationDuration = NotificationDuration.Short
            });

            await success.ShowAsync();
        }));

        private ICommand clearCacheCommand;
        public ICommand ClearCacheCommand => clearCacheCommand ?? (clearCacheCommand = new Command(async () =>
        {
            var res = await ShowConfirmAsync("Do you want to clear cache?", "Are you sure?");

            if (!res)
            {
                return;
            }

            var success = new NotificationPopup(new NotificationOption
            {
                MessageTitle = "Cache is cleared",
                Message = "Your cache is cleared and you have now lot's of space",
                NotificationGrade = NotificationGrade.Success,
                NotificationDuration = NotificationDuration.Short
            });

            await success.ShowAsync();
        }));

        private ICommand aboutUsCommand;
        public ICommand AboutUsCommand => aboutUsCommand ?? (aboutUsCommand = new Command(async () =>
        {
            await Navigation.PushAsync(new AboutPage());
        }));

        private ICommand feedbackCommand;
        public ICommand FeedbackCommand => feedbackCommand ?? (feedbackCommand = new Command(async () =>
        {
            await Navigation.PushAsync(ViewContainer.Current.CreatePage<FeedbackViewModel>());
        }));

        private ICommand contactUsCommand;
        public ICommand ContactUsCommand => contactUsCommand ?? (contactUsCommand = new Command(async () =>
        {
        }));

        private ICommand privacyPolicyCommand;
        public ICommand PrivacyPolicyCommand => privacyPolicyCommand ?? (privacyPolicyCommand = new Command(async () =>
        {
        }));
    }
}
