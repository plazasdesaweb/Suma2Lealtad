﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suma2Lealtad.Models
{
    public class ParametrosReporte
    {
        public string TipoConsulta { get; set; }
        public string ModoTransaccion { get; set; }
        public string fechadesde { get; set; }
        public string fechahasta { get; set; }
        public int idCliente { get; set; }
        public string numdoc { get; set; }
        public string referencia { get; set; }
        public string estatusTarjeta { get; set; }
        public decimal numeroTarjeta { get; set; }
    }
}
