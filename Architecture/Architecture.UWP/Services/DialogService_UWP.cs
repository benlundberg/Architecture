using Architecture.Core;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Architecture.UWP
{
    public class DialogService_UWP : IDialogService
    {
        public async Task<bool> ShowCheckboxDialogAsync(string title, string content, string checkboxTitle, string confirm = "Ok")
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = confirm,
                SecondaryButtonText = checkboxTitle
            };

            var res = await contentDialog.ShowAsync();

            return res == ContentDialogResult.Secondary;
        }

        public async Task<string> ShowSimpleDialogAsync(string title, string[] items, string close = null)
        {
            var res = await Architecture.App.Current.MainPage.DisplayActionSheet(title, close, null, items);

            return res;
        }

        public void ShowToast(string text, bool shortLength = true)
        {
            
        }
    }
}
