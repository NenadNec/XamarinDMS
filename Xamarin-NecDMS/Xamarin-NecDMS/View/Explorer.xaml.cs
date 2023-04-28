using Acr.UserDialogs;
using NecDMS.Models;
using NecDMS.TreeView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NecDMS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Explorer : ContentPage
    {
		public Explorer()
		{
			InitializeComponent();
            LoadExplorerDataAsync();

        }


        protected override void OnAppearing()
        {

            base.OnAppearing();
            try
            {
                if (TheTreeView.SelectedItem != null) 
                {
                    Console.WriteLine(TheTreeView);
                }
            }
            catch (Exception ex)
            {
            }
        }



        public async void LoadExplorerDataAsync() 
        {
            try
            {
                var client = new HttpClient();
                string RequestUri = GlobalVariables.ServerAdresa;
                string method = "SiteMenuListKorisnik/";
                string url = RequestUri + method + GlobalVariables.getOsnovniPodaci().idkorisnika;
                var response = await client.GetStringAsync(url);
                List<ExplorerModels> result = (List<ExplorerModels>)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(List<ExplorerModels>));
                List<ExplorerGroupModels> recursiveExplorer = ExplorerRecursive(result, 0);

                var rootNodes = ProcessExplorerGroup(recursiveExplorer);

                TheTreeView.RootNodes = rootNodes;
            }

            catch (Exception ex)
            {
                await DisplayAlert("Greška!", "Pokušajte ponovo", "OK");
            }
        }


        public List<ExplorerGroupModels> ExplorerRecursive(List<ExplorerModels> flatItems, int? parentId = 0)
        {
            List<ExplorerGroupModels> list = new List<ExplorerGroupModels>();

            return flatItems.Where(x => x.ParentMenuID.Equals(parentId)).Select(item => new ExplorerGroupModels
            {

             MenuID = item.MenuID,
             MenuName = item.MenuName,
             NavURL = item.NavURL,
             ParentMenuID = item.ParentMenuID,
             Pravo = item.Pravo,
             Tip = item.Tip,
             Children = ExplorerRecursive(flatItems, item.MenuID).OrderBy(x => x.MenuName).ToList()}).OrderBy(x => x.MenuID).ToList();
        }


        //set what icons shows for expanded/Collapsed/Leafe Nodes or on request node expand icon (when ShowExpandButtonIfEmpty true).
        public class ExpandButtonContent : ContentView
        {

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();

                var node = (BindingContext as TreeViewNode);
                bool isLeafNode = (node.Children == null || node.Children.Count == 0);

                //empty nodes have no icon to expand unless showExpandButtonIfEmpty is et to true which will show the expand
                //icon can click and populated node on demand propably using the expand event.
                if ((isLeafNode) && !node.ShowExpandButtonIfEmpty)
                {
                    Content = new ResourceImage
                    {
                        Resource = isLeafNode ? "" : "NecDMS.Resource.FolderOpen.png",
                        HeightRequest = 30,
                        WidthRequest = 30
                    };
                }
                else
                {
                    Content = new ResourceImage
                    {
                        Resource = node.IsExpanded ? "NecDMS.Resource.OpenGlyph.png" : "NecDMS.Resource.CollpsedGlyph.png",
                        HeightRequest = 30,
                        WidthRequest = 30
                    };
                }
            }

        }

           private static ObservableCollection<TreeViewNode> ProcessExplorerGroup(List<ExplorerGroupModels> explorerGroupList)
           {
               var rootNodes = new ObservableCollection<TreeViewNode>();

               foreach (var explorerGroup in explorerGroupList)
               {

                   var label = new Label
                   {
                       VerticalOptions = LayoutOptions.Center,
                       TextColor = Color.Black
                   };
                   label.SetBinding(Label.TextProperty, "MenuName");

                   var groupTreeViewNode = CreateTreeViewExplorerNode(explorerGroup, label, false);

                   rootNodes.Add(groupTreeViewNode);

                   groupTreeViewNode.Children = ProcessExplorerGroup(explorerGroup.Children);

            }

               return rootNodes;
           }


        private static TreeViewNode CreateTreeViewExplorerNode(ExplorerGroupModels bindingContext, Label label, bool isItem)
        {
            var node = new TreeViewNode
            {
                
                BindingContext = bindingContext,
                Content = new StackLayout
                {
                    Children =
                        {
                            new ResourceImage
                            {
                                Resource = isItem? "NecDMS.Resource.Item.png" :"NecDMS.Resource.FolderClosed.png" ,
                                HeightRequest= 30,
                                WidthRequest = 30
                            },
                            label
                        },
                    Orientation = StackOrientation.Horizontal
                }
            };

            //set DataTemplate for expand button content
            node.ExpandButtonTemplate = new DataTemplate(() => new ExpandButtonContent { BindingContext = node });

            return node;
        }


        [Obsolete]
        private async void TheTreeView_SelectedItemChanged(object sender, EventArgs e)
        {
            var selectedItem = TheTreeView.SelectedItem?.BindingContext as ExplorerGroupModels;

            if (selectedItem.Tip == -2 && (selectedItem.MenuName.Contains("Poruke") || selectedItem.MenuName.Contains("Inbox") || selectedItem.MenuName.Contains("Outbox")))
            {
                try
                {

                    MessageRequest mRequest = new MessageRequest()
                    {

                        BrojZapisa = 25,
                        TrenutnaStrana = 0,
                        IDDir = selectedItem.NavURL,
                        IDKor = GlobalVariables.getOsnovniPodaci().idkorisnika,
                        IDGrupe = GlobalVariables.getOsnovniPodaci().IDGrupe.ToString(),
                        ParentID = GlobalVariables.getOsnovniPodaci().ParentID,
                        term = "null",
                        type = "explorer"

                    };
                    await Navigation.PushAsync(new Inbox(null,mRequest), false);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Greška", "Pokušajte ponovo", "OK");
                }

            }
            else
            {
                try
                {

                    PretragaDokumentaRequestModels request = new PretragaDokumentaRequestModels()
                    {
                        referent = 0,
                        sluzba = 0,
                        naziv = "",
                        delovodni = "",
                        vrsta = 0,
                        pojam = "",
                        type = "",
                        datumDo = "",
                        datumOd = "",
                        term = "",
                        partner = "",
                        ParentID = "",
                        upit3 = "",
                        upit2 = "",
                        upit1 = "",
                        IDVrste = "",
                        IDDir = selectedItem.NavURL,
                        IDKor = GlobalVariables.getOsnovniPodaci().idkorisnika,
                        IDGrupe = GlobalVariables.getOsnovniPodaci().IDGrupe.ToString(),
                        Filteri = new object {},
                        TrenutnaStrana = 0,
                        BrojZapisa = 25,
                        upitPDF = "",
                        arhviranaDok = "0",

                    };

                    await Navigation.PushAsync(new PretragaDokumentaView(request,null),false);
                    
                }

                catch (Exception ex)
                {
                    await DisplayAlert("Greška", "Pokušajte ponovo", "OK");
                }
            }
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
	}
}