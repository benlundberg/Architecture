using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IDialogService
    {
        /// <summary>
        /// Displays a simple dialog with list item options
        /// </summary>
        /// <param name="title">Title of dialog</param>
        /// <param name="items">Selectable options</param>
        /// <param name="confirm">Confirm button text</param>
        /// <returns>Selected item</returns>
        Task<string> ShowSimpleDialogAsync(string title, string[] items, string close = null);

        /// <summary>
        /// Displays a dialog with checkbox
        /// </summary>
        /// <param name="title">Title of dialog</param>
        /// <param name="content">Content text for dialog</param>
        /// <param name="checkboxTitle">Title for checkbox</param>
        /// <returns>If checked or not</returns>
        Task<bool> ShowCheckboxDialogAsync(string title, string content, string checkboxTitle, string confirm = "Ok");

        /// <summary>
        /// Shows a simple toast message on Android (On iOS nothing shows)
        /// </summary>
        /// <param name="text">Message in Toast</param>
        /// <param name="shortLength">Bool = true will display short ToastLength</param>
        void ShowToast(string text, bool shortLength = true);
    }
}
