using NecDMS.View;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using static NecDMS.Models.LoginModels;

namespace NecDMS.Models
{
	public static class GlobalVariables
	{
		public static string ServerAdresa = Preferences.Get("ApiUrl", string.Empty);
		public static string PrikazDokumenataAdresa = Preferences.Get("PreviewUrl", string.Empty);
		public static List<int> GroupSelectedIndices = null;

		public static KorisnikOsnovniPodaci getOsnovniPodaci()
		{
			return JsonConvert.DeserializeObject<KorisnikOsnovniPodaci>(Preferences.Get("KorisnikOsnovniPodaci", string.Empty));
		}

		public static List<KorisnikPrava> getPravaKorisnika()
		{
			return (List<KorisnikPrava>)JsonConvert.DeserializeObject(Preferences.Get("KorisnikPrava", string.Empty), typeof(List<KorisnikPrava>));
		}


		public static void Odjava()
		{

			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					{
						CrossFirebasePushNotification.Current.Unsubscribe(getOsnovniPodaci().FirmName + "_" + getOsnovniPodaci().idkorisnika.ToString());
						break;
					}

				case Device.Android:
					{
						CrossFirebasePushNotification.Current.Unsubscribe(getOsnovniPodaci().FirmName + "_" + getOsnovniPodaci().idkorisnika.ToString());
						break;
					}
			}
			Preferences.Set("KorisnikOsnovniPodaci", string.Empty);
			//Preferences.Set("KorisnikPrava", string.Empty);
			App.Current.MainPage = new NavigationPage(new Login());

		}


	}
}
