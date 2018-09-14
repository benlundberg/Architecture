using Acr.UserDialogs;
using Architecture.Core;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public virtual void OnInitialize()
        {
        }

        public virtual void Appearing()
        {
        }

        public virtual void Disappearing()
        {
        }

        protected string Translate(string key)
        {
            return TranslateHelper.Translate(key);
        }

        protected Task RunInBackgroundModeAsync(Func<Task> action, string backgroundTaskName = null)
        {
            return BackgroundHelper.RunInBackgroundModeAsync(action, backgroundTaskName);
        }

        protected void ExecuteIfConnected(Action actionToExecute, bool showAlert)
        {
            if (NetStatusHelper.IsConnected)
            {
                actionToExecute();
            }
            else if (showAlert)
            {
                ShowNoNetworkError();
            }
        }

        protected void ShowNoNetworkError(string message = null, string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                title = Translate("GenErr_NoNetworkTitle");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                message = Translate("GenErr_NoNetworkMessage");
            }

            message += " " + Translate("GenErr_CheckYourNetworkMessage");

            ShowAlert(message, title);
        }

        protected void ShowAlert(string message, string title, string okText = "Ok")
        {
            UserDialogs.Instance.Alert(message, title, okText);
        }

        protected Task<bool> ShowConfirm(string message, string title, string ok = null, string cancel = null)
        {
            ok = ok ?? Translate("Gen_Yes");
            cancel = cancel ?? Translate("Gen_No");

            return UserDialogs.Instance.ConfirmAsync(message, title, ok, cancel);
        }

        protected void ShowLoading(string title = null)
        {
            title = title ?? Translate("Gen_Loading");

            UserDialogs.Instance.ShowLoading(title);
        }

        protected void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }

        private ITranslateHelper translateHelper;
        public ITranslateHelper TranslateHelper => translateHelper ?? (translateHelper = ComponentContainer.Current.Resolve<ITranslateHelper>());

        private INetworkStatusHelper netStatusHelper;
        public INetworkStatusHelper NetStatusHelper => netStatusHelper ?? (netStatusHelper = ComponentContainer.Current.Resolve<INetworkStatusHelper>());

        private IBackgroundHelper backgroundHelper;
        public IBackgroundHelper BackgroundHelper => backgroundHelper ?? (backgroundHelper = ComponentContainer.Current.Resolve<IBackgroundHelper>());

        public INavigation Navigation { get; set; }
        public bool IsNavigating { get; set; }
        public bool IsBusy { get; set; }
        public bool IsNotBusy => !IsBusy;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
