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
    
    public partial class PARAMETRO
    {
        public int ID_PARAM { get; set; }
        public string NOMBRE_PARAM { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<int> MTO_CANJE { get; set; }
        public Nullable<int> PTS_CANJE { get; set; }
        public Nullable<int> MTO_ACR { get; set; }
        public Nullable<int> PTS_ACR { get; set; }
        public Nullable<int> TIPO_PRE { get; set; }
        public Nullable<int> TIPO_SUMA { get; set; }
        public Nullable<System.DateTime> FECHA_ACT_PARAM { get; set; }
        public Nullable<System.DateTime> FECHA_DESAC_PARAM { get; set; }
        public Nullable<System.DateTime> FECHA { get; set; }
    }
}
