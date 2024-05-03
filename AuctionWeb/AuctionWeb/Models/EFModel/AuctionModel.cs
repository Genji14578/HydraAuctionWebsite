using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models.EFModel
{
    public class AuctionModel
    {

        public AuctionModel(int id,int id_category,int? id_buyer_account, string auction_name,double? auction_bid_increment, DateTime auction_start_date, DateTime auction_end_date,bool? auction_isCompleted)
        {
            this.id = id;
            this.id_category = id_category;
            this.id_buyer_account = id_buyer_account;
            this.auction_name = auction_name;
            this.auction_bid_increment = auction_bid_increment;
            this.auction_start_date = auction_start_date;
            this.auction_end_date = auction_end_date;
            this.auction_isCompleted = auction_isCompleted;
        }

        public int id { get; set; }
        
        public int id_category { get; set; }
        
        public int id_account { get; set; }
       
        public int? id_buyer_account { get; set; }
        
        public string auction_name { get; set; }
       
        public double? auction_bid_increment { get; set; }
        
        public DateTime auction_start_date { get; set; }
       
        public DateTime auction_end_date { get; set; }
        
        public bool? auction_isCompleted { get; set; }
    }
}