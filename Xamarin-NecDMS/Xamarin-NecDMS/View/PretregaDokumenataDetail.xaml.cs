using NecDMS.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace NecDMS.View
{


	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class PretregaDokumenataDetail : ContentPage

	{
		PretragaDokumentaResponseModels document = new PretragaDokumentaResponseModels();
		public PretregaDokumenataDetail(List<Thumb> listaTambova, PretragaDokumentaResponseModels tapedDocument)
		{
			InitializeComponent();
			BindingContext = listaTambova;
			document = tapedDocument;


		}


		public async void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
		{

			try
			{
				Thumb previousThumb = e.PreviousItem as Thumb;
				Thumb currentThumb = e.CurrentItem as Thumb;

				string baseUrl = GlobalVariables.PrikazDokumenataAdresa + "/ImageViewMobil";
				if (baseUrl == string.Empty || baseUrl.Length < 10)
				{
					await DisplayAlert("Unesite ispravnu url adresu za prikaz dokumenata!", "Pokušajte ponovo", "OK");
					return;
				}
				string idfile = currentThumb.IDN.ToString();
				string statuswf = document.StatusWF.ToString();
				string webviewUrl = baseUrl + "?idfile=" + idfile + "&statuswf=" + statuswf;
				webview.Source = webviewUrl;
				webview.On<Android>().EnableZoomControls(true);
				webview.On<Android>().DisplayZoomControls(false);
				
			}
            catch (Exception ex)
            {
				await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
			}
		}


		
	}
}