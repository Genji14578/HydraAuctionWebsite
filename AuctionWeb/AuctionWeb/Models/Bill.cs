//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuctionWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill
    {
        public int id { get; set; }
        public int id_account { get; set; }
        public int id_auction { get; set; }
        public double final_bid { get; set; }
        public string payment_method { get; set; }
        public Nullable<bool> isActive { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Auction Auction { get; set; }
    }
}
