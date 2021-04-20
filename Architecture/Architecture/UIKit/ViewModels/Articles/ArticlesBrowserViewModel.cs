﻿using Architecture.UIKit.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit.ViewModels
{
    public class ArticlesBrowserViewModel: BaseViewModel
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

                Articles = new ObservableCollection<ArticleItemViewModel>(UIKitService.GetArticles(12));
                
                TopArticles = new ObservableCollection<ArticleItemViewModel>(UIKitService.GetArticles(6));
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, "Error");
            }
        }

        private ICommand itemClickedCommand;
        public ICommand ItemClickedCommand => itemClickedCommand ?? (itemClickedCommand = new Command((param) =>
        {
            if (!(param is ArticleItemViewModel item))
            {
                return;
            }

            ItemSelected(item.Title);
        }));

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
        public ObservableCollection<ArticleItemViewModel> TopArticles { get; private set; }
    }
}
