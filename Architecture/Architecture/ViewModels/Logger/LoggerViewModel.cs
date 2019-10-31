using Architecture.Core;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Architecture
{
    public class LoggerViewModel : BaseViewModel
    {
        public override void Appearing()
        {
            base.Appearing();

            LogText = Logger.GetLog();
        }

        private ICommand clearLogCommand;
        public ICommand ClearLogCommand => clearLogCommand ?? (clearLogCommand = new Command(async () =>
        {
            var res = await ShowConfirmAsync("Clear log?", "Sure?");

            if (res == false)
            {
                return;
            }

            LogText = string.Empty;
            Logger.ClearLog();
        }));

        private ICommand sendLogCommand;
        public ICommand SendLogCommand => sendLogCommand ?? (sendLogCommand = new Command(async () =>
        {
            try
            {
                await Email.ComposeAsync("Log", LogText);
            }
            catch (System.Exception ex)
            {
                ex.Print();
                ShowAlert(ex.Message, "Logger");
            }
        }));

        public string LogText { get; private set; }
    }
}
