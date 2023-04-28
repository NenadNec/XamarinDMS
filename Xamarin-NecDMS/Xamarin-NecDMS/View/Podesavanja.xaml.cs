using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NecDMS;
using Xamarin.Essentials;
using NecDMS.Models;

namespace NecDMS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Podesavanja : ContentPage
    {
        public Podesavanja()
        {
            InitializeComponent();

            ServerApiUrl.Text = Preferences.Get("ApiUrl" , string.Empty);
            DocumentPreviewUrl.Text = Preferences.Get("PreviewUrl", string.Empty);
            if (ServerApiUrl.Text == string.Empty) 
            {
                ServerApiUrl.Text = "http://";
            }

            if (DocumentPreviewUrl.Text == string.Empty)
            {
                DocumentPreviewUrl.Text = "http://";
            }



        }

        private  void Button_Save_Server(object sender, EventArgs e)
        {
            Preferences.Set("ApiUrl", ServerApiUrl.Text);
            GlobalVariables.ServerAdresa = Preferences.Get("ApiUrl", string.Empty);
           
        }

		private async void ServerApiUrlТapped(object sender, EventArgs e)
		{
            var result = await DisplayPromptAsync("Server Url", "", "Potvrdi", "Otkaži", null, -1, null, ServerApiUrl.Text);
            if (result != null)
            {
                ServerApiUrl.Text = result;
                Preferences.Set("ApiUrl", ServerApiUrl.Text);
                GlobalVariables.ServerAdresa = Preferences.Get("ApiUrl", string.Empty);
            }


        }

        private async void DocumentPreviewUrlТapped(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("Klijent Url", "", "Potvrdi", "Otkaži", null, -1, null, DocumentPreviewUrl.Text);
            if (result != null)
            {
                DocumentPreviewUrl.Text = result;
                Preferences.Set("PreviewUrl", DocumentPreviewUrl.Text);
                GlobalVariables.PrikazDokumenataAdresa = Preferences.Get("PreviewUrl", string.Empty);
            }


        }
    }
}