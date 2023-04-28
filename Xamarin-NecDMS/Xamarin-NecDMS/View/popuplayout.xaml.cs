using NecDMS.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;

namespace NecDMS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class popuplayout : Rg.Plugins.Popup.Pages.PopupPage
	{



		private ObservableCollection<MessagesModels> listItems;

		public popuplayout(MessagesModels message)
		{
			InitializeComponent();

			lblTekst.Text = message.Tekst;
			title.Text = message.Naslov.Trim() + " (" + message.Posiljaoc.Trim() + ")";



		}

		public popuplayout(ObservableCollection<MessagesModels> listItems)
		{
			this.listItems = listItems;
		}



		private async void On_Popup_Close(object sender, EventArgs e)
		{

			await PopupNavigation.Instance.PopAsync(false);

		}
	}
}