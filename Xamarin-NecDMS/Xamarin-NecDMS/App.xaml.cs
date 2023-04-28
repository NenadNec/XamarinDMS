using Acr.UserDialogs;
using NecDMS.Models;
using NecDMS.View;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NecDMS
{
	public partial class App : Application
	{
		[Obsolete]
		public App()
		{
			InitializeComponent();



			MainPage = Preferences.Get("KorisnikOsnovniPodaci", string.Empty) == string.Empty ? new NavigationPage(new Login()) : new NavigationPage(new GlavniMeni())
			{
				BarBackgroundColor = Color.FromHex("#1b4b76")
			};

			Device.OnPlatform(
				iOS: () => CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh,
				Android: () => CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh
				);

			Device.OnPlatform(
			   iOS: () => CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived,
			   Android: () => CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived
			   );
			Device.OnPlatform(
			   iOS: () => CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened,
			   Android: () => CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened
			   );

			Device.OnPlatform(
			  iOS: () => NotificationCenter.Current.NotificationTapped += Current_NotificationTapped,
			  Android: () => NotificationCenter.Current.NotificationTapped += Current_NotificationTapped
			  );



			Device.OnPlatform(
			   iOS: () => CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened,
			   Android: () => CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened
			   );
			//CrossFirebasePushNotification.Current.Subscribe("all");
			//CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
			//CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
		}

		[Obsolete]
		private void Current_NotificationTapped(NotificationEventArgs e) //klik na lokalnu notifikaciju kada je aplikacija u foreground
		{
			using (var progress = UserDialogs.Instance.Loading("Otvaram poruku..."))
			{
				MessagesModels message = JsonConvert.DeserializeObject<MessagesModels>(e.Request.ReturningData);
				GetMessages(message);
			}
		}

		[Obsolete]
		private void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)//klik na notifikaciju kada je aplikacija u background
		{
			using (var progress = UserDialogs.Instance.Loading("Otvaram poruku..."))
			{
				MessagesModels message = JsonConvert.DeserializeObject<MessagesModels>(e.Data.ToList()[4].Value.ToString());
				GetMessages(message);
			}
		}


		[Obsolete]
		private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
		{

			NotificationRequest message = new NotificationRequest
			{
				Description = e.Data.ToList()[0].Value.ToString(),
				Title = e.Data.ToList()[1].Value.ToString(),
				ReturningData = e.Data.ToList()[3].Value.ToString(),
				//Subtitle = e.Data.ToList()[0].Value.ToString()
			};
			NotificationCenter.Current.Show(message);
		}


		private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
		}

		protected override void OnStart()
		{
			/*using (var progress = UserDialogs.Instance.Loading("Učitavanje..."))
			  {
				  for (var i = 0; i < 100; i++)
				  {
					  progress.PercentComplete = i;

					  await Task.Delay(20);
				  }
			  }*/
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}

		[Obsolete]
		public async void GetMessages(MessagesModels message)
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

			if (App.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(Inbox))
			{
				await App.Current.MainPage.Navigation.PushAsync(new Inbox(message, mRequest), false);
			}
		}
	}
}
