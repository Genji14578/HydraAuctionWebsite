using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models.Dao
{
    public class Bid_historyDao
    {
        private DBAuctioningWebEntities db = new DBAuctioningWebEntities();

        public List<Bid_history> GetBid_HistoriesByAuction(int id_auction)
        {
            return db.Bid_history.Where(q => q.id_auction == id_auction).ToList();
        }

        public void CreateBid_history(Bid_history bid_history)
        {
            db.Bid_history.Add(bid_history);
            Auction auction=db.Auctions.Where(q => q.id == bid_history.id_auction).Single();
            auction.auction_bid_increment = bid_history.bid;


            db.SaveChanges();
        }

    }
}