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
    
    public partial class PrepaidBeneficiary
    {
        public int affiliateid { get; set; }
        public int beneficiaryid { get; set; }
        public Nullable<System.DateTime> begindate { get; set; }
        public Nullable<System.DateTime> enddate { get; set; }
        public string comments { get; set; }
        public Nullable<bool> active { get; set; }
    
        public virtual PrepaidCustomer PrepaidCustomer { get; set; }
    }
}
