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
    
    public partial class Interest
    {
        public Interest()
        {
            this.CustomerInterests = new HashSet<CustomerInterest>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public bool Checked { get; set; }
        public virtual ICollection<CustomerInterest> CustomerInterests { get; set; }
    }
}
