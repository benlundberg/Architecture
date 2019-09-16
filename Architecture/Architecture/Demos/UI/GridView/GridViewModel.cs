using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture.Demos.UI.GridView
{
	public class GridViewModel : BaseViewModel
	{
		public override void Appearing()
		{
			if (NetStatusHelper.IsConnected)
			{
				LoadItems();
			}
			else
			{
				ShowNoNetworkError();
			}

			base.Appearing();
		}

		private ICommand selectItemCommand;
		public ICommand SelectItemCommand => selectItemCommand ?? (selectItemCommand = new Command(async (param) =>
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
		}));

		private void LoadItems()
		{
			if (IsBusy)
			{
				return;
			}

			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					IsBusy = true;

					await Task.Delay(TimeSpan.FromSeconds(2));

					var items = new ObservableCollection<string>();

					for (int i = 0; i < 20; i++)
					{
						items.Add($"Item {i}");
					}

                    Items = new ObservableCollection<string>(items);
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

		public ObservableCollection<string> Items { get; private set; }
	}
}
