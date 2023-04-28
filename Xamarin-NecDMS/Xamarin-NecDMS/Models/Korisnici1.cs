using System;
using System.Collections.Generic;
using System.Text;
using static NecDMS.Models.SlanjeDokumenataModel;

namespace NecDMS.Models
{
	public class Korisnici1
	{
        public string Racunar { get; set; }
        public int IDUlogovanogKorisnika { get; set; }
        public List<Grupe> GrupeZaDodavanje { get; set; }
        public string NazivSluzbe { get; set; }
        public int IDGrupe { get; set; }
        public int id_odeljenja { get; set; }
        public string Napomena { get; set; }
        public string WWW { get; set; }
        public string IPAdresa { get; set; }
        public string eMail { get; set; }
        public string Adresa { get; set; }
        public string Lozinka { get; set; }
        public string OLDKorisnickoIme { get; set; }
        public string KorisnickoIme { get; set; }
        public string Prezime { get; set; }
        public string SrednjeIme { get; set; }
        public string Ime { get; set; }
        public int IDKorisnika { get; set; }
        public string Mesto { get; set; }
        public int Obrisano { get; set; }
    }
}
