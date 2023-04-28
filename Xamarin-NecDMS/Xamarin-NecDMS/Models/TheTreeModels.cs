using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
   public class TheTreeModels
    {

        public int referent { get; set; }
        public int sluzba { get; set; }
        public string naziv { get; set; }
        public string delovodni { get; set; }
        public int vrsta { get; set; }
        public string pojam { get; set; }
        public string type { get; set; }
        public string datumDo { get; set; }
        public string datumOd { get; set; }
        public string term { get; set; }
        public string partner { get; set; }
        public string ParentID { get; set; }
        public string upit3 { get; set; }
        public string upit2 { get; set; }
        public string upit1 { get; set; }
        public string IDVrste { get; set; }
        public string IDDir { get; set; }
        public string IDKor { get; set; }
        public string IDGrupe { get; set; }
        public List<Parametri> Filteri { get; set; }
        public string TrenutnaStrana { get; set; }
        public string BrojZapisa { get; set; }
        public string upitPDF { get; set; }
        public string arhviranaDok { get; set; }

    }
}
