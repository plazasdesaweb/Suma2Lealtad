//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Carga_Masiva_POS
{
    using System;
    using System.Collections.Generic;
    
    public partial class RESTRICCION
    {
        public int COD_RESTRICCION { get; set; }
        public Nullable<System.DateTime> FECHA_ACT_REST { get; set; }
        public Nullable<System.DateTime> FECHA_DESAC_REST { get; set; }
        public Nullable<byte> MINTRAN_ACR { get; set; }
        public Nullable<byte> MAXTRAN_ACR { get; set; }
        public Nullable<decimal> MINMONTO_ACR { get; set; }
        public Nullable<decimal> MAXMONTO_ACR { get; set; }
        public Nullable<byte> MINTRAN_RED { get; set; }
        public Nullable<byte> MAXTRAN_RED { get; set; }
        public Nullable<decimal> MINMONTO_RED { get; set; }
        public Nullable<decimal> MAXMONTO_RED { get; set; }
    }
}
