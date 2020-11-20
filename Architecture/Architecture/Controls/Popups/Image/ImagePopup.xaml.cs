using Architecture.Core;
using FFImageLoading.Forms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePopup : PopupPage
    {
        public ImagePopup(ImageSource imageSource)
        {
            this.ImageSource = imageSource;

            InitializeComponent();
        }

        public async Task ShowAsync()
        {
            await PopupNavigation.Instance.PushAsync(this);
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (!(sender is CachedImage image))
            {
                return;
            }

            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = image.Scale;
                image.AnchorX = 0;
                image.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = image.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (image.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = image.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (image.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * image.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * image.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                image.TranslationX = targetX.Clamp(-image.Width * (currentScale - 1), 0);
                image.TranslationY = targetY.Clamp(-image.Height * (currentScale - 1), 0);

                // Apply scale factor.
                image.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    image.TranslationX = 0;
                    image.TranslationY = 0;

                    await Task.WhenAll(image.ScaleTo(1, 150));

                    currentScale = 1;
                    startScale = 1;
                    xOffset = 0;
                    yOffset = 0;
                });
            }
        }

        private ICommand imageTappedCommand;
        public ICommand ImageTappedCommand => imageTappedCommand ?? (imageTappedCommand = new Command(() =>
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (topToolbar.IsVisible)
                {
                    await topToolbar.FadeTo(0);

                    topToolbar.IsVisible = false;
                }
                else
                {
                    topToolbar.IsVisible = true;

                    await topToolbar.FadeTo(1);
                }
            });
        }));

        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new Command(async () =>
        {
            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }));

        public ImageSource ImageSource { get; set; }

        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
    }
}