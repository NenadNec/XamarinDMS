
using System;
using System.Collections.Generic;
using System.Text;
using static NecDMS.Models.SlanjeDokumenataModel;

namespace NecDMS.Models
{
    class SlanjeEmailaModel
    {
        public bool WF { get; set; }
        public int file { get; set; }
        public string OriginLink { get; set; }
        public int statusWF { get; set; }
        public string vrstaDok { get; set; }
        public string Posiljaoc { get; set; }
        public List<Korisnici1> Primaoci { get; set; }
        public List<int> listaIDNKor { get; set; }
        public int idnDok { get; set; }
        public string Datum_s { get; set; }
        public bool Procitana { get; set; }
        public string Tekst { get; set; }
        public string Naslov { get; set; }
        public int ID_Primaoca { get; set; }
        public int ID_Posiljaoca { get; set; }
        public int IDN { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
    }
}
