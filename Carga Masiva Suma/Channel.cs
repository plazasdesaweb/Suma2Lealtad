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
    
    public partial class Channel
    {
        public Channel()
        {
            this.Affiliates = new HashSet<Affiliate>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<Affiliate> Affiliates { get; set; }
    }
}
