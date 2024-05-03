using AuctionWeb.Models.Dao;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace AuctionWeb.Controllers
{
    public class HomeController : Controller
    {
        CategoryDao categoryDao = new CategoryDao();

        public ActionResult Index(int? page)
        {
            if (Session["Login"] == null)
            {
                Session["Login"] = "Login";
            }
            else
            {
                Session["Login"] = null;
            }

            /*if (Session["Username"] == null)
            {
                Session["Username"] = "Welcome to my Web";
            } */
            if (User.Identity.IsAuthenticated)
            {
                Session["Username"] = System.Web.HttpContext.Current.User.Identity.Name;
                AccountDao accountDao = new AccountDao();
                Account account = accountDao.FindAccountByUsername(Session["Username"].ToString());
                Session["Role"] = account.Role.role_name;
                Session["Id"] = account.id;
            }
            else
            {
                Session["Username"] = "Welcome to my Web";
            }

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);


            return View(loadData().ToPagedList(pageNumber, pageSize));
        }

        public List<Category> loadData()
        {
            List<Category> categories = categoryDao.GetCategories();
            return categories;
        }

        public ActionResult test() {
            return View();
        }


    }
}