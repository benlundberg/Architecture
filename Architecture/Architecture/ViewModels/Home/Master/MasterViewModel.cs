using System.Collections.Generic;
using System.Linq;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture
{
    public class MasterViewModel : BaseViewModel
    {
        public MasterViewModel(FlyoutPage masterDetailPage)
        {
            Title = Device.RuntimePlatform == Device.iOS ? "☰" : AppConfig.AppName;

            this.masterDetailPage = masterDetailPage;

            if (Device.RuntimePlatform == Device.UWP)
            {
                masterDetailPage.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
            }

            MasterItems = new List<MenuViewModel>()
            {
                // TODO: Add menu pages here
                new MenuViewModel()
                {
                    Title = "Article Browser",
                    Page = new NavigationPage(new UIKit.Views.Phone.ArticleBrowserPage())
                }
            };

            // TODO: If test you can add this log view
            if (true)
            {
                MasterItems.Add(new MenuViewModel()
                {
                    Title = Resources.Strings.Gen_Log,
                    Page = new NavigationPage(ViewContainer.Current.CreatePage<LoggerViewModel>())
                });
            }

            ItemSelected(MasterItems.FirstOrDefault()?.Page);
        }

        private void ItemSelected(Page page)
        {
            if (page == null)
            {
                return;
            }

            masterDetailPage.Detail = page;
        }

        private MenuViewModel selectedMasterItem;
        public MenuViewModel SelectedMasterItem
        {
            get { return selectedMasterItem; }
            set
            {
                selectedMasterItem = value;

                if (selectedMasterItem != null)
                {
                    masterDetailPage.IsPresented = false;

                    if (selectedMasterItem.Page == null)
                    {
                        selectedMasterItem.Action?.Invoke();
                    }
                    else
                    {
                        ItemSelected(selectedMasterItem.Page);
                    }

                    SelectedMasterItem = null;
                }
            }
        }

        public List<MenuViewModel> MasterItems { get; private set; }

        public string Title { get; set; }

        private readonly FlyoutPage masterDetailPage;
    }
}
