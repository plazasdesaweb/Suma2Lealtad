using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class AfiliadoSuma
    {

        public string id { get; set; }
        public string type { get; set; }
        public string docnumber { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string gender { get; set; }
        public string maritalstatus { get; set; }
        public string birthdate { get; set; }
        public string nationality { get; set; }
        public string occupation { get; set; }
        public string phone3 { get; set; }
        public List<Interest> Intereses { get; set; }

        public string exnumber { get; set; }
        public string exdetail { get; set; }

        public string comments { get; set; }

    }

}