using Architecture.UIKit.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class ArticlesListViewModel : BaseViewModel
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                await LoadDataAsync();

                IsBusy = false;
            });
        }

        public async Task LoadDataAsync()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                Articles = new ObservableCollection<ArticleItemViewModel>(UIKitService.GetArticles(14));
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, "Error");
            }
        }

        public void ItemSelected(string id)
        {
            ShowAlert($"You clicked on {id}", "");
        }

        private ArticleItemViewModel selectedItem;
        public ArticleItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                if (selectedItem != null)
                {
                    ItemSelected(selectedItem.Title);

                    SelectedItem = null;
                }
            }
        }

        public ObservableCollection<ArticleItemViewModel> Articles { get; private set; }
    }
}
