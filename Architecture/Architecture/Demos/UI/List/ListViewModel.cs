using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture.Demos.UI.List
{
	public class ListViewModel : BaseViewModel
	{
        public ListViewModel()
        {
            if (NetStatusHelper.IsConnected)
            {
                LoadItems();
            }
            else
            {
                ShowNoNetworkError();
            }
        }

		private ICommand refreshListCommand;
		public ICommand RefreshListCommand => refreshListCommand ?? (refreshListCommand = new Command(async () =>
		{
			try
			{
				await Task.Delay(TimeSpan.FromSeconds(2));

				Items = new ObservableCollection<string>();

				for (int i = 0; i < 20; i++)
				{
					Items.Add($"Item {i}");
				}
			}
			catch (Exception ex)
			{
                Logger.LogException(ex, GetType().ToString(), sendToService: false);
			}
			finally
			{
				IsRefreshing = false;
			}
		}));

		private void ItemSelected(string item)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				if (IsBusy)
				{
					return;
				}

				try
				{
					IsBusy = true;

					await Navigation.PushAsync(ViewContainer.Current.CreatePage<Details.DetailsViewModel>());
				}
				catch (Exception ex)
				{
					ex.Print();
				}
				finally
				{
					IsBusy = false;
				}
			});
		}

		private void LoadItems()
		{
			if (IsBusy)
			{
				return;
			}

			try
			{
				IsBusy = true;

				Items = new ObservableCollection<string>();

				for (int i = 0; i < 21; i++)
				{
					Items.Add($"Item {i}");
				}
			}
			catch (Exception ex)
			{
				ex.Print();
			}
			finally
			{
				IsBusy = false;
			}
		}

		private string selectedItem;
		public string SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;

				if (selectedItem != null)
				{
					ItemSelected(selectedItem);

					SelectedItem = null;
				}
			}
		}

		public ObservableCollection<string> Items { get; private set; }
		public bool IsRefreshing { get; set; }
	}
}
