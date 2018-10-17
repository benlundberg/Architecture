﻿using System.Collections.Generic;
using System.Linq;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture
{
    public class MasterViewModel : BaseViewModel
    {
        public MasterViewModel(MasterDetailPage masterDetailPage)
        {
			Title = Device.RuntimePlatform == Device.iOS ? "☰" : AppConfig.AppName;

			this.masterDetailPage = masterDetailPage;

            MasterItems = new List<MenuViewModel>()
            {
                new MenuViewModel()
                {
                    Title = "List view",
                    Page = new NavigationPage(ViewContainer.Current.CreatePage<ListViewModel>())
                }
            };

            ItemSelected(MasterItems.FirstOrDefault()?.Page);
        }

        private void ItemSelected(Page page)
        {
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

                    ItemSelected(selectedMasterItem.Page);

                    SelectedMasterItem = null;
                }
            }
        }

        public List<MenuViewModel> MasterItems { get; private set; }

		public string Title { get; set; }

        private MasterDetailPage masterDetailPage;
    }
}
