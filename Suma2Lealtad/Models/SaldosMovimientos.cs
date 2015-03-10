﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class SaldosMovimientos
    {
        public string DocId { set; get; }        
        public IEnumerable<Saldo> Saldos { set; get; }
        public IEnumerable<Movimiento> MovimientosSuma { set; get; }
        public IEnumerable<Movimiento> MovimientosPrepago { set; get; }
    }
}