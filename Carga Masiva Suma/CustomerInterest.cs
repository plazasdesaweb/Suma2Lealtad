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
    
    public partial class CustomerInterest
    {
        public int customerid { get; set; }
        public int interestid { get; set; }
        public string comments { get; set; }
    
        public virtual Interest Interest { get; set; }
    }
}
