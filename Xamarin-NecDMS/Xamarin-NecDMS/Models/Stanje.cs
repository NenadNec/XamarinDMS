using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
	public class Stanje
	{
		public int IDKorisnika2 { get; set; }
		public int IDKorisnika { get; set; }
		public int podfaza { get; set; }
		public int ParentID { get; set; }
		public int WF { get; set; }
		public int ZaduzenAltTrenutna { get; set; }
		public int ZaduzenTrenutna { get; set; }
		public int ZaduzenoLiceA { get; set; }
		public string TrajanjeSati { get; set; }
		public string ZaduzenAlt { get; set; }
		public string Zaduzen { get; set; }
		public int ZaduzenoLice { get; set; }
		public int Status { get; set; }
		public int KreatorID { get; set; }
		public string ImeIzlaznog { get; set; }
		public string ImeUlaznog { get; set; }
		public string RM2 { get; set; }
		public int ID_Stanja { get; set; }
		public string Naziv_Stanja { get; set; }
		public string Opis_Stanja { get; set; }
		public string Datum_Kreiranja { get; set; }
		public string Kreator { get; set; }
		public int ID_Akcije { get; set; }
		public List<KorisniciFaze> listaKorisnika { get; set; }
		public int ID_AlternativneAkcije { get; set; }
		public string UlazniObavezan { get; set; }
		public string IzlazniDokument { get; set; }
		public string IzlazniObavezan { get; set; }
		public string RM1 { get; set; }
		public string Naziv_A_Akcije { get; set; }
		public string Naziv_Akcije { get; set; }
		public string UlazniDokument { get; set; }
		public List<Stanje> listaStanja { get; set; }
	}
}
