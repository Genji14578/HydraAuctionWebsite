using AuctionWeb.Models;
using AuctionWeb.Models.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AccountDao accountDao = new AccountDao();
                Account f_account = accountDao.FindAccountByUsername(System.Web.HttpContext.Current.User.Identity.Name);
                return View(f_account);
            }

        }
        public ActionResult All(int id)
        {
            AccountDao accountDao = new AccountDao();
            accountDao.All();
            ViewBag.id = id;
            return View("All", accountDao.All());

        }
        [HttpPost]
        public ActionResult Update(System.Web.HttpPostedFileWrapper avatar, string username, string password, string phone, string email, string paypal)
        {
            AccountDao accountDao = new AccountDao();
            if (avatar != null && avatar.ContentLength > 0)
            {
                var img = System.Drawing.Image.FromStream(avatar.InputStream, true, true);
                int width = img.Width;
                int height = img.Height;
                if (width == 500 && height == 500)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/images"), Path.GetFileName(avatar.FileName));
                    avatar.SaveAs(path);
                    accountDao.UpdateAccount(int.Parse(Session["Id"].ToString()), username, password, phone, email, paypal, avatar.FileName);
                    return Redirect("Index");
                }
                else
                {
                    Session["msg"] = "Oversized";
                    return Redirect("Index");
                }
                

            }
            else
            {
                Account account = accountDao.FindAccountById(int.Parse(Session["Id"].ToString()));
                accountDao.UpdateAccount(int.Parse(Session["Id"].ToString()), username, password, phone, email, paypal, account.avatar);

                return Redirect("Index");
            }

        }

        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["Login"] = null;

            return RedirectToAction("Index", "Home");
        }




    }
}
