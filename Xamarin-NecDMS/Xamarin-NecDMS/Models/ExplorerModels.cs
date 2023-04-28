using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
	public class ExplorerModels
	{
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string NavURL { get; set; }
        public int ParentMenuID { get; set; }
        public string Pravo { get; set; }
        public int Tip { get; set; }
    }
}
