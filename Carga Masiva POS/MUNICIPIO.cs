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
    
    public partial class MUNICIPIO
    {
        public MUNICIPIO()
        {
            this.CIUDADs = new HashSet<CIUDAD>();
            this.PARROQUIAs = new HashSet<PARROQUIA>();
        }
    
        public string COD_MUNICIPIO { get; set; }
        public string DESCRIPC_MUNICIPIO { get; set; }
    
        public virtual ICollection<CIUDAD> CIUDADs { get; set; }
        public virtual ICollection<PARROQUIA> PARROQUIAs { get; set; }
    }
}