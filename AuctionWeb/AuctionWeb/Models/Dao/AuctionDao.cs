using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models.Dao
{
    public class AuctionDao
    {
        private DBAuctioningWebEntities db = new DBAuctioningWebEntities();

        public List<Auction> GetAuctions()
        {
            var now = DateTime.Now;

            List<Auction> auctions = db.Auctions.ToList();

            List<Auction> auctionsInTime = new List<Auction>();

            foreach (Auction auction in auctions)
            {
                if (auction.auction_start_date <= now && auction.auction_end_date >= now)
                {
                    auctionsInTime.Add(auction);
                }
            }

            return auctionsInTime;
        }

        public List<Auction> GetAuctionsByCategoryId(int id)
        {
            var now = DateTime.Now;
            return db.Auctions.Where(q => q.id_category == id&&q.auction_start_date <= now && q.auction_end_date >= now).ToList();
        }

        public List<Auction> GetAuctionsByCategoryName(string category_name)
        {
            var now = DateTime.Now;
            return db.Auctions.Where(q => q.Category.category_name.Equals(category_name) && q.auction_start_date <= now && q.auction_end_date >= now).ToList();
        }

        public List<Auction> GetAuctionsIsCompletedInAccount(int id_buyer_account)
        {
            return db.Auctions.Where(q => q.auction_isCompleted == true & q.id_buyer_account == id_buyer_account).ToList();
        }

        public List<Auction> MyAuction(int id_account)
        {
            return db.Auctions.Where(q => q.auction_isCompleted == false & q.id_account == id_account).ToList();
        }

        public Auction FindAuctionById(int id)
        {
            return db.Auctions.Find(id);
        }

        public void UpdateAuction(string filename, string photo, int id_auction)
        {
            Auction auction = FindAuctionById(id_auction);
            auction.auction_file = filename;
            auction.auction_image = photo;
            db.SaveChanges();
        }

        public void UpdateStatus(int id_auction)
        {
            Auction auction = FindAuctionById(id_auction);
            auction.auction_isCompleted = true;
            db.SaveChanges();
        }

        public void UpdateIdBuyer(int id_buyer,int id_auction)
        {
            Auction auction = FindAuctionById(id_auction);
            auction.id_buyer_account = id_buyer;
            db.SaveChanges();
        }

        public void CreateAuction(Auction auction)
        {
            db.Auctions.Add(auction);
            db.SaveChanges();
        }

        public void DeleteAuction(int id_auction)
        {
            Auction auction = FindAuctionById(id_auction);
            db.Auctions.Remove(auction);
            db.SaveChanges();
        }
    }
}