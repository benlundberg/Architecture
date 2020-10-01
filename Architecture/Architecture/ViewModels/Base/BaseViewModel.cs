using Architecture.Core;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Architecture
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Method is called when page is created and passes the created page as param
        /// </summary>
        public virtual void OnPageCreated(Page page)
        {

        }

        /// <summary>
        /// Method is called when page is initialized
        /// </summary>
        public virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Method is called when page is appearing
        /// </summary>
        public virtual void Appearing()
        {
        }

        /// <summary>
        /// Method is called when page is disappearing
        /// </summary>
        public virtual void Disappearing()
        {
        }

        /// <summary>
        /// Method to execute an action if device has internet connection
        /// </summary>
        /// <param name="actionToExecute">Action to execute</param>
        /// <param name="showAlert">Boolean value if an alert should be prompt if device don't have internet connection</param>
        protected void ExecuteIfConnected(Action actionToExecute, bool showAlert)
        {
            if (IsConnected)
            {
                actionToExecute();
            }
            else if (showAlert)
            {
                ShowNoNetworkError();
            }
        }

        /// <summary>
        /// Displays a "No network" dialog
        /// </summary>
        /// <param name="message">If a different message then default should be shown</param>
        /// <param name="title">If a different title then default should be shown</param>
        protected void ShowNoNetworkError(string message = null, string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                title = Resources.Strings.GenErr_NoNetworkMessage;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                message = Resources.Strings.GenErr_NoNetworkMessage;
            }

            message += " " + Resources.Strings.GenErr_CheckYourNetworkMessage;

            ShowAlert(message, title);
        }

        protected void ShowAlert(string message, string title, string okText = "Ok")
        {
            Application.Current.MainPage.DisplayAlert(title, message, okText);
        }

        protected Task<bool> ShowConfirmAsync(string message, string title, string ok = null, string cancel = null)
        {
            ok = ok ?? Resources.Strings.Gen_Yes;
            cancel = cancel ?? Resources.Strings.Gen_No;

            return Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }

        protected Task<string> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            return Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

		private ICommand popModalCommand;
		public ICommand PopModalCommand => popModalCommand ?? (popModalCommand = new Command(async () =>
		{
			await Navigation.PopModalAsync();
		}));

        private ICommand popCommand;
        public ICommand PopCommand => popCommand ?? (popCommand = new Command(async () =>
        {
            await Navigation.PopAsync();
        }));

        private ILoggerService loggerHelper;
        protected ILoggerService Logger => loggerHelper ?? (loggerHelper = ComponentContainer.Current.Resolve<ILoggerService>());

        private IDialogService dialogService;
        protected IDialogService Dialog => dialogService ?? (dialogService = ComponentContainer.Current.Resolve<IDialogService>());

        private IConnectivityService connectivityService;
        protected IConnectivityService Connectivity => connectivityService ?? (connectivityService = ComponentContainer.Current.Resolve<IConnectivityService>());

        public INavigation Navigation { get; set; }
        public bool IsBusy { get; set; }
        public bool IsNotBusy => !IsBusy;
        public bool IsConnected => Connectivity.IsConnected;

        public event PropertyChangedEventHandler PropertyChanged;
	}
}
