using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class ClientePrepago
    {
        public int idCliente { get; set; }
        public string nameCliente { get; set; }
        public string aliasCliente { get; set; }
        public string rifCliente { get; set; }
        public string addressCliente { get; set; }
        public string phoneCliente { get; set; }
        public string emailCliente { get; set; }

        public List<AfiliadoSuma> beneficiarios;
        public List<OrdenRecargaPrepago> ordenes;
    }
}