using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class DetalleOrdenRecargaPrepago
    {
        //public BeneficiarioPrepago beneficiario { get; set; }   //Beneficiario 
        public decimal montoRecarga { get; set; }               //Monto de Recarga de Saldo
        public string resultadoRecarga { get; set; }            //Resultado de la Operación de Recarga de Saldo en Cards
        public bool beneficiarioExcluido {get;set;}             //Indica si un beneficiario fue excluido de la Orden de Recarga en su revisión
        public string observacionesExclusion { get; set; }      //Información relativa a la exclusión (motivos)
        public string statusDetalleOrden { get; set; }          //Estatus del detalle de la orden
    }
}