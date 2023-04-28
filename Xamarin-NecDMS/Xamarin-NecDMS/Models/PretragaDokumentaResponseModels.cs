using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
public class PretragaDokumentaResponseModels
    {

        public string Broj { get; set; }
        public string BrojFajlova { get; set; }
        public int BrojPovezanih { get; set; }
        public string Datum { get; set; }
        public string DatumIzmene { get; set; }
        public int DokumentiReferenti { get; set; }
        public string Godina { get; set; }
        public int IDDir { get; set; }
        public int IDN { get; set; }
        public int Iznos { get; set; }
        public string Korisnik { get; set; }
        public string Naziv { get; set; }
        public string Opcioni1 { get; set; }
        public string Opcioni2 { get; set; }
        public string Opcioni3 { get; set; }
        public int OpcioniIznos { get; set; }
        public string Partner { get; set; }
        public List<PretragaDokumentaResponseModels> PodDokumenti { get; set; }
        public string Predmet { get; set; }
        public string Redni { get; set; }
        public string Referent { get; set; }
        public string Sluzba { get; set; }
        public string Smer { get; set; }
        public int StatusWF { get; set; }
        public string Vrsta { get; set; }
        public object iznos { get; set; }
    }
}
