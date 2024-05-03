using AuctionWeb.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Controllers
{
    public class BidController : Controller
    {
        // GET: Bid
        public ActionResult Index(int auc_id)
        {
            Bid_historyDao bid_HistoryDao = new Bid_historyDao();
            bid_HistoryDao.GetBid_HistoriesByAuction(auc_id);
            

            return View(bid_HistoryDao.GetBid_HistoriesByAuction(auc_id));
        }
    }
}