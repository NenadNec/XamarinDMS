using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
	public class KorisniciFaze
	{
		public int IDKorisnika { get; set; }
		public string ImePrezime { get; set; }
		public List<PartneriFaze> listaPartnera { get; set; }
	}
}
