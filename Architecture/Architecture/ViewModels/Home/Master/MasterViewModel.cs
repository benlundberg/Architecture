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

            if (Device.RuntimePlatform == Device.UWP)
            {
                masterDetailPage.MasterBehavior = MasterBehavior.Popover;
            }

            MasterItems = new List<MenuViewModel>()
            {
                new MenuViewModel()
                {
                    Title = Translate("Gen_Login"),
                    Page = new NavigationPage(ViewContainer.Current.CreatePage<Demos.UI.Login.LoginViewModel>())
                },
                new MenuViewModel()
                {
                    Title = "List imagecell",
                    Page = new NavigationPage(new Demos.UI.List.ListImageCellPage() { BindingContext = new Demos.UI.List.ListViewModel() })
				},
                new MenuViewModel()
                {
                    Title = "List textcell",
                    Page = new NavigationPage(new Demos.UI.List.ListTextCellPage() { BindingContext = new Demos.UI.List.ListViewModel() })
                },
                new MenuViewModel()
				{
					Title = "Grid view",
                    Page = new NavigationPage(new Demos.UI.List.GridViewPage() { BindingContext = new Demos.UI.List.ListViewModel() })
                },
                new MenuViewModel()
                {
                    Title = "List card view",
                    Page = new NavigationPage(new Demos.UI.List.ListCardViewPage() { BindingContext = new Demos.UI.List.ListViewModel() })
                },
                new MenuViewModel()
				{
					Title = "Details",
					Page = new NavigationPage(ViewContainer.Current.CreatePage<Demos.UI.Details.DetailsViewModel>())
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
