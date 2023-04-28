using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NecDMS.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using NecDMS.View;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace NecDMS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PretragaPoSadrzaju : ContentPage
    {
        public PretragaPoSadrzaju()
        {
            InitializeComponent();  
            PojamIcn.IsVisible = false;
        }


        #region Akcija pretraga
        private async void PoSadrzajuPretrazi_Clicked(object sender, EventArgs e)
        {

            try { 
                PretragaDokumentaRequestModels request = new PretragaDokumentaRequestModels
                {

                    referent = 0,
                    sluzba = 0,
                    naziv = "",
                    delovodni = "",
                    vrsta = 0,
                    pojam = Pojam.Text == null ? "" : Pojam.Text,
                    type = "sadrzaj",
                    datumDo = "",
                    datumOd = "",
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
                    arhviranaDok = ""

            };

                await Navigation.PushAsync(new PretragaDokumentaView(request,null),false);

            }

            catch (Exception ex)
            {
                await DisplayAlert("Greška", "Pokušajte ponovo", "OK");
            }
        }

        #endregion


        #region IconDeleteTapped
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (Pojam.Text != " ")
            {
                Pojam.Text = string.Empty;
                PojamIcn.IsVisible = false;
            }
        }

        #endregion IconDeleteTapped

        #region IconHide
        private void Pojam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Pojam.Text == string.Empty)
            {
                PojamIcn.IsVisible = false;
            }

            else
            {
                PojamIcn.IsVisible = true;
            };
        }

        #endregion IconHide
    }
}