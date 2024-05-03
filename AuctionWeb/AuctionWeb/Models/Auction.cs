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
    
    public partial class Auction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auction()
        {
            this.Bid_history = new HashSet<Bid_history>();
            this.Bills = new HashSet<Bill>();
        }
    
        public int id { get; set; }
        public int id_category { get; set; }
        public int id_account { get; set; }
        public string auction_name { get; set; }
        public string auction_description { get; set; }
        public string auction_image { get; set; }
        public string auction_file { get; set; }
        public double auction_minimum_bid { get; set; }
        public Nullable<double> auction_gap_bid { get; set; }
        public Nullable<double> auction_bid_increment { get; set; }
        public System.DateTime auction_start_date { get; set; }
        public System.DateTime auction_end_date { get; set; }
        public Nullable<bool> auction_isCompleted { get; set; }
        public Nullable<int> id_buyer_account { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bid_history> Bid_history { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
