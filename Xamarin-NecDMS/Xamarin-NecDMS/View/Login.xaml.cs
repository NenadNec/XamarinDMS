using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using NecDMS.Models;
using static NecDMS.Models.LoginModels;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using System.Net;
using Xamarin.Essentials;

namespace NecDMS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login()
		{
			InitializeComponent();

			if (Device.RuntimePlatform == Device.iOS)
			{
				toolbarItem1.Order = ToolbarItemOrder.Primary;
			}
			else
			{
				toolbarItem1.Order = ToolbarItemOrder.Secondary;
			}

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

			Entry entry = this.FindByName<Entry>("EntryUser");
			entry.Focus();

		}



		#region ButtonLogin
		[Obsolete]
		private async void Button_Login(object sender, EventArgs e)
		{
			try
			{

				using (var progress = UserDialogs.Instance.Loading("Prijava..."))
				{
					var client = new HttpClient();
					string RequestUri = GlobalVariables.ServerAdresa;
					if (RequestUri == string.Empty || RequestUri.Length < 10)
					{
						await DisplayAlert("Unesite ispravnu adresu servera!", "Pokušajte ponovo", "OK");
						return;
					}
					string method = "login?";
					string korisnik = EntryUser.Text;
					string lozinka = EntryPassword.Text;
					//string korisnik = "Admin";
					//string lozinka = " ";
					string url = RequestUri + method + "korisnik=" + korisnik + "&lozinka=" + lozinka;
					HttpResponseMessage response = await client.GetAsync(url);

					if (response.StatusCode == HttpStatusCode.OK)
					{

						string response_string = await response.Content.ReadAsStringAsync();
						//Preferences.Set("KorisnikPrava", response_string);
						List<KorisnikPrava> prava_korisnika = (List<KorisnikPrava>)JsonConvert.DeserializeObject(response_string, typeof(List<KorisnikPrava>));
						GetOsnovniPodaci(korisnik);
					}
					else
					{
						await DisplayAlert("Pokuštajte ponovo", "Korisničko ime ili loznika nisu ispravni !", "OK");
					}
				}
			}

			catch (Exception ex)
			{
				await DisplayAlert("Greška prilikom prijave!", "Pokušajte ponovo", "OK");
			}


		}

		private async void GetOsnovniPodaci(string korisnik)
		{
			var client = new HttpClient();
			string RequestUri = GlobalVariables.ServerAdresa;
			string metoda = "getosnovnipodaci/";
			string uri = RequestUri + metoda + korisnik;
			HttpResponseMessage response = await client.GetAsync(uri);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				string response_string = await response.Content.ReadAsStringAsync();
				KorisnikOsnovniPodaci osnovni_podaci = JsonConvert.DeserializeObject<KorisnikOsnovniPodaci>(response_string);
				Preferences.Set("KorisnikOsnovniPodaci", response_string);

				switch (Device.RuntimePlatform)
				{
					case Device.iOS:
						{
							CrossFirebasePushNotification.Current.Subscribe(osnovni_podaci.FirmName + "_" + osnovni_podaci.idkorisnika.ToString());
							break;
						}

					case Device.Android:
						{
							CrossFirebasePushNotification.Current.Subscribe(osnovni_podaci.FirmName + "_" + osnovni_podaci.idkorisnika.ToString());
							break;
						}
				}
				await Navigation.PushAsync(new GlavniMeni(), false);
			}
			else
			{
				await DisplayAlert("Pokuštajte ponovo", "Korisničko ime ili loznika nisu ispravni !", "OK");
			}

		}

		private async void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new Podesavanja(), false);
		}

		#endregion ButtonLogin


	}
}