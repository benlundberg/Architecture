using Architecture.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Architecture.UIKit
{
    public class MediaViewModel: BaseViewModel
    {
        private ICommand mediaCommand;
        public ICommand MediaCommand => mediaCommand ?? (mediaCommand = new Command((param) =>
        {
            if (!int.TryParse(param?.ToString(), out int res))
            {
                return;
            }

            switch (res)
            {
                case 1:
                    TakePhoto();
                    break;
                case 2:
                    PickPhoto();
                    break;
                case 3:
                    TakeVideo();
                    break;
                case 4:
                    PickVideo();
                    break;
                default:
                    break;
            }
        }));

        private void PickVideo()
        {
            
        }

        private void TakeVideo()
        {
        }

        private void PickPhoto()
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
        }

        private void TakePhoto()
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
        }

        private async Task LoadPhotoAsync(FileResult photo)
        {
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using var stream = await photo.OpenReadAsync();
            
            using var newStream = File.OpenWrite(newFile);
            
            await stream.CopyToAsync(newStream);

            ImageSource = ImageSource.FromUri(new Uri(newFile));
        }

        public ImageSource ImageSource { get; set; }
    }
}
