using Acr.UserDialogs;
using NecDMS.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static NecDMS.Models.LoginModels;

namespace NecDMS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PretragaDokumentaView : ContentPage
	{

		KorisnikOsnovniPodaci korisnik = GlobalVariables.getOsnovniPodaci();
		private ObservableCollection<PretragaDokumentaResponseModels> listItems = new ObservableCollection<PretragaDokumentaResponseModels>();
		private PretragaDokumentaRequestModels request_object = new PretragaDokumentaRequestModels();
		private PretragaDokumentaResponseModels response_object = new PretragaDokumentaResponseModels();
		private int load_counter = 0;
		private int results_found = 0;

		public bool NavVisible { get; set; }

		public PretragaDokumentaView(PretragaDokumentaRequestModels request, List<PretragaDokumentaResponseModels> responseList)
		{

			InitializeComponent();



			if (request != null)
			{
				request_object = request;
				CountItemsAsync(request_object);
				_ = GetItemsAsync(request_object);
			}
			else if (responseList != null)
			{
				title.Text = "Prikazano: " + responseList.Count.ToString() + " od " + responseList.Count;
				foreach (var s in responseList)
				{
					listItems.Add(s);
				}

			}

			collection.ItemsSource = listItems;
			collection.RemainingItemsThreshold = 5;
			collection.RemainingItemsThresholdReached += Collection_RemainingItemsThresholdReached;
		}

		protected override bool OnBackButtonPressed()
		{
			 Navigation.PushAsync(new TabbBarPage(1), false);

			return true;
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



        #region ListaDokumenata

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (collection.SelectedItem != null)
				{
					var detalji = collection.SelectedItem as PretragaDokumentaResponseModels;
					if (Convert.ToInt32(detalji.BrojFajlova) > 0)
					{
						var client = new HttpClient();
						string RequestUri = GlobalVariables.ServerAdresa;
						string metoda = "Thumb/";

						int dokument = detalji.IDN;
						string url = RequestUri + metoda + dokument + "/" + korisnik.idkorisnika;
						var response = await client.GetStringAsync(url);
						List<Thumb> ListItems = (List<Thumb>)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(List<Thumb>));
						await Navigation.PushAsync(new PretregaDokumenataDetail(ListItems, detalji), false);
						collection.SelectedItem = null;
					}
					else
					{
						await DisplayAlert("", "Odabrani dokument ne sadrži fajlove !", "OK");
						collection.SelectedItem = null;
					}


				}
			}
			catch (Exception ex)
			{

				await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");

			}
		}
		#endregion ListaDokumenta


		#region ButtonPretrazi
		private async Task GetItemsAsync(PretragaDokumentaRequestModels request)
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

					switch (request.type)
					{

						case "osnovna":

							method = "GetDokumentiPretraga";
							break;


						case "istek":

							method = "GetDokumentiPretraga";
							break;

						case "sadrzaj":

							method = "GetDokumentiPretraga";
							break;

						case "":

							method = "GetDokumentiDyn";
							break;

						case "ocr":

							method = "GetDokumentiPretraga";
							break;
					}


					string url = RequestUri + method;

					HttpResponseMessage response = await client.PostAsync(url, content);
					if (response.StatusCode == HttpStatusCode.OK)
					{

						var result = await response.Content.ReadAsStringAsync();


						List<PretragaDokumentaResponseModels> responseList = (List<PretragaDokumentaResponseModels>)Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(List<PretragaDokumentaResponseModels>));

						foreach (var s in responseList)
						{
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
				}
			}

			catch (Exception ex)
			{
				await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
			}

		}


		private async void CountItemsAsync(PretragaDokumentaRequestModels request)
		{

			try
			{
				var client = new HttpClient();
				string RequestUri = GlobalVariables.ServerAdresa;
				string method = "";
				var jsonData = JsonConvert.SerializeObject(request);
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

				switch (request.type)
				{

					case "osnovna":

						method = "GetBrojDokPretraga";
						break;


					case "istek":

						method = "GetBrojDokPretraga";
						break;

					case "sadrzaj":

						method = "GetBrojDokPretraga";
						break;

					case "":

						method = "GetBrojDok";
						break;

					case "ocr":

						method = "GetBrojDokPretraga";
						break;

				}

				string url = RequestUri + method;
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
			}

			catch (Exception ex)
			{
				return;
			}

		}


        #endregion ButtonPretrazi

        #region ViewRelated
        private async void ViewRelated(object sender, EventArgs e)
		{
			SwipeItem item = sender as SwipeItem;
			var clickedItem = item.BindingContext as PretragaDokumentaResponseModels;
			if (clickedItem.PodDokumenti != null)
			{
				if (clickedItem.PodDokumenti.Count != 0)
				{
					await Navigation.PushAsync(new PretragaDokumentaView(null, clickedItem.PodDokumenti), false);
				}
				else
				{
					await DisplayAlert("", "Izabrani dokument ne sadrži poddokumente !", "OK");
				}
			}
			else 
			{
					await DisplayAlert("", "Izabrani dokument ne sadrži poddokumente !", "OK");
			}



		}

        #endregion ViewRelated

        #region SendDucument

        private async void SendDocument(object sender, EventArgs e)
		{
			SwipeItem item = sender as SwipeItem;
			PretragaDokumentaResponseModels clickedItem = item.BindingContext as PretragaDokumentaResponseModels;
			if (clickedItem.StatusWF == 1)
			{
				await DisplayAlert("", "Nije moguće proslediti izabrani dokument. Za ovaj dokument je pokrenut workflow !", "OK");
			}
			else
			{
				await Navigation.PushModalAsync(new SandDocumentView(clickedItem));
			}

		}

        #endregion SendDocument

        #region VerifiyDocument
        private async void VerifyDocument(object sender, EventArgs e)
		{
			SwipeItem item = sender as SwipeItem;
			var clickedItem = item.BindingContext as PretragaDokumentaResponseModels;

			if (clickedItem.StatusWF == 1)
			{
				try
				{

					var client = new HttpClient();
					string RequestUri = GlobalVariables.ServerAdresa;
					string metoda = "GetStanjePromenaFaze/";

					var dokument = clickedItem;
					string url = RequestUri + metoda + dokument.IDN + "/" + korisnik.idkorisnika;

					HttpResponseMessage response = await client.GetAsync(url);

					if (response.StatusCode == HttpStatusCode.OK)
					{
						var response_string = await response.Content.ReadAsStringAsync();
						List<Stanje> result = (List<Stanje>)JsonConvert.DeserializeObject(response_string, typeof(List<Stanje>));
						List<int> zaduzeni = result[0].Zaduzen.Split(',').Select(int.Parse).ToList();
						if (zaduzeni.Contains(korisnik.idkorisnika))
						{
							string temp = await OverioDokument(dokument);
							if (temp == "")
							{
								await DisplayAlert("", "Nemate dozvolu za ovu akciju !", "OK");
							}
							else if (Convert.ToInt32(temp) == 0)
							{
								await Navigation.PushModalAsync(new VerificationDocView(result[0], dokument));
							}
							else
							{
								await DisplayAlert("", "Nemate dozvolu za ovu akciju !", "OK");
							}
						}
						else
						{
							await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

			}
			else
			{
				await DisplayAlert("", "Nemate dozvolu za ovu akciju !", "OK");
			}
		}

        #endregion VerifyDoucment

        #region OverioDokument
        private async Task<string> OverioDokument(PretragaDokumentaResponseModels dokument)
		{
			string result_string = "";
			try
			{

				var client = new HttpClient();
				string RequestUri = GlobalVariables.ServerAdresa;
				string metoda = "/GetOverio/";

				string url = RequestUri + metoda + dokument.IDN + "/" + korisnik.idkorisnika;

				HttpResponseMessage response = await client.GetAsync(url);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var response_string = await response.Content.ReadAsStringAsync();
					return result_string = (string)JsonConvert.DeserializeObject(response_string, typeof(string));

				}
				else
				{
					return "";
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "";
			}

		}

        #endregion OverioDokument

        #region BtnSend
        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
			Button item = sender as Button;
			PretragaDokumentaResponseModels clickedItem = item.BindingContext as PretragaDokumentaResponseModels;
			if (clickedItem.StatusWF == 1)
			{
				await DisplayAlert("", "Nije moguće proslediti izabrani dokument. Za ovaj dokument je pokrenut workflow !", "OK");
			}
			else
			{
				await Navigation.PushModalAsync(new SandDocumentView(clickedItem));
			}
		}

        #endregion BtnSend

        #region BtnRelated
        private async void BtnRelated_Clicked(object sender, EventArgs e)
        {
			Button item = sender as Button;
			var clickedItem = item.BindingContext as PretragaDokumentaResponseModels;
			if (clickedItem.PodDokumenti != null)
			{
				if (clickedItem.PodDokumenti.Count != 0)
				{
					await Navigation.PushAsync(new PretragaDokumentaView(null, clickedItem.PodDokumenti), false);
				}
				else
				{
					await DisplayAlert("", "Izabrani dokument ne sadrži poddokumente !", "OK");
				}
			}
			else
			{
				await DisplayAlert("", "Izabrani dokument ne sadrži poddokumente !", "OK");
			}
		}

        #endregion BtnRelated

        #region BtnVerify
        private async void Btnverify_Clicked(object sender, EventArgs e)
        {
			Button item = sender as Button;
			var clickedItem = item.BindingContext as PretragaDokumentaResponseModels;

			if (clickedItem.StatusWF == 1)
			{
				try
				{

					var client = new HttpClient();
					string RequestUri = GlobalVariables.ServerAdresa;
					string metoda = "GetStanjePromenaFaze/";

					var dokument = clickedItem;
					string url = RequestUri + metoda + dokument.IDN + "/" + korisnik.idkorisnika;

					HttpResponseMessage response = await client.GetAsync(url);

					if (response.StatusCode == HttpStatusCode.OK)
					{
						var response_string = await response.Content.ReadAsStringAsync();
						List<Stanje> result = (List<Stanje>)JsonConvert.DeserializeObject(response_string, typeof(List<Stanje>));
						List<int> zaduzeni = result[0].Zaduzen.Split(',').Select(int.Parse).ToList();
						if (zaduzeni.Contains(korisnik.idkorisnika))
						{
							string temp = await OverioDokument(dokument);
							if (temp == "")
							{
								await DisplayAlert("", "Nemate dozvolu za ovu akciju !", "OK");
							}
							else if (Convert.ToInt32(temp) == 0)
							{
								await Navigation.PushModalAsync(new VerificationDocView(result[0], dokument));
							}
							else
							{
								await DisplayAlert("", "Nemate dozvolu za ovu akciju !", "OK");
							}
						}
						else
						{
							await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

			}
			else
			{
				await DisplayAlert("", "Nemate dozvolu za ovu akciju !", "OK");
			}
		}
        #endregion BtnVerify


    }
}