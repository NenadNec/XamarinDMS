using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
    public class LoginModels
    {

        public class KorisnikPrava
        {
            public int AutoPregled { get; set; }
            public int Brisanje { get; set; }
            public int Izmena { get; set; }
            public string Naziv { get; set; }
            public int NoviZapis { get; set; }
            public int Stampa { get; set; }

        }

        public class KorisnikOsnovniPodaci
        {
            public int IDGrupe { get; set; }
            public int IDSluzbe { get; set; }
            public string Ime { get; set; }
            public int ParentID { get; set; }
            public string Prezime { get; set; }
            public int idkorisnika { get; set; }
            public string FirmName { get; set; }
        }

    }
}
