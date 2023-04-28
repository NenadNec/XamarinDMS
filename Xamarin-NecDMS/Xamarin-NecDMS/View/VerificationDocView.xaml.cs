using NecDMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
	public partial class VerificationDocView : ContentPage
	{
		public Stanje state = new Stanje();
		private PretragaDokumentaResponseModels document = new PretragaDokumentaResponseModels();
		KorisnikOsnovniPodaci korisnik = GlobalVariables.getOsnovniPodaci();
		public VerificationDocView(Stanje stanje, PretragaDokumentaResponseModels dokument)
		{
			InitializeComponent();
			state = stanje;
			document = dokument;
		}

		private async void Potvrdi_Clicked(object sender, EventArgs e)
		{
			string akcija = "";
			if (obradaCheckBox.IsChecked == false && overiCheckBox.IsChecked == false)
			{
				await DisplayAlert("", "Morate odabrati jednu od ponuđenih akcija !", "OK");
			}
			else
			{
				if (obradaCheckBox.IsChecked)
				{
					akcija = "vratite na obradu";
				}
				else
				{
					akcija = "overite";
				}
				var answer = await DisplayAlert("", "Da li zaista želite da " + akcija + " odabrani dokument ?", "Da", "Ne");

				if (answer)
				{
					if (overiCheckBox.IsChecked)
					{
						_ = NextPhase();
						await Navigation.PopModalAsync();
					}
					else if (obradaCheckBox.IsChecked)
					{
						_ = PreviousPhase();
						await Navigation.PopModalAsync();
					}
					else
					{
						await DisplayAlert("", "Morate odabrati jednu od ponuđenih akcija !", "OK");
					}

				}
			}


		}

		private async void Odustani_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}

		private void obradaCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (obradaCheckBox.IsChecked)
			{
				overiCheckBox.IsChecked = false;
			}

		}

		private void overiCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (overiCheckBox.IsChecked)
			{
				obradaCheckBox.IsChecked = false;
			}

		}


		public async Task NextPhase()
		{
			try
			{
				var IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault();
				string userIpAddress = IpAddress != null ? IpAddress.ToString() : "mobile ip nije pročitana";

				int ID_Instance = state.ID_AlternativneAkcije;
				int Rb_Stanja = Convert.ToInt32(state.RM1);
				int podfaza = state.podfaza;

				var client = new HttpClient();
				string RequestUri = GlobalVariables.ServerAdresa;
				string poruka = napomena.Text != null ? "Napomena: " + napomena.Text : "Napomena: ";
				string url = RequestUri + "SledecaFaza?ID_Instance=" + ID_Instance + "&Rb_Stanja=" + Rb_Stanja + "&idKorisnika=" + korisnik.idkorisnika + "&userIpAddress=" + userIpAddress + "&doc=" + document.IDN + "&naslov=WF" + "&poruka=" + poruka + "&paralelno=" + state.RM2 + "&podfaza=" + podfaza;

				HttpResponseMessage response = await client.GetAsync(url);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var response_string = await response.Content.ReadAsStringAsync();
					List<PretragaDokumentaResponseModels> result = (List<PretragaDokumentaResponseModels>)JsonConvert.DeserializeObject(response_string, typeof(List<PretragaDokumentaResponseModels>));
					if (result.Count != 0 && result[0].BrojPovezanih != 0)
					{
						if (result[0].BrojPovezanih == 5)
						{
							await DisplayAlert("", "Uspešno upisano! Poruka je uspešno poslata! Naredna faza će se pokrenuti nakon što svi korisnici završe rad na trenutnoj fazi!", "OK");

						}
						if (result[0].BrojPovezanih == 6)
						{

							await DisplayAlert("", "Uspešno upisano! Naredna faza će se pokrenuti nakon što svi korisnici završe rad na trenutnoj fazi!", "OK");
						}
						if (result[0].BrojPovezanih == 1)
						{
							await DisplayAlert("", "Uspešno ste odobrili, wf je završen!", "OK");
							_ = SendMailToUserWF(result);
						}
					}
					else
					{
						if (result.Count > 0)
						{

							await DisplayAlert("", "Uspešno ste odobrili i prebacili u sledeću fazu !", "OK");
							_ = SendMailToUserWF(result);
						}
						else
						{
							await DisplayAlert("", "Došlo je do greške pri promeni faze, kontaktirajte administratora.", "OK");
						}
					}
				}
				else
				{
					await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public async Task PreviousPhase()
		{
			try
			{
				int Rb_Stanja = Convert.ToInt32(state.RM1);
				if (Rb_Stanja > 1)
				{
					Rb_Stanja = Rb_Stanja - 1;
					var IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault();
					string userIpAddress = IpAddress != null ? IpAddress.ToString() : "mobile ip nije pročitana";

					int ID_Instance = state.ID_AlternativneAkcije;
					string poruka = napomena.Text != null ? "Napomena: " + napomena.Text : "Napomena: ";

					var client = new HttpClient();
					string RequestUri = GlobalVariables.ServerAdresa;

					string url = RequestUri + "prethodnaFaza?ID_Instance=" + ID_Instance + "&Rb_Stanja=" + Rb_Stanja + "&idKorisnika=" + korisnik.idkorisnika + "&userIpAddress=" + userIpAddress + "&naslov=WF" + "&poruka=" + poruka + "&doc=" + document.IDN;

					HttpResponseMessage response = await client.GetAsync(url);

					if (response.StatusCode == HttpStatusCode.OK)
					{
						var response_string = await response.Content.ReadAsStringAsync();
						List<PretragaDokumentaResponseModels> result = (List<PretragaDokumentaResponseModels>)JsonConvert.DeserializeObject(response_string, typeof(List<PretragaDokumentaResponseModels>));
						if (result.Count > 0)
						{
							await DisplayAlert("", "Uspešno ste vratili dokument u predhodnu fazu !", "OK");
							_ = SendMailToUserWF(result);
						}
						else
						{
							await DisplayAlert("", "Došlo je do greške pri promeni faze, kontaktirajte administratora.", "OK");
						}
					}
					else
					{
						await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
					}

				}
				else
				{
					await DisplayAlert("", "Ne možete vratiti dokument u predhodnu fazu, ovo je prva faza !", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}





		public async Task SendMailToUserWF(List<PretragaDokumentaResponseModels> data)
		{

			try
			{
				if (data.Count != 0 && data[0].BrojPovezanih != 0)
				{
					if (data[0].BrojPovezanih == 1)
					{
						_ = SendMailToCreatorWF();
					}
				}
				else
				{

					if (data.Count() != 0)
					{
						List<int> zaduzenoLiceFaza1 = data[0].Korisnik.Split(',').Select(int.Parse).ToList();

						SlanjeEmailaModel request = new SlanjeEmailaModel();

						request.WF = false;
						request.file = 0;
						request.ID_Posiljaoca = korisnik.idkorisnika;
						request.idnDok = data[0].IDN;
						request.listaIDNKor = zaduzenoLiceFaza1;
						request.vrstaDok = data[0].Vrsta;
						request.statusWF = data[0].StatusWF;
						request.Naslov = "WF";
						request.Tekst = napomena.Text != null ? "Napomena: " + napomena.Text : "Napomena: ";
						request.OriginLink = GlobalVariables.PrikazDokumenataAdresa;
						request.ime = korisnik.Ime;
						request.prezime = korisnik.Prezime;

						var client = new HttpClient();
						string RequestUri = GlobalVariables.ServerAdresa;
						string metoda = "SendEmailToUserWF";

						string url = RequestUri + metoda;
						var jsonData = JsonConvert.SerializeObject(request);
						var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
						HttpResponseMessage response = await client.PostAsync(url, content);
						if (response.StatusCode == HttpStatusCode.OK)
						{
							var response_string = await response.Content.ReadAsStringAsync();
							List<Korisnici1> result = (List<Korisnici1>)JsonConvert.DeserializeObject(response_string, typeof(List<Korisnici1>));
						}
						else
						{
							await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
						}
					}

					else
					{
						
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
		}

		public async Task SendMailToCreatorWF()
		{
			try
			{
				List<int> list = new List<int>();
				list.Add(state.KreatorID);
				string poruka = " <p> Poštovani,</p></br><p>Workflow za dokument " + state.Naziv_Akcije + " je završen!</p> </br> ";

				SlanjeEmailaModel request = new SlanjeEmailaModel();

				request.ID_Posiljaoca = korisnik.idkorisnika;
				request.listaIDNKor = list;
				request.Naslov = "WF";
				request.Tekst = poruka;
				request.ime = korisnik.Ime;
				request.prezime = korisnik.Prezime;


				var client = new HttpClient();
				string RequestUri = GlobalVariables.ServerAdresa;
				string metoda = "SendMailToCreatorWF";

				string url = RequestUri + metoda;
				var jsonData = JsonConvert.SerializeObject(request);
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync(url, content);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					var response_string = await response.Content.ReadAsStringAsync();
					bool result = (bool)JsonConvert.DeserializeObject(response_string, typeof(bool));
				}
				else
				{
					await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
		}
	}
}