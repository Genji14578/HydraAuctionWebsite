using AuctionWeb.Models.Dao;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Validation;
using System.Diagnostics;
using AuctionWeb.Models.EFModel;
using Newtonsoft.Json;

namespace AuctionWeb.Controllers
{
    public class AuctionController : Controller
    {
        // GET: Auction
        public ActionResult Index(int? page)
        {
            AuctionDao auctionDao = new AuctionDao();

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            List<Auction> auctions = auctionDao.GetAuctions();


            return View(auctions.ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult getAuctionDetail()
        {
            AuctionDao auctionDao = new AuctionDao();

            List<Auction> auctions = new List<Auction>();

            auctions = auctionDao.GetAuctions();

            List<AuctionModel> auctionModels = new List<AuctionModel>();

            foreach (Auction auction in auctions)
            {
                auctionModels.Add(new AuctionModel(auction.id, auction.id_category, auction.id_buyer_account, auction.auction_name, auction.auction_bid_increment, auction.auction_start_date, auction.auction_end_date, auction.auction_isCompleted));
            }



            string json = JsonConvert.SerializeObject(auctionModels, Formatting.Indented
                    , new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,

                    });

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Bidding(int id, double bid)
        {
            Bid_historyDao bid_HistoryDao = new Bid_historyDao();

            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.username == System.Web.HttpContext.Current.User.Identity.Name && x.isActive == true).SingleOrDefault();
            Bid_history bid_History = new Bid_history();
            bid_History.id_account = account.id;
            bid_History.id_auction = id;
            bid_History.bid = bid;
            bid_History.bidding_time = DateTime.Now;

            AuctionDao auctionDao = new AuctionDao();
            Auction auction = auctionDao.FindAuctionById(id);

            auctionDao.UpdateIdBuyer(account.id, id);

            bid_HistoryDao.CreateBid_history(bid_History);

            return RedirectToAction("Index");
        }
        public ActionResult ShowAuctionInCategory(int? page, int id)
        {
            AuctionDao auctionDao = new AuctionDao();

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            List<Auction> auctions = auctionDao.GetAuctionsByCategoryId(id);


            return View("Index", auctions.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult IsCompletedAuctionInAccount(int? page)
        {
            AuctionDao auctionDao = new AuctionDao();

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            List<Auction> auctions = auctionDao.GetAuctionsIsCompletedInAccount(int.Parse(Session["Id"].ToString()));

            return View("Index", auctions.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detail(int id)
        {
            AuctionDao auctionDao = new AuctionDao();
            return View("Detail", auctionDao.FindAuctionById(id));
        }
        [HttpGet]
        public ActionResult Sell()
        {
            Session["lst_Category"] = DataProvider.Ins.DB.Categories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Sell(string product_name, string category, HttpPostedFileBase product_image, string product_description, double gap_bid, DateTime start_date, DateTime end_date, double min_price)
        {
            
            if (start_date == end_date)
            {
                Session["msg"] = "Overtime";
                return RedirectToAction("Sell");
            }
            else if (end_date.Subtract(start_date).TotalHours > 24)
            {
                Session["msg"] = "Overtime";
                return RedirectToAction("Sell");
            }
            else
            {
                Category cate = DataProvider.Ins.DB.Categories.Where(x => x.category_name == category).SingleOrDefault();
                Account account = DataProvider.Ins.DB.Accounts.Where(x => x.username.Equals(System.Web.HttpContext.Current.User.Identity.Name) && x.isActive == true).SingleOrDefault();
                if (account == null)
                {
                    Session["msg"] = "Banned";
                    return RedirectToAction("Sell");
                }
                else if (product_image != null)
                {
                    var img = System.Drawing.Image.FromStream(product_image.InputStream, true, true);
                    int width = img.Width;
                    int height = img.Height;
                    if (width == 500 && height == 500)
                    {
                        string newFileName = System.IO.Path.GetFileNameWithoutExtension(product_image.FileName) + DateTime.Now.Ticks + System.IO.Path.GetExtension(product_image.FileName);
                        string saveFilePath = Server.MapPath("~/Content/images/") + newFileName;
                        product_image.SaveAs(saveFilePath);
                        Auction auction = new Auction
                        {
                            auction_name = product_name,
                            id_category = cate.id,
                            auction_description = product_description,
                            auction_gap_bid = gap_bid,
                            auction_start_date = start_date,
                            auction_end_date = end_date,
                            id_account = account.id,
                            auction_image = newFileName,
                            auction_isCompleted = false,
                            auction_bid_increment = min_price,
                            auction_minimum_bid = min_price
                        };
                        DataProvider.Ins.DB.Auctions.Add(auction);
                        Debug.WriteLine("This code goes to add");
                        DataProvider.Ins.DB.SaveChanges();
                        Session["msg"] = "Success";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Session["msg"] = "Oversized";
                        return View();
                    }
                }
                else if (product_image == null)
                {
                    return View();

                }
            }

            return View();
        }

        public ActionResult MyAuction(int? page)
        {
            AuctionDao auctionDao = new AuctionDao();

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            List<Auction> auctions = auctionDao.MyAuction(int.Parse(Session["Id"].ToString()));

            return View("Index", auctions.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Search(string keyword, int? page)
        {

            AuctionDao auctionDao = new AuctionDao();

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            List<Auction> auctions = auctionDao.GetAuctionsByCategoryName(keyword);


            return View("Index", auctions.ToPagedList(pageNumber, pageSize));
        }


    }
}