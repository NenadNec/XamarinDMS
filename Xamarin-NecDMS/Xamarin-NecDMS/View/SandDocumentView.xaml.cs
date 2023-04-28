using Acr.UserDialogs;
﻿using NecDMS.Models;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static NecDMS.Models.SlanjeDokumenataModel;

namespace NecDMS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SandDocumentView : ContentPage
    {
        private PretragaDokumentaResponseModels document = new PretragaDokumentaResponseModels();
        private List<Thumb> thumbList = new List<Thumb>();
        private  List<Thumb> finalThumbList = new List<Thumb>();
        public SandDocumentView(PretragaDokumentaResponseModels selectedDocument)
        {
            InitializeComponent();
            document = selectedDocument;
            _ = GeThumb();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _ = GetGrupe();
            pickerGrupe.SelectedIndices = pickerGrupe.SelectedIndices;

            _ = GetKorisnici();
            pickerKorisnici.SelectedIndices = pickerKorisnici.SelectedIndices;


            if (pickerGrupe.SelectedIndices != null && pickerGrupe.SelectedIndices != GlobalVariables.GroupSelectedIndices)
            {
                var items = pickerKorisnici.ItemsSource;
                pickerKorisnici.ItemsSource = pickerKorisnici.ItemsSource.DistinctBy(x =>x.key).ToList();


                var positionSelectedGrupe = pickerGrupe.SelectedIndices;
                var selektovaneGrupe = pickerGrupe.ItemsSource.Where((c, index) => positionSelectedGrupe.Contains(index)).ToList();

                var grupeIDS = selektovaneGrupe.Select(a => a.key).ToList();

                var temp = pickerKorisnici.ItemsSource.Where((c, index) => grupeIDS.Any(x => c.key_group.Any(y => y == x))).Select(q => pickerKorisnici.ItemsSource.IndexOf(q)).ToList();
                pickerKorisnici.SelectedIndices = temp;

                GlobalVariables.GroupSelectedIndices = pickerGrupe.SelectedIndices;
            }

        }

        private async void  Potvrdi_Clicked(object sender, EventArgs e)
        {
            if (pickerKorisnici.SelectedIndices != null)
            {
                var answer = await DisplayAlert("", "Da li zaista želite da prosledite odabrani dokument ?", "Da", "Ne");
                if (answer)
                {
                    int id_vrste = await GetVrstaID();
                    int new_id = 0;

                    if (id_vrste > 0)
                    {
                        new_id = await SendDocument(id_vrste);
                    }
                    else
                    {
                        await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
                    }
                    if (new_id > 0)
                    {
                        await SendEmailToUser(new_id);
                    }
                    else
                    {
                        await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
                    }
                }
            }
            else 
            {
                await DisplayAlert("Validacija!", "Odaberite korisnika", "OK");
            }
        }

        private async void Odustani_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async Task GetGrupe()
        {
            var client = new HttpClient();
            string RequestUri = GlobalVariables.ServerAdresa;
            string method = "GetGrupe";
            string url = RequestUri + method;
            var response = await client.GetStringAsync(url);
            
            List<Grupe> listItems = (List<Grupe>)JsonConvert.DeserializeObject(response, typeof(List<Grupe>));

            List<KeyValue> result = listItems.Select(x => new KeyValue { key = x.IDGrupe, value = x.Naziv }).ToList();
            pickerGrupe.ItemsSource = result;

        }


        private async Task GetKorisnici()
        {
            var client = new HttpClient();
            string RequestUri = GlobalVariables.ServerAdresa;
            string method = "GetKorisniciSlanja?idkorisnika="+ GlobalVariables.getOsnovniPodaci().idkorisnika;
            string url = RequestUri + method;
            var response = await client.GetStringAsync(url);
           
            List<Korisnici> listItems = (List<Korisnici>)JsonConvert.DeserializeObject(response, typeof(List<Korisnici>));

            List<KeyValue> result = listItems.GroupBy(c => c.IDKorisnika).Select(x => new KeyValue { key = x.Key, value = x.Select(l => l.SrednjeIme).First(), key_group = x.Select(k =>k.IDGrupe).ToList()}).ToList();
            pickerKorisnici.ItemsSource = result;
        }

        private async Task GeThumb()
        {
            var client = new HttpClient();
            string RequestUri = GlobalVariables.ServerAdresa;
            string method = "Thumb/" + document.IDN +"/" +GlobalVariables.getOsnovniPodaci().idkorisnika;
            string url = RequestUri + method;
            var response = await client.GetStringAsync(url);

            thumbList = (List<Thumb>)JsonConvert.DeserializeObject(response, typeof(List<Thumb>));
            finalThumbList = thumbList;
            BindingContext = thumbList;
        }

        public void DeleteClicked(object sender, EventArgs e)
        {
            var item = (ImageButton)sender;
            int id =Convert.ToInt32(item.CommandParameter);
            finalThumbList = finalThumbList.Where(c => c.IDN != id).ToList();
            BindingContext = finalThumbList;

        }

        public async Task<int> SendDocument(int vrsta_id) 
        {
            List<Korisnici> primaoci = new List<Korisnici>();
            int new_id = 0;
            try
            {
                List<int> korisniciSelectedIndexList = pickerKorisnici.SelectedIndices;
                primaoci = pickerKorisnici.ItemsSource.Where((c, index) => korisniciSelectedIndexList.Contains(index)).Select(x => new Korisnici { IDKorisnika = x.key }).ToList();

                ProsledjivanjeDok request = new ProsledjivanjeDok()
                {
                    GrupeZaSlanje = null,
                    Primaoci = primaoci,
                    IDN = 0,
                    ID = 0,
                    ID_VrsteDok = vrsta_id,
                    IDPosiljaoca = GlobalVariables.getOsnovniPodaci().idkorisnika,
                    Datum_s = string.Join(",", finalThumbList.Select(p => p.IDN.ToString())),
                    IDPrimaoca = 0,
                    IDGrupe = 0,
                    ID_Dokumenta = document.IDN,
                    IDStatusa = 0,
                    Naslov = naslov.Text,
                    Poruka = poruka.Text,
                    Tip = 1
                };

                var client = new HttpClient();
                string RequestUri = GlobalVariables.ServerAdresa;
                string metoda = "SlanjeDokumenata";

                string url = RequestUri + metoda;
                var jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    new_id = (int)JsonConvert.DeserializeObject(result, typeof(int)); //dobijam id novog dokumenta
                }
                else 
                {
                    await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
                }
                return new_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new_id;
            }
        }


        public async Task SendEmailToUser(int newDocId)
        {
            List<Korisnici> primaoci = new List<Korisnici>();
            try
            {
                List<int> korisniciSelectedIndexList = pickerKorisnici.SelectedIndices;
                primaoci = pickerKorisnici.ItemsSource.Where((c, index) => korisniciSelectedIndexList.Contains(index)).Select(x => new Korisnici { IDKorisnika = x.key }).ToList();

                int CheckBoxVariable = 0;

                if (zipCheckBox.IsChecked)
                {
                    CheckBoxVariable = 1;
                }
                else if (fileCheckBox.IsChecked)
                {
                    CheckBoxVariable = 2;
                }
                else if (!zipCheckBox.IsChecked && !fileCheckBox.IsChecked)
                {
                    CheckBoxVariable = 0;
                }


                SlanjeEmailaModel request = new SlanjeEmailaModel()
                {
                    WF = false,
                    file = CheckBoxVariable,
                    OriginLink = GlobalVariables.PrikazDokumenataAdresa,
                    statusWF = document.StatusWF,
                    vrstaDok = document.Vrsta,
                    Posiljaoc = "",
                    Primaoci = null,
                    listaIDNKor = pickerKorisnici.ItemsSource.Where((c, index) => korisniciSelectedIndexList.Contains(index)).Select(x => x.key).ToList(),
                    idnDok = newDocId,
                    Datum_s = "",
                    Procitana = false,
                    Tekst = poruka.Text,
                    Naslov = naslov.Text,
                    ID_Primaoca = 0,
                    ID_Posiljaoca = GlobalVariables.getOsnovniPodaci().idkorisnika,
                    IDN = 0,
                    ime = GlobalVariables.getOsnovniPodaci().Ime,
                    prezime = GlobalVariables.getOsnovniPodaci().Prezime
                };

                var client = new HttpClient();
                string RequestUri = GlobalVariables.ServerAdresa;
                string metoda = "SendEmailToUser";

                string url = RequestUri + metoda;
                var jsonData = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("", "Dokument uspešno prosleđen.", "OK");
                    await Navigation.PopModalAsync();
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

		private void fileCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
            if (fileCheckBox.IsChecked)
            {
                zipCheckBox.IsChecked = false;
            }
        }

		private void zipCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
            if (zipCheckBox.IsChecked)
            {
                fileCheckBox.IsChecked = false;
            }
        }

        private async Task<int> GetVrstaID()
        {
            int result = 0;
            try 
            {
                    var client = new HttpClient();
                    string RequestUri = GlobalVariables.ServerAdresa;
                    string method = "GetVrstaDok?vrstadok";
                    string url = RequestUri + method + "=" + document.Vrsta;
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var response_string = await response.Content.ReadAsStringAsync();

                        result = (int)JsonConvert.DeserializeObject(response_string, typeof(int)); //rezultat je id_vrste dokumenta 
                    }
                    else
                    {
                        await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
                       
                    }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }



    }
}