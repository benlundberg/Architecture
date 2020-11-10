using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
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
    public enum NotificationGrade
    {
        Regular,
        Warning,
        Success,
        Error
    }

    public enum NotificationDuration
    {
        Short,
        Long,
        UntilDissmissed
    }

    public class NotificationOption
    {
        public string Message { get; set; }
        public NotificationGrade NotificationGrade { get; set; }
        public NotificationDuration NotificationDuration { get; set; }
        public MoveAnimationOptions MoveInAnimation { get; set; } = MoveAnimationOptions.Top;
        public MoveAnimationOptions MoveOutAnimation { get; set; } = MoveAnimationOptions.Top;
        public string MessageTitle { get; set; }
        public string ButtonText { get; set; }
        public ICommand Command { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPopup : PopupPage
    {
        public NotificationPopup(NotificationOption option)
        {
            this.Option = option;

            this.Animation = new MoveAnimation(option.MoveInAnimation, option.MoveOutAnimation);

            switch (option.NotificationGrade)
            {
                case NotificationGrade.Regular:
                    NotificationBackground = Color.White;
                    TextColor = Color.Black;
                    ButtonTextColor = Application.Current.PrimaryColor();
                    break;
                case NotificationGrade.Warning:
                    NotificationBackground = Application.Current.Get<Color>("WarningColor");
                    TextColor = Color.White;
                    ButtonTextColor = Color.White;
                    break;
                case NotificationGrade.Success:
                    NotificationBackground = Application.Current.Get<Color>("SuccessColor");
                    TextColor = Color.White;
                    ButtonTextColor = Color.White;
                    break;
                case NotificationGrade.Error:
                    NotificationBackground = Application.Current.Get<Color>("ErrorColor");
                    TextColor = Color.White;
                    ButtonTextColor = Color.White;
                    break;
                default:
                    break;
            }

            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (Option.NotificationDuration == NotificationDuration.UntilDissmissed)
            {
                return;
            }

            if (Option.NotificationDuration == NotificationDuration.Short)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            await HideAsync();
        }

        public async Task ShowAsync()
        {
            await PopupNavigation.Instance.PushAsync(this);
        }

        public async Task HideAsync()
        {
            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Option.Command?.Execute(null);
            await HideAsync();
        }

        public NotificationOption Option { get; set; }
        public bool IsButtonVisible => !string.IsNullOrEmpty(Option?.ButtonText);
        public Color TextColor { get; set; }
        public Color NotificationBackground { get; set; }
        public Color ButtonTextColor { get; set; } 
    }
}