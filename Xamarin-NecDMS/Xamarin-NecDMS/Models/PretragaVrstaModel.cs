using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
    public class PretragaVrstaModel
    {
            public int IDN { get; set; }
            public int ID_PodKat { get; set; }
            public int ID_Sluzbe { get; set; }
            public string Naziv { get; set; }
            public string NazivSluzbe { get; set; }
            public int TokDok { get; set; }
            public object binIkona { get; set; }
        
    }
}
