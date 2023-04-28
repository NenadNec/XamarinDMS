using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
    public class SlanjeDokumenataModel
    {
        public class ProsledjivanjeDok
        {
            public List<Grupe> GrupeZaSlanje { get; set; }
            public List<Korisnici> Primaoci { get; set; }
            public int IDN { get; set; }
            public int ID { get; set; }
            public int ID_VrsteDok { get; set; }
            public int IDPosiljaoca { get; set; }
            public int IDPrimaoca { get; set; }
            public string Datum_s { get; set; }
            public int IDGrupe { get; set; }
            public int ID_Dokumenta { get; set; }
            public int IDStatusa { get; set; }
            public string Naslov { get; set; }
            public string Poruka { get; set; }
            public int Tip { get; set; }
        }

        public class Grupe
        {
            public int IDGrupe { get; set; }
            public string Naziv { get; set; }
            public string Napomena { get; set; }
            public int BrojKorisnika { get; set; }
            public int IDUlogovanogKorisnika { get; set; }
            public string Racunar { get; set; }
            public string IPAdresa { get; set; }

        }

        public class Korisnici
        {
            public int IDKorisnika { get; set; }
            public int IDGrupe { get; set; }
            public string SrednjeIme { get; set; }
        }
    }
}
