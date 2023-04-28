using NecDMS.Models;
using NecDMS.View;
using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Acr.UserDialogs;

namespace NecDMS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PretragaOsnovna : ContentPage
    {
        int odeljenjeID = 0;
        int referentID = 0;
        int vrstaID = 0;
        string SifraParnera = "";
       
        public PretragaOsnovna()
        {
            
            InitializeComponent();

            


            VrstaIcn.IsVisible = false;
            BrojIcn.IsVisible = false;
            NazivIcn.IsVisible = false;
            PartnerIcn.IsVisible = false;
            ReferentIcn.IsVisible = false;
            OdeljenjeIcn.IsVisible = false;   

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

         
            GetFirstDate();
           

        }


        #region Icon Hide
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

            return;
        }

        private void Broj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Broj.Text == string.Empty)
            {
                BrojIcn.IsVisible = false;
            }

            else
            {
                BrojIcn.IsVisible = true;
            };


        }


        private void Naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Naziv.Text == string.Empty)
            {
                NazivIcn.IsVisible = false;
            }

            else
            {
                NazivIcn.IsVisible = true;
            };


        }

        public void IconHidePartner()
        {
            if (Partner.Text == " ")
            {
                PartnerIcn.IsVisible = false;
            }

            else
            {
                PartnerIcn.IsVisible = true;
            };

            
        }

        public void IconHidenOdeljenje()
        {
            if (Odeljenje.Text == " ")
            {
                OdeljenjeIcn.IsVisible = false;
            }

            else
            {
                OdeljenjeIcn.IsVisible = true;
            };

            
        }

        public void IconHidenReferent()
        {
            if (Referent.Text == " ")
            {
                ReferentIcn.IsVisible = false;
            }

            else
            {
                ReferentIcn.IsVisible = true;
            };

      
        }

        #endregion Icon Hide

        #region AutoCompleteVrsta
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
                    string type = "/default";
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
            catch (Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }

        }
        private void Vrsta_Focused(object sender, FocusEventArgs e)
        {
            AutoSuggestBox input = (AutoSuggestBox)sender;
            input.ItemsSource = null;


        }

        private void Vrsta_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            PretragaVrstaModel obj = e.SelectedItem as PretragaVrstaModel;
            vrstaID = obj.IDN;

            
            
        }

        #endregion AutoCompleteVrsta

        #region AutoCompletePartner
        private async void Partner_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {


            try
            {
                IconHidePartner();
                AutoSuggestBox input = (AutoSuggestBox)sender;
                if (input.Text.Length >= 2)
                {

                    List<PretragaPartnerModels> NazivLista = new List<PretragaPartnerModels>();
                    var client = new HttpClient();
                    string RequestUri = GlobalVariables.ServerAdresa;
                    string method = "GetPartneriUpit?";
                    string partner = input.Text;
                    string url = RequestUri + method + "term=" + partner;
                    var response = await client.GetStringAsync(url);
                    List<PretragaPartnerModels> ListItems = (List<PretragaPartnerModels>)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(List<PretragaPartnerModels>));
                    string NazivPartnera = Partner.Text;

                    var ListaNaziv = ListItems.FirstOrDefault(s => s.NAZIV == NazivPartnera);
                    NazivLista.Add(ListaNaziv);
                    input.ItemsSource = ListItems;




                }

                if(input.Text.Length <2)
                {
                    input.ItemsSource = null;
                    PartnerIcn.IsVisible = false;
                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }


        }

        private void Partner_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            PretragaPartnerModels obj = e.SelectedItem as PretragaPartnerModels;
            SifraParnera = obj.SIFRA;

        }

        private void Partner_Focused(object sender, FocusEventArgs e)
        {
            AutoSuggestBox input = (AutoSuggestBox)sender;
            input.ItemsSource = null;
            
        }


        #endregion AutoCompletePartner

        #region AutoCompleteOdeljenje
        private async void Odeljenje_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            try
            {
                IconHidenOdeljenje();
                AutoSuggestBox input = (AutoSuggestBox)sender;

                if (input.Text.Length >= 2)
                {
                    List<PretragaOdeljenjeModels> OdeljenjeLista = new List<PretragaOdeljenjeModels>();
                    var client2 = new HttpClient();
                    string RequestUri = GlobalVariables.ServerAdresa;
                    string methodOdeljenje = "GetSluzbeUpit?";
                    string odeljenje = Odeljenje.Text;
                    string url = RequestUri + methodOdeljenje + "upit=" + odeljenje;
                    var response = await client2.GetStringAsync(url);
                    List<PretragaOdeljenjeModels> ListItemsOdeljenje = (List<PretragaOdeljenjeModels>)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(List<PretragaOdeljenjeModels>));


                    string NazivOdeljenja = Odeljenje.Text;

                    var ListaOdeljenje = ListItemsOdeljenje.FirstOrDefault(s => s.NazivSluzbe == NazivOdeljenja);
                    OdeljenjeLista.Add(ListaOdeljenje);
                    input.ItemsSource = ListItemsOdeljenje;

                }

                if (input.Text.Length < 2)
                {
                    input.ItemsSource = null;
                    OdeljenjeIcn.IsVisible = false;
                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }
        }


        private void Odeljenje_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            PretragaOdeljenjeModels obj = e.SelectedItem as PretragaOdeljenjeModels;
            odeljenjeID = obj.IDD;
        }

        private void Odeljenje_Focused(object sender, FocusEventArgs e)
        {
            AutoSuggestBox input = (AutoSuggestBox)sender;
            input.ItemsSource = null;
        }

        #endregion AutoCompleteOdeljenje

        #region AutoCompleteReferent
        private async void Referent_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {

            try
            {
                IconHidenReferent();
                AutoSuggestBox input = (AutoSuggestBox)sender;
                if (input.Text.Length >= 2)
                {

                    List<PretragaReferentModels> ReferentLista = new List<PretragaReferentModels>();
                    var client = new HttpClient();
                    string RequestUri = GlobalVariables.ServerAdresa;
                    string method = "GetReferentiUpit?";
                    string referent = input.Text;
                    string url = RequestUri + method + "term=" + referent;
                    var response = await client.GetStringAsync(url);
                    //LoginModels test = (LoginModels)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(LoginModels));
                    List<PretragaReferentModels> ListItems = (List<PretragaReferentModels>)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(List<PretragaReferentModels>));
                    string NazivReferenta = Referent.Text;

                    var ListaNaziv = ListItems.FirstOrDefault(s => s.ReferentNaziv == NazivReferenta);
                    ReferentLista.Add(ListaNaziv);
                    input.ItemsSource = ListItems;

                }

                if (input.Text.Length < 2)
                {
                    input.ItemsSource = null;
                    ReferentIcn.IsVisible = false;
                        
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }
        }

        private void Referent_SuggestionChosen_1(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {

            PretragaReferentModels obj = e.SelectedItem as PretragaReferentModels;
            referentID = obj.ReferentId;
        }

        private void Referent_Focused(object sender, FocusEventArgs e)
        {
            AutoSuggestBox input = (AutoSuggestBox)sender;
            input.ItemsSource = null;
        }


        #endregion AutoCompleteReferent

        #region DatumOD

        private async void GetFirstDate()
        {
            try
            {
                
                var client = new HttpClient();
                string RequestUri = GlobalVariables.ServerAdresa;
                string date = "GetFirstDate";
                string url = RequestUri + date;
                var response = await client.GetStringAsync(url);


                if (response != "")
                {
                    string datumOdString = Regex.Unescape(response).Replace('"', ' ').Trim().Replace('.', '/');
                    DateTime datumOd = DateTime.ParseExact(datumOdString, "dd/MM/yyyy", null);


                    startDatePicker.Date = datumOd;

                }
                else
                {
                    await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }
        }

        #endregion DatumOD

        #region Akcija pretraga
        private async void OsnovaPretrazi_Clicked(object sender, EventArgs e)
        {
            try {
               
                PretragaDokumentaRequestModels request = new PretragaDokumentaRequestModels()
                {

                    referent = referentID,
                    sluzba = odeljenjeID,
                    naziv = Naziv.Text == null ? "" : Naziv.Text,
                    delovodni = Broj.Text == null ? "" : Broj.Text,
                    vrsta = vrstaID,
                    pojam = "",
                    type = "osnovna",
                    datumDo = endDatePicker.Date.ToString(@"yyyy-MM-dd"),
                    datumOd = startDatePicker.Date.ToString(@"yyyy-MM-dd"),
                    term = "",
                    partner = SifraParnera,
                    ParentID = "",
                    upit3 = "",
                    upit2 = "",
                    upit1 = "",
                    IDVrste ="" ,
                    IDDir = "",
                    IDKor = GlobalVariables.getOsnovniPodaci().idkorisnika,
                    IDGrupe = "",
                    Filteri = new object {},
                    TrenutnaStrana = 0,
                    BrojZapisa = 25,
                    upitPDF = "",
                    arhviranaDok = arhivirano.IsChecked == true ? "1" : "0",
                };
                await Navigation.PushAsync(new PretragaDokumentaView(request,null),false);

            }


            catch (Exception ex)
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

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (Broj.Text != " ")
            {
                Broj.Text = string.Empty;
                BrojIcn.IsVisible  = false;
            }

        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            if (Naziv.Text != " ")
            {
                Naziv.Text = string.Empty;
                NazivIcn.IsVisible = false;
                
            }
           

           
        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            if (Partner.Text != " ")
            {
                Partner.Text = string.Empty;
                PartnerIcn.IsVisible = false;
                SifraParnera = "";
                
                
                
            }
        }

        private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            if (Odeljenje.Text != " ")
            {
                Odeljenje.Text = string.Empty;
                OdeljenjeIcn.IsVisible = false;
                odeljenjeID = 0;                
            }

        }

        private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {
            if (Referent.Text != " ")
            {
                Referent.Text = string.Empty;
                ReferentIcn.IsVisible = false;
                referentID = 0;
            }
        }





        #endregion IconDeleteTapped

        

        
    }
}
