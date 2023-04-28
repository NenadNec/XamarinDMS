using Acr.UserDialogs;
using NecDMS.Interface;
using NecDMS.Interface;
using NecDMS.Interface;
using NecDMS.Models;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace NecDMS.View
{

	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class GlavniMeni : ContentPage
	{
		public GlavniMeni()
		{

			InitializeComponent();
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

		protected override bool OnBackButtonPressed()
		{
			base.OnBackButtonPressed();
			return true;
		}

		#region NavigationPage
		private async void Explorer_Clicked(object sender, EventArgs e)
		{
			using (var progress = UserDialogs.Instance.Loading("Učitavam..."))
			{
				await Navigation.PushAsync(new TabbBarPage(0), false);
			}

		}

		private async void Osnovna_Clicked(object sender, EventArgs e)
		{
			using (var progress = UserDialogs.Instance.Loading("Učitavam..."))
			{
				await Navigation.PushAsync(new TabbBarPage(1), false);
			}
		}

		private async void PoSadrzaju_Clicked(object sender, EventArgs e)
		{
			using (var progress = UserDialogs.Instance.Loading("Učitavam..."))
			{
				await Navigation.PushAsync(new TabbBarPage(2), false);
			}
		}

		private async void DatumIsteka_Clicked(object sender, EventArgs e)
		{
			using (var progress = UserDialogs.Instance.Loading("Učitavam..."))
			{
				await Navigation.PushAsync(new TabbBarPage(3), false);
			}
		}


		private async void OCR_Tapped(object sender, EventArgs e)
		{
			using (var progress = UserDialogs.Instance.Loading("Učitavam..."))
			{
				await Navigation.PushAsync(new TabbBarPage(4), false);
			}
		}


		#endregion NavigationPage

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