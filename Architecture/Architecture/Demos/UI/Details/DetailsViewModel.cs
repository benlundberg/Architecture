﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Architecture.Core;
using Xamarin.Forms;

namespace Architecture.Demos.UI.Details
{
	public class DetailsViewModel : BaseViewModel
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

					Items = new ObservableCollection<string>();

					for (int i = 0; i < 20; i++)
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
			});
		}

		public ObservableCollection<string> Items { get; private set; }
	}
}
