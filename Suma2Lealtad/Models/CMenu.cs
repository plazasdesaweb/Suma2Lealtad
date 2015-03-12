using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Suma2Lealtad.Models
{

    public class CMenu
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentid { get; set; }
        public string controller { get; set; }
        public string actions { get; set; }
        public int order { get; set; }
    }
}