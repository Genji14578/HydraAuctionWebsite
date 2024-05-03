using AuctionWeb.Models.Dao;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using PagedList;
using AuctionWeb.App_Start;
using AuctionWeb.Models.Logger;
using System.Diagnostics;

namespace AuctionWeb.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        public ActionResult Index(int? page)
        {
            BillDao billDao = new BillDao();

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            List<Bill> bills = billDao.GetBillsByAccount(int.Parse(Session["ID"].ToString()));


            return View(bills.ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public ActionResult CreateBill(int idBuyer, int idAuc, double finalBid)
        {
            BillDao billDao = new BillDao();
            Bill bill = new Bill();
            bill.id_account = idBuyer;
            bill.id_auction = idAuc;
            bill.final_bid = finalBid;
            bill.isActive = false;

            billDao.CreateBill(bill);

            AuctionDao auctionDao = new AuctionDao();
            auctionDao.UpdateStatus(idAuc);

            return RedirectToAction("Index", "Auction");
        }
        private Payment payment;
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, int bill_id)
        {
            BillDao billDao = new BillDao();

            Bill bill = billDao.FindBillById(bill_id);

            Item item = new Item();
            item.name = bill.Auction.auction_name;
            item.currency = "USD";
            item.price = bill.final_bid.ToString();
            item.quantity = "1";
            item.sku = "sku";

            ItemList itemList = new ItemList() { items = new List<Item>() };
            itemList.items.Add(item);

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrl = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl

            };

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = bill.final_bid.ToString(),




            };

            var amount = new Amount()
            {
                currency = "USD",
                total = details.subtotal,
                details = details,
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = bill.Auction.auction_description,
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrl

            };
            return payment.Create(apiContext);
        }
        public Payment ExcutePayment(APIContext apiContext, string payerId, string paymentId, string token)
        {

            apiContext = PayPalConfig.GetAPIContext();

            var paymentExcution = new PaymentExecution()
            {
                payer_id = payerId

            };

            payment = new Payment() { id = paymentId, token = token };

            return payment.Execute(apiContext, paymentExcution);
        }
        public ActionResult CheckOut(int bill_id)
        {


            APIContext apiContext = PayPalConfig.GetAPIContext();

            try
            {
                string token = Request.Params["token"];
                string payer_id = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payer_id))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Bill/ExcutePayment?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, bill_id);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;

                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }

                    }

                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);



                }
                else
                {
                    var guid = Request.Params["guid"];
                    var excutePayment = ExcutePayment(apiContext, payer_id, Session["guid"] as string, token);
                    if (excutePayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                    else {
                        return RedirectToAction("Index");
                    }

                }

            }
            catch (Exception ex)
            {
                PayPalLogger.Log("Error:" + ex.Message);
                Debug.WriteLine(ex.Message);
                return View("Failure");
                throw;
            }

            return View();




        }
    }
}