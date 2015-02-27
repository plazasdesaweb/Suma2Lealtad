using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class AfiliadoWeb
    {
        [Key, Column(Order = 1)]
        //public string id { get; set; }
        //public string email { get; set; }
        //public int type { get; set; }
        //public string docnumber { get; set; }
        //public string name { get; set; }
        //public string phone1 { get; set; }
        //public string phone2 { get; set; }
        //public string clave { get; set; }

        public int FechaSolicitud { get; set; }
        public int FechaAfiliacion { get; set; }
        public string IdSucursal { get; set; }
        public string IdOcupacion { get; set; }
        public string IdNacionalidad { get; set; }

        public int? questionid { get; set; }

        public string answer { get; set; }

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

    }
}