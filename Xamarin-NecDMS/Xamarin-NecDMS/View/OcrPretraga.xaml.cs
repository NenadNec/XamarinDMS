using NecDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NecDMS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OcrPretraga : ContentPage
    {
        public OcrPretraga()
        {
            InitializeComponent();
            OcrIcn.IsVisible = false;
        }

        private async void OCRPretraga_Clicked(object sender, EventArgs e)
        {
            try
            {
                PretragaDokumentaRequestModels request = new PretragaDokumentaRequestModels
                {

                    referent = 0,
                    sluzba = 0,
                    naziv = "",
                    delovodni = "",
                    vrsta = 0,
                    pojam = OCR.Text == null ? "" : OCR.Text,  // a ovde kupis iz tvog entry kako ti se zove
                    type = "ocr",  //ovo je bitno da stoji ocr 
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
                    Filteri = new object { },
                    TrenutnaStrana = 0,
                    BrojZapisa = 25,
                    upitPDF = "",
                    arhviranaDok = ""

                };

                await Navigation.PushAsync(new PretragaDokumentaView(request, null), false);

            }

            catch (Exception ex)
            {
                await DisplayAlert("Greška", "Pokušajte ponovo", "OK");
            }

        }
        private void OCR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OCR.Text == string.Empty)
            {
                OcrIcn.IsVisible = false;
            }

            else
            {
                OcrIcn.IsVisible = true;
            };
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (OCR.Text != " ")
            {
                OCR.Text = string.Empty;
                OcrIcn.IsVisible = false;
            }
        }
    }
}