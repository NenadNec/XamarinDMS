using NecDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace NecDMS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]


	public partial class Inbox
	{
		private ObservableCollection<MessagesModels> listItems { get; set; } = new ObservableCollection<MessagesModels>();
		MessageRequest request_object = new MessageRequest();
		private int load_counter = 0;
		private int results_found = 0;

		public ObservableCollection<string> MyItems { get; set; }

		[Obsolete]
		public Inbox(MessagesModels message = null, MessageRequest request = null)

		{

			InitializeComponent();
			if (request != null)
			{
				request_object = request;
				CountItemsAsync(request_object);
				_ = GetItemsAsync(request_object);
			}


			OpenMessage(message);

			collection.ItemsSource = listItems;
			collection.RemainingItemsThreshold = 5;
			collection.RemainingItemsThresholdReached += Collection_RemainingItemsThresholdReached;
		}

		public void RefreshMe(MessagesModels item)
		{
			listItems[listItems.IndexOf(item)] = item;
		}

		private void Collection_RemainingItemsThresholdReached(object sender, EventArgs e)
		{
			try
			{

				if (listItems.Count < results_found)
				{




					if (IsBusy)
						return;

					this.IsBusy = true;
					_ = GetItemsAsync(request_object);


				}
				else
				{
					return;
				}
			}

			catch { }

		}

		private async void CountItemsAsync(MessageRequest request)
		{

			try
			{
				MessageCountRequest msg_count = new MessageCountRequest();
				msg_count.IDN = request.IDDir;
				msg_count.ID_Posiljaoca = request.IDKor;
				msg_count.Tekst = "null";


				var client = new HttpClient();
				string RequestUri = GlobalVariables.ServerAdresa;
				string method = "";
				var jsonData = JsonConvert.SerializeObject(msg_count);
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
				string url = "";

				switch (request.type)
				{

					case "bell":
						{
							method = "GetBrojPorukeAll";
							url = RequestUri + method + "?idkor=" + request.IDKor;
							HttpResponseMessage response = await client.GetAsync(url);

							if (response.StatusCode == HttpStatusCode.OK)
							{
								var result = await response.Content.ReadAsStringAsync();
								results_found = Convert.ToInt32(result);
							}
							else
							{
								return;
							}
							break;
						}


					case "explorer":
						{
							method = "GetBrojPoruka";
							url = RequestUri + method;
							HttpResponseMessage response = await client.PostAsync(url, content);
							if (response.StatusCode == HttpStatusCode.OK)
							{
								var result = await response.Content.ReadAsStringAsync();
								results_found = Convert.ToInt32(result);

							}
							else
							{
								return;
							}
							break;
						}
				}
			}

			catch (Exception ex)
			{
				return;
			}

		}


		private async Task GetItemsAsync(MessageRequest request)
		{

			try
			{
				using (UserDialogs.Instance.Loading("Učitavam..."))
				{

					request.TrenutnaStrana = load_counter;
					var client = new HttpClient();
					string RequestUri = GlobalVariables.ServerAdresa;
					string method = "";
					var jsonData = JsonConvert.SerializeObject(request);
					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
					string url = "";

					switch (request.type)
					{

						case "bell":
							{
								method = "GetPorukeAll";
								url = RequestUri + method + "?idkor=" + request.IDKor + "&BrojZapisa=" + request.BrojZapisa + "&strana=" + request.TrenutnaStrana;
								HttpResponseMessage response = await client.GetAsync(url);

								if (response.StatusCode == HttpStatusCode.OK)
								{

									var result = await response.Content.ReadAsStringAsync();
									List<MessagesModels> responseList = (List<MessagesModels>)JsonConvert.DeserializeObject(result, typeof(List<MessagesModels>));
									foreach (var s in responseList)
									{
										switch (Device.RuntimePlatform)
										{
											case Device.iOS:
												{
													s.imgSrc = s.Procitana ? "openMessage.png" : "closeMessage.png";
													break;
												}

											case Device.Android:
												{
													s.imgSrc = s.Procitana ? "Assets/openMessage.png" : "Assets/closeMessage.png";
													break;
												}
											case Device.UWP:
												{
													s.imgSrc = s.Procitana ? "Assets/openMessage.png" : "Assets/closeMessage.png";
													break;
												}
										}
										s.shortText = s.Tekst.Length <= 30 ? s.Tekst : s.Tekst.Substring(0, 30) + " ...";

										listItems.Add(s);

									}
									if (load_counter == 0)
									{
										title.Text = results_found < request.BrojZapisa ? ("Prikazano: " + results_found.ToString() + " od " + results_found) : ("Prikazano: " + listItems.Count + " od " + results_found);

									}
									else
									{
										title.Text = results_found < request.BrojZapisa ? ("Prikazano: " + results_found.ToString() + " od " + results_found) : ("Prikazano: " + listItems.Count + " od " + results_found);

									}
									load_counter++;
									IsBusy = false;
								}
								else
								{
									await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
								}



								break;
							}




						case "explorer":
							{
								method = "GetPorukeDyn";
								url = RequestUri + method;

								HttpResponseMessage response = await client.PostAsync(url, content);

								if (response.StatusCode == HttpStatusCode.OK)
								{
									var result = await response.Content.ReadAsStringAsync();

									List<MessagesModels> responseList = (List<MessagesModels>)JsonConvert.DeserializeObject(result, typeof(List<MessagesModels>));
									foreach (var s in responseList)
									{
										switch (Device.RuntimePlatform)
										{
											case Device.iOS:
												{
													s.imgSrc = s.Procitana ? "openMessage.png" : "closeMessage.png";
													break;
												}

											case Device.Android:
												{
													s.imgSrc = s.Procitana ? "Assets/openMessage.png" : "Assets/closeMessage.png";
													break;
												}
											case Device.UWP:
												{
													s.imgSrc = s.Procitana ? "Assets/openMessage.png" : "Assets/closeMessage.png";
													break;
												}
										}
										s.shortText = s.Tekst.Length <= 30 ? s.Tekst : s.Tekst.Substring(0, 30) + " ...";

										listItems.Add(s);

									}
									if (load_counter == 0)
									{
										title.Text = results_found < request.BrojZapisa ? ("Prikazano: " + results_found.ToString() + " od " + results_found) : ("Prikazano: " + listItems.Count + " od " + results_found);

									}
									else
									{
										title.Text = results_found < request.BrojZapisa ? ("Prikazano: " + results_found.ToString() + " od " + results_found) : ("Prikazano: " + listItems.Count + " od " + results_found);

									}
									load_counter++;
									IsBusy = false;
								}
								else
								{
									await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
								}
								break;
							}
					}
				}
			}

			catch (Exception ex)
			{
				await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
			}

		}





		protected override bool OnBackButtonPressed()
		{
			Navigation.PushAsync(new GlavniMeni());

			return true;

		}


		#region MessageList
		[Obsolete]
		private async void ListaPoruka_SelectionChanged(object sender, ItemTappedEventArgs e)
		{

			try
			{


				if (collection.SelectedItem != null)
				{

					var detalji = collection.SelectedItem as MessagesModels;
					if (!detalji.Procitana)
					{
						MakeAsRead(detalji);
					}
					await PopupNavigation.Instance.PushAsync(new popuplayout(detalji), false);
					collection.SelectedItem = null;
				}
			}
			catch (Exception ex)
			{

				await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");

			}

		}

		protected async void OpenMessage(MessagesModels message)
		{
			if (message != null)
			{
				await PopupNavigation.Instance.PushAsync(new popuplayout(message), false);
				MakeAsRead(message);
			}

		}

		protected async void MakeAsRead(MessagesModels message)
		{
			MessageReadRequest request = new MessageReadRequest()
			{
				IDN = message.IDN
			};

			var client2 = new HttpClient();
			string RequestUri = GlobalVariables.ServerAdresa;
			string metoda = "PreuzmiPoruke";

			string url = RequestUri + metoda;
			var jsonData = JsonConvert.SerializeObject(request);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client2.PostAsync(url, content);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				var result = await response.Content.ReadAsStringAsync();
				var selected_message = listItems.FirstOrDefault(x => x.IDN == message.IDN);
				if (selected_message != null)
				{
					selected_message.Procitana = true;
				}
				switch (Device.RuntimePlatform)
				{
					case Device.iOS:
						{
							if (selected_message != null)
							{
								selected_message.imgSrc = "openMessage.png";
							}
							break;
						}

					case Device.Android:
						{
							if (selected_message != null)
							{
								selected_message.imgSrc = "Assets/openMessage.png";
							}
							break;
						}

					case Device.UWP:
						{
							if (selected_message != null)
							{
								selected_message.imgSrc = "Assets/openMessage.png";
							}
							break;
						}
				}
				RefreshMe(selected_message);
			}
		}


		#endregion MessageList


	}
}