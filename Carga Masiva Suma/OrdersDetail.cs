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
    
    public partial class OrdersDetail
    {
        public int id { get; set; }
        public int orderid { get; set; }
        public int customerid { get; set; }
        public decimal amount { get; set; }
        public string comments { get; set; }
        public string cardsresponse { get; set; }
        public Nullable<int> sumastatusid { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual SumaStatus SumaStatu { get; set; }
    }
}