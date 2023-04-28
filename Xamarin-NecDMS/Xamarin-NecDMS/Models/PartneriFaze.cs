using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
	public class PartneriFaze
	{
		public int Sifra { get; set; }
		public string Naziv { get; set; }
		public List<PartneriFaze> listaPartnera { get; set; }
	}
}
