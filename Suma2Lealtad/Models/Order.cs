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
    
    public partial class Order
    {
        public Order()
        {
            this.OrdersDetails = new HashSet<OrdersDetail>();
        }
    
        public int id { get; set; }
        public int prepaidcustomerid { get; set; }
        public decimal totalamount { get; set; }
        public string paymenttype { get; set; }
        public System.DateTime creationdate { get; set; }
        public int creationuserid { get; set; }
        public System.DateTime processdate { get; set; }
        public string comments { get; set; }
        public int sumastatusid { get; set; }
    
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
        public virtual PrepaidCustomer PrepaidCustomer { get; set; }
        public virtual SumaStatus SumaStatu { get; set; }
    }
}
