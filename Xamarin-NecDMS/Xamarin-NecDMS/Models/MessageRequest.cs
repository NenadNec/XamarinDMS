using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
    public class MessageRequest
    {
        public int BrojZapisa { get; set; }
        public int TrenutnaStrana { get; set; }
        public string IDDir { get; set; }
        public int IDKor { get; set; }
        public string IDGrupe { get; set; }
        public int ParentID { get; set; }
        public string term { get; set; }
        public string type { get; set; }
    }
}
