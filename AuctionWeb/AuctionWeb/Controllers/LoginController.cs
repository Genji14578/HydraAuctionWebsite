using AuctionWeb.Models.Dao;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using System.Diagnostics;

namespace AuctionWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (ModelState.IsValid)
            {
                Account acc = DataProvider.Ins.DB.Accounts.Where(x => x.username == account.username && x.password == account.password).SingleOrDefault();
                if (acc != null)
                {
                    Session["Username"] = account.username;
                    Session["Role"] = account.id_role;
                    Session["Id"] = account.id;
                    FormsAuthentication.SetAuthCookie(account.username, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.User = new GenericPrincipal(new System.Security.Principal.GenericIdentity(string.Empty), null);
            Session["Username"] = "Welcome to my Web";
            Session["Id"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RegisterPage()
        {
            return View();
        }

        public ActionResult Register(string username, string password, string phone)
        {
            Session["Status"] = null;
            AccountDao accountDao = new AccountDao();
            Account account = new Account();
            account.username = username;
            account.password = password;
            account.phone = phone;
            account.id_role = 2;
            account.isActive = true;


            if (accountDao.Register(account))
            {
                Session["Status"] = "RegisterSuccessful";
            }
            else
            {
                Session["Status"] = "RegisterFail";
            }

            return Redirect("Index");
        }

        public ActionResult FogotPasswordPage()
        {
            return View();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private static Account account = new Account();
        private static string re_code = "";
        [HttpPost]
        public ActionResult ForgotPassword(string username)
        {
            AccountDao accountDao = new AccountDao();
            account = accountDao.FindAccountByUsername(username);

            if (account.email == null)
            {
                Session["ErrInvalid"] = "InvalidAccount";
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("hydragamingstore@gmail.com", "Hydra");
                        var receiverEmail = new MailAddress(account.email);
                        var password = "HydraGamingStore12345";
                        var sub = "Recover Password Code";
                        var body = re_code = RandomNumber(10000, 99999).ToString();
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = sub,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }
                        return View("CheckCode");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ViewBag.Error = "Some Error";
                }



            }
            return View("Index");
        }

        public ActionResult CheckCode(string code)
        {
            if (re_code == code)
            {
                return View("RecoverPassword");
            }
            else
            {
                return View("CheckCode");
            }
        }

        public ActionResult ChangePassword(string repassword, string password)
        {
            if (account != null)
            {
                if (repassword == password)
                {
                    AccountDao accountDao = new AccountDao();
                    accountDao.UpdatePassword(account.id, password);

                    return View("Index");
                }
                else
                {
                    return View("RecoverPassword");
                }
            }
            else
            {
                return View();
            }


        }
    }

}