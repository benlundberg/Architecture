using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Architecture.Core;
using Xamarin.Essentials;
using System.IO;
using System.Threading.Tasks;
using Architecture.Controls;

namespace Architecture.UIKit.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        public FeedbackViewModel()
        {
            DescriptionText = new ValidatableObject<string>(new List<IValidationRule<string>>
            {
                new IsNotNullOrEmptyRule<string>("You need do provide a description text")
            });

            Email = new ValidatableObject<string>(new List<IValidationRule<string>>()
            {
                new IsValidEmailRule<string>("You need to provide a valid email") { IsOptional = true }
            });
        }

        private ICommand takePhotoCommand;
        public ICommand TakePhotoCommand => takePhotoCommand ?? (takePhotoCommand = new Command(() =>
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                    {
                        Title = AppConfig.AppName
                    });

                    if (photo == null)
                    {
                        return;
                    }

                    await LoadPhotoAsync(photo);
                }
                catch (PermissionException pe)
                {
                    pe.Print();

                    var res = await ShowConfirmAsync("The app needs permission to use this feature. You can grant them in app settings", "Need permissions", "Goto settings");

                    if (res)
                    {
                        AppInfo.ShowSettingsUI();
                    }
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
            });
        }));

        private ICommand selectPhotoCommand;
        public ICommand SelectPhotoCommand => selectPhotoCommand ?? (selectPhotoCommand = new Command(() =>
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                    {
                        Title = AppConfig.AppName
                    });

                    if (photo == null)
                    {
                        return;
                    }

                    await LoadPhotoAsync(photo);
                }
                catch (PermissionException pe)
                {
                    pe.Print();

                    var res = await ShowConfirmAsync("The app needs permission to use this feature. You can grant them in app settings", "Need permissions", "Goto settings");

                    if (res)
                    {
                        AppInfo.ShowSettingsUI();
                    }
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
            });
        }));

        private ICommand sendFeedbackPhotoCommand;
        public ICommand SendFeedbackPhotoCommand => sendFeedbackPhotoCommand ?? (sendFeedbackPhotoCommand = new Command(async () =>
        {
            if (!DescriptionText.Validate() || !Email.Validate())
            {
                return;
            }

            var res = await ShowConfirmAsync("Send us your feedback?", "Are you sure?");

            if (!res)
            {
                return;
            }

            var loading = new LoadingPopup("Sending feedback");

            await loading.ShowAsync();

            await Task.Delay(TimeSpan.FromSeconds(2));

            await loading.HideAsync();

            var notification = new NotificationPopup(new NotificationOption
            {
                Message = "We really appreciate your feedback!",
                MessageTitle = "Thank you",
                NotificationGrade = NotificationGrade.Success
            });

            ImageSource = null;
            DescriptionText.Value = string.Empty;
            Email.Value = string.Empty;
        }));
        
        private ICommand descriptionTextChanged;
        public ICommand DescriptionTextChanged => descriptionTextChanged ?? (descriptionTextChanged = new Command(async () =>
        {
            DescriptionText.IsValid = true;    
        }));

        private async Task LoadPhotoAsync(FileResult photo)
        {
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using var stream = await photo.OpenReadAsync();

            using var newStream = File.OpenWrite(newFile);

            await stream.CopyToAsync(newStream);

            ImageSource = ImageSource.FromUri(new Uri(newFile));
        }

        public ValidatableObject<string> DescriptionText { get; set; }
        public ValidatableObject<string> Email { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
