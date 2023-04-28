using Acr.UserDialogs;
using NecDMS.Interface;
using NecDMS.Models;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace NecDMS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TabbBarPage : TabbedPage
	{
		List<MessagesModels> ListItems = new List<MessagesModels>();
		int BellCount = 0;
		public TabbBarPage(int PageIndex)
		{
			InitializeComponent();

			BarBackgroundColor = Color.FromHex("#1b4b76");

			var firstPage = Children[PageIndex];
			CurrentPage = firstPage;

			On<Android>().SetOffscreenPageLimit(5); //definise se broj tabova kako bi bili u memoriji kako bi se brze prelazilo sa tab na tab


			On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					{
						toolbar_item_settings.Order = ToolbarItemOrder.Primary;
						title_view.IsVisible = true;
						ToolbarItems.Remove(ToolbarItems[0]);
						break;
					}

				case Device.UWP:
					{
						toolbar_item_settings.Order = ToolbarItemOrder.Secondary;
						title_view.IsVisible = true;
						ToolbarItems.Remove(ToolbarItems[0]);
						break;
					}

				case Device.Android:
					{
						toolbar_item_settings.Order = ToolbarItemOrder.Secondary;
						title_view.IsVisible = false;
						break;
					}
			}


		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			//BellMessagesNotify();
			SetBadgeCount();
		}

		protected override bool OnBackButtonPressed()
		{
			Navigation.PushAsync(new GlavniMeni());

			return true;

		}

		protected override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();
			if (Device.RuntimePlatform == Device.Android)
			{
				SetBadgeCount();
			}
		}

		private async void SetBadgeCount()
		{
			var client = new HttpClient();
			string RequestUri = GlobalVariables.ServerAdresa;
			string method = "GetBrojPorukaNP";
			int IdKor = GlobalVariables.getOsnovniPodaci().idkorisnika;
			string url = RequestUri + method + "/" + IdKor;
			var response = await client.GetStringAsync(url);
			if (ToolbarItems.Count > 0)
			{
				if (Device.RuntimePlatform == Device.Android)
				{
					DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems[0], response, Color.Red, Color.White);
				}
				else
				{
					labelText.Text = response;
				}

			}

		}

		[Obsolete]
		private async void Odjava_Clicked(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("", "Da li zaista želite da se odjavite ?", "Da", "Ne");
			if (answer)
			{
				GlobalVariables.Odjava();
				//await Navigation.PopToRootAsync(false);
			}
		}

		[Obsolete]
		private async void BellToolbarItem_Clicked(object sender, EventArgs e)
		{
			MessageRequest mRequest = new MessageRequest()
			{

				BrojZapisa = 25,
				TrenutnaStrana = 0,
				IDDir = null,
				IDKor = GlobalVariables.getOsnovniPodaci().idkorisnika,
				IDGrupe = GlobalVariables.getOsnovniPodaci().IDGrupe.ToString(),
				ParentID = GlobalVariables.getOsnovniPodaci().ParentID,
				term = "null",
				type = "bell"

			};
			await Navigation.PushAsync(new Inbox(null, mRequest), false);
		}

		[Obsolete]
		private async void BellImage_Clicked(object sender, EventArgs e)
		{
			MessageRequest mRequest = new MessageRequest()
			{

				BrojZapisa = 25,
				TrenutnaStrana = 0,
				IDDir = null,
				IDKor = GlobalVariables.getOsnovniPodaci().idkorisnika,
				IDGrupe = GlobalVariables.getOsnovniPodaci().IDGrupe.ToString(),
				ParentID = GlobalVariables.getOsnovniPodaci().ParentID,
				term = "null",
				type = "bell"

			};
			await Navigation.PushAsync(new Inbox(null, mRequest), false);
		}


		
	}
}