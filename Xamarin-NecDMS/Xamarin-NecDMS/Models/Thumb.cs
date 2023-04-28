using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
     public class Thumb
    {
       
            public DateTime Datum_Izmene { get; set; }
            public string Display_Naziv { get; set; }
            public object Display_Putanja { get; set; }
            public int IDN { get; set; }
            public int ID_Dokumenta { get; set; }
            public string NazivDokumenta { get; set; }
            public string Stvarna_Putanja { get; set; }
            public string Thumb64 { get; set; }
            public string korisnik { get; set; }
       
    }
}
