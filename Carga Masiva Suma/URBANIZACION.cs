//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Carga_Masiva_Suma
{
    using System;
    using System.Collections.Generic;
    
    public partial class URBANIZACION
    {
        public URBANIZACION()
        {
            this.PARROQUIAs = new HashSet<PARROQUIA>();
        }
    
        public string COD_URBANIZACION { get; set; }
        public string DESCRIPC_URBANIZACION { get; set; }
    
        public virtual ICollection<PARROQUIA> PARROQUIAs { get; set; }
    }
}