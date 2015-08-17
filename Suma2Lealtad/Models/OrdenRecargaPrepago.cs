﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class OrdenRecargaPrepago
    {
        public ClientePrepago Cliente { get; set; }                               //Datos del Cliente 
        public int id { set; get; }
        public string statusOrden { set; get; }
        public decimal montoOrden { set; get; }
        public DateTime creationdateOrden { set; get; }
        public string tipoOrden { set; get; }                                   //Individual(Indicar Recargas de forma manual) ó Masiva(Indicar Recargas desde archivo)          
        //public List<DetalleOrdenRecargaPrepago> DetalleOrden { set; get; }        //Detalle de la orden
    }
}