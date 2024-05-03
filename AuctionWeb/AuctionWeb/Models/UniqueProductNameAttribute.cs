using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class UniqueProductNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Auction auction = DataProvider.Ins.DB.Auctions.Where(x => x.auction_name.Equals(value)).SingleOrDefault();
            return auction == null;
        }
    }
}