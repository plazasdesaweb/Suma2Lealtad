using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class Orden
    {
        public int id { set; get; }
        public int companyid { set; get; }
        public string status { set; get; }
        public decimal totalamount { set; get; }
        public DateTime creationdate { set; get; }
    }
}