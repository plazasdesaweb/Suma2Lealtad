//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Suma2Lealtad.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TARJETA
    {
        public decimal NRO_TARJETA { get; set; }
        public int NRO_AFILIACION { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        public string ESTATUS_TARJETA { get; set; }
        public Nullable<decimal> SALDO_PUNTOS { get; set; }
        public string OBSERVACIONES { get; set; }
        public Nullable<int> COD_USUARIO { get; set; }
        public string TRACK1 { get; set; }
        public string TRACK2 { get; set; }
        public string CVV2 { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
    }
}
