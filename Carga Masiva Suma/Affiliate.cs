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
    
    public partial class Affiliate
    {
        public int id { get; set; }
        public int customerid { get; set; }
        public string docnumber { get; set; }
        public int clientid { get; set; }
        public int storeid { get; set; }
        public int channelid { get; set; }
        public int typeid { get; set; }
        public System.DateTime affiliatedate { get; set; }
        public string typedelivery { get; set; }
        public Nullable<int> storeiddelivery { get; set; }
        public System.DateTime estimateddatedelivery { get; set; }
        public System.DateTime creationdate { get; set; }
        public int creationuserid { get; set; }
        public int modifieduserid { get; set; }
        public System.DateTime modifieddate { get; set; }
        public Nullable<int> reasonsid { get; set; }
        public string twitter_account { get; set; }
        public string facebook_account { get; set; }
        public string instagram_account { get; set; }
        public string comments { get; set; }
        public Nullable<int> sumastatusid { get; set; }
    
        public virtual Channel Channel { get; set; }
        public virtual SumaStatus SumaStatu { get; set; }
    }
}
