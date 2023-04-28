using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
  public class PretragaOdeljenjeModels
    {
        public int IDD { get; set; }
        public int IDUlogovanogKorisnika { get; set; }
        public object IPAdresa { get; set; }
        public object NazivParentID { get; set; }
        public object NazivRukovodioca { get; set; }
        public string NazivSluzbe { get; set; }
        public object NazivZamenikaRukovodioca { get; set; }
        public int ParentID { get; set; }
        public object PodSluzbe { get; set; }
        public object Racunar { get; set; }
        public int Rukovodilac { get; set; }
        public string SifraSluzbe { get; set; }
        public string Telefon { get; set; }
        public int ZamenikRukovodioca { get; set; }
    }
}
