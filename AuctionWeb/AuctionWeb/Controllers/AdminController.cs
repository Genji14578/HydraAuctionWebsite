using AuctionWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        // GET: Admin
        #region General
        public ActionResult Index()
        {
            ViewBag.num_auctions = DataProvider.Ins.DB.Auctions.ToList().Count;
            ViewBag.num_user = DataProvider.Ins.DB.Accounts.ToList().Count;
            ViewBag.num_category = DataProvider.Ins.DB.Categories.ToList().Count;
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.username.Equals(System.Web.HttpContext.Current.User.Identity.Name)).SingleOrDefault();
            Session["img"] = account.avatar;
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        #endregion General
        #region Account
        [HttpGet]
        public ActionResult ListAccount(int? page)
        {
            DataProvider.Ins.DB.Configuration.ProxyCreationEnabled = false;
            List<Account> accounts = DataProvider.Ins.DB.Accounts.Where(x => x.isActive.Equals(true)).ToList();
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            return View(accounts.ToPagedList(pageIndex, pageSize));
        }
        [HttpGet]
        public ActionResult GetAccount()
        {
            DataProvider.Ins.DB.Configuration.ProxyCreationEnabled = false;
            List<Account> accounts = DataProvider.Ins.DB.Accounts.Where(x => x.isActive.Equals(true)).ToList();
            return Json(new { data = accounts }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddAccount()
        {
            return View("AddAccount", new Account());
        }
        [HttpPost]
        public ActionResult AddAccount(Account account, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null)
            {
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Ticks + System.IO.Path.GetExtension(file.FileName);
                string saveFilePath = Server.MapPath("~/Content/images/") + newFileName;
                file.SaveAs(saveFilePath);
                account.avatar = newFileName;
                account.id_role = 2;
                DataProvider.Ins.DB.Accounts.Add(account);
                DataProvider.Ins.DB.SaveChanges();
                Session["msg"] = "Success";
                return RedirectToAction("Index");
            }
            else if (ModelState.IsValid && file == null)
            {
                account.id_role = 2;
                account.avatar = "default.png";
                DataProvider.Ins.DB.Accounts.Add(account);
                DataProvider.Ins.DB.SaveChanges();
                Session["msg"] = "Success";
                return RedirectToAction("Index");
            }
            else if (!ModelState.IsValid)
            {
                Session["msg"] = "Failed";
                return View();
            }
            else
            {
                Session["msg"] = "Failed";
                return View();
            }
        }
        [HttpGet]
        public ActionResult DeleteAccount(int id)
        {
            //Bid_history history = DataProvider.Ins.DB.Bid_history.Where(x => )
            Account acc = DataProvider.Ins.DB.Accounts.Where(x => x.id.Equals(id)).SingleOrDefault();
            if (acc == null)
            {
                return View("Error");
            }
            else
            {
                List<Bid_history> l_bid_history = DataProvider.Ins.DB.Bid_history.Where(x => x.id_account == id).ToList();
                List<Auction> l_auction = DataProvider.Ins.DB.Auctions.Where(x => x.id_account == id).ToList();
                if (l_bid_history != null)
                {
                    foreach (Bid_history item in l_bid_history)
                    {
                        DataProvider.Ins.DB.Bid_history.Remove(item);
                    }
                    DataProvider.Ins.DB.SaveChanges();
                }
                if (l_auction != null)
                {
                    foreach (Auction item in l_auction)
                    {
                        DataProvider.Ins.DB.Auctions.Remove(item);
                    }
                    DataProvider.Ins.DB.SaveChanges();
                }
                DataProvider.Ins.DB.Accounts.Remove(acc);
                DataProvider.Ins.DB.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult EditAccount(int id)
        {
            Account acc = DataProvider.Ins.DB.Accounts.Where(x => x.id.Equals(id)).SingleOrDefault();
            return View(acc);
        }
        [HttpGet]
        public ActionResult Info(int id)
        {
            Account acc = DataProvider.Ins.DB.Accounts.Where(x => x.id.Equals(id)).SingleOrDefault();

            return View(acc);
        }
        [HttpGet]
        public ActionResult BanAccount(int id)
        {
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.id.Equals(id)).SingleOrDefault();
            account.isActive = true;
            DataProvider.Ins.DB.SaveChanges();
            return RedirectToAction("BanList");
        }
        [HttpGet]
        public ActionResult UnBannedAccount(int id)
        {
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.id.Equals(id)).SingleOrDefault();
            account.isActive = true;
            DataProvider.Ins.DB.SaveChanges();
            return RedirectToAction("ListAccount");
        }
        [HttpGet]
        public ActionResult BanList(int ?page)
        {
            List<Account> accounts = DataProvider.Ins.DB.Accounts.Where(x => x.isActive.Equals(false)).ToList();
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            return View(accounts.ToPagedList(pageIndex,pageSize));
        }
        public JsonResult SearchAccount(string username)
        {
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.username.Equals(username)).SingleOrDefault();
            return Json(new { data = account }, JsonRequestBehavior.AllowGet);
        }
        #endregion Account
        #region Category
        [HttpGet]
        public ActionResult ListCategory(int ?page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<Category> categories = DataProvider.Ins.DB.Categories.ToList();
            return View(categories.ToPagedList(pageIndex,pageSize));
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View(new Category());
        }
        [HttpPost]
        public ActionResult AddCategory(Category category, HttpPostedFileBase file)
        {
            if (file == null)
            {
                return View();
            }
            if (file != null)
            {
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Ticks + System.IO.Path.GetExtension(file.FileName);
                string saveFilePath = Server.MapPath("~/Content/images/") + newFileName;
                file.SaveAs(saveFilePath);
                category.avatar = newFileName;
                DataProvider.Ins.DB.Categories.Add(category);
                DataProvider.Ins.DB.SaveChanges();
                Session["msg"] = "Success";
                return RedirectToAction("ListCategory");
            }
            return RedirectToAction("ListCategory");
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            Category category = DataProvider.Ins.DB.Categories.Where(x => x.id.Equals(id)).SingleOrDefault();
            return View(category);
        }
        [HttpPost]
        public ActionResult EditCategory(int id, string name, HttpPostedFileBase file)
        {
            string newFileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Ticks + System.IO.Path.GetExtension(file.FileName);
            string saveFilePath = Server.MapPath("~/Content/images/") + newFileName;
            file.SaveAs(saveFilePath);
            Category category = DataProvider.Ins.DB.Categories.Where(x => x.id.Equals(id)).SingleOrDefault();
            category.category_name = name;
            category.avatar = newFileName;
            Session["msg"] = "Success";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.id.Equals(id)).SingleOrDefault();
            DataProvider.Ins.DB.Accounts.Remove(account);
            DataProvider.Ins.DB.SaveChanges();
            Session["msg"] = "Success";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult MergeCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MergeCategory(string category1, string category2, string merge_category, HttpPostedFileBase mergeImage)
        {
            // Input: 2 categories
            // Ouput: 1 categories merge from 2 categories
            Category cate1 = DataProvider.Ins.DB.Categories.Where(x => x.category_name.Equals(category1)).SingleOrDefault();
            Category cate2 = DataProvider.Ins.DB.Categories.Where(x => x.category_name.Equals(category2)).SingleOrDefault();
            string newFileName = System.IO.Path.GetFileNameWithoutExtension(mergeImage.FileName) + DateTime.Now.Ticks + System.IO.Path.GetExtension(mergeImage.FileName);
            string saveFilePath = Server.MapPath("~/Content/images/") + newFileName;
            mergeImage.SaveAs(saveFilePath);
            Category merge_cate = new Category
            {
                category_name = merge_category,
                avatar = newFileName
            };
            DataProvider.Ins.DB.Categories.Add(merge_cate);
            DataProvider.Ins.DB.SaveChanges();
            List<Auction> auction1 = DataProvider.Ins.DB.Auctions.Where(x => x.id_category.Equals(cate1.id)).ToList();
            List<Auction> auction2 = DataProvider.Ins.DB.Auctions.Where(x => x.id_category.Equals(cate2.id)).ToList();
            Category new_cate = DataProvider.Ins.DB.Categories.Where(x => x.category_name.Equals(merge_category)).SingleOrDefault();
            try
            {
                foreach(var item in auction1)
                {
                    item.id_category = new_cate.id;
                }
                DataProvider.Ins.DB.SaveChanges();
                foreach(var item in auction2)
                {
                    item.id_category = new_cate.id;
                }
                DataProvider.Ins.DB.SaveChanges();
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return RedirectToAction("Error");
            }
            

            return RedirectToAction("ListCategory");
        }
        #endregion Category
        #region Auction
        [HttpGet]
        public ActionResult ListAuction(int ?page)
        {
            List<Auction> auctions = DataProvider.Ins.DB.Auctions.ToList();
            ViewBag.ListAccount = DataProvider.Ins.DB.Accounts.ToList();
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            return View(auctions.ToPagedList(pageIndex, pageSize));               
        }
        [HttpGet]
        public ActionResult DeleteAuction(int id)
        {
            Auction auction = DataProvider.Ins.DB.Auctions.Where(x => x.id.Equals(id)).SingleOrDefault();
            DataProvider.Ins.DB.Auctions.Remove(auction);
            DataProvider.Ins.DB.SaveChanges();
            Session["msg"] = "Success";
            return RedirectToAction("ShowAuction");
        }
        #endregion Auction
        #region Bill
        [HttpGet]
        public ActionResult ListBill(int ?page)
        {
            List<Bill> bills = DataProvider.Ins.DB.Bills.ToList();
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            return View(bills.ToPagedList(pageIndex, pageSize));
        }
        [HttpGet]
        public ActionResult EditBill(int id)
        {
            Bill bill = DataProvider.Ins.DB.Bills.Where(x => x.id == id).SingleOrDefault();
            List<string> lst = new List<string>();
            lst.Add("True");
            lst.Add("False");
            List<string> payment = new List<string>();
            payment.Add("Cash");
            payment.Add("Pay Pal");
            ViewBag.lst = lst;
            ViewBag.payment = payment;
            return View(bill);
        }
        [HttpPost]
        public ActionResult EditBill(int id, string method, string bit)
        {
            Bill bill = DataProvider.Ins.DB.Bills.Where(x => x.id == id).SingleOrDefault();
            if (bit == "True")
            {
                bill.isActive = true;
            }
            else
            {
                bill.isActive = false;
            }
            bill.payment_method = method;
            DataProvider.Ins.DB.SaveChanges();
            return RedirectToAction("ListBill");
        }
        #endregion
    }
}