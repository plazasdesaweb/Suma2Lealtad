﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class AfiliadoSaldosMovimientos
    {
        public string denominacionSuma { get; set; }
        public string denominacionPrepago { get; set; }
        public Afiliado Afiliado { get; set; }
        public SaldosMovimientos SaldosMovimientos { get; set; }
        
    }
}