using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
  public  class MessagesModels
    {

        public DateTime Datum { get; set; }
        public object Datum_s { get; set; }
        public int IDN { get; set; }
        public int ID_Posiljaoca { get; set; }
        public int ID_Primaoca { get; set; }
        public string Naslov { get; set; }
        public string Posiljaoc { get; set; }
        public object Primaoci { get; set; }
        public bool Procitana { get; set; }
        public string Tekst { get; set; }
        public bool WF { get; set; }
        public int idnDok { get; set; }
        public string imgSrc { get; set; }
        public string shortText { get; set; }

        public int BellCount { get; set; }

    }
}
