using NecDMS.Models;
using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net;
using Acr.UserDialogs;
using Xamarin.Forms.PlatformConfiguration;
using static NecDMS.Models.SlanjeDokumenataModel;

namespace NecDMS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PretragaDatumIsteka : ContentPage
    {
        int vrstaID = 0;
        public PretragaDatumIsteka()
        {
            InitializeComponent();
            VrstaIcn.IsVisible = false;
            

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetFirstTime();
        }

        #region AutoComplete DatumIsteka

        private async void Vrsta_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            try
            {
                IconHideVrsta();
                AutoSuggestBox input = (AutoSuggestBox)sender;
                if (input.Text.Length >= 2)
                {
                    var client = new HttpClient();
                    string RequestUri = GlobalVariables.ServerAdresa;
                    string method = "GetVrsteDokumenataAutoCombo/";
                    string type = "/istek";
                    string url = RequestUri + method + input.Text + type;
                    var response = await client.GetStringAsync(url);
                    List<PretragaVrstaModel> ListItems = (List<PretragaVrstaModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(List<PretragaVrstaModel>));
                    input.ItemsSource = ListItems;

                }

                if (input.Text.Length < 2)
                {
                    input.ItemsSource = null;
                    VrstaIcn.IsVisible = false;
                }
            }

            catch(Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }

        }

        private void Vrsta_SuggestionChosen_1(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            PretragaVrstaModel obj = e.SelectedItem as PretragaVrstaModel;
            vrstaID = obj.IDN;
        }


        #endregion AutoComplete DatumIsteka


        #region Akcija pretraga 

        private async void DatumIstekaPretrazi_Clicked(object sender, EventArgs e)
        {

            try
            {
                PretragaDokumentaRequestModels request = new PretragaDokumentaRequestModels()
                {
                    referent = 0,
                    sluzba = 0,
                    naziv = "",
                    delovodni = "",
                    vrsta = vrstaID,
                    pojam = "",
                    type = "istek",
                    datumDo = endDateTime.Date.ToString(@"yyyy-MM-dd"),
                    datumOd = startDateTime.Date.ToString(@"yyyy-MM-dd"),
                    term = "",
                    partner = "",
                    ParentID = "",
                    upit3 = "",
                    upit2 = "",
                    upit1 = "",
                    IDVrste = "",
                    IDDir = "",
                    IDKor = GlobalVariables.getOsnovniPodaci().idkorisnika,
                    IDGrupe = "",
                    Filteri = new object {},
                    TrenutnaStrana = 0,
                    BrojZapisa = 25,
                    upitPDF = "",
                    arhviranaDok = "",
                };

                await Navigation.PushAsync(new PretragaDokumentaView(request,null),false);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }

        }
        #endregion


        #region Datum

        private async void GetFirstTime()
        {
            try
            {
                var client = new HttpClient();
                string RequestURi = GlobalVariables.ServerAdresa;
                string date = "GetFirstDate";
                string uri = RequestURi + date;
                var response = await client.GetStringAsync(uri);

                if (response != "")
                {
                    string datumOdString = Regex.Unescape(response).Replace('"', ' ').Trim().Replace('.', '/');
                    DateTime datumOd = DateTime.ParseExact(datumOdString, "dd/MM/yyyy", null);

                    startDateTime.Date = datumOd;
                }

                else
                {
                    await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
                }
            }
            catch(Exception ex)         
            { 
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }
        }


        #endregion


        #region IconDeleteTapped
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (Vrsta.Text != " ")
            {
                Vrsta.Text = string.Empty;
                VrstaIcn.IsVisible = false;
                vrstaID = 0;
            }
        }

        #endregion IconDeleteTapped


        #region IconHideVrsta
        public void IconHideVrsta()
        {
            if (Vrsta.Text == " ")
            {
                VrstaIcn.IsVisible = false;
            }

            else
            {
                VrstaIcn.IsVisible = true;
            };


        }

        #endregion IconHideVrsta

    }
}