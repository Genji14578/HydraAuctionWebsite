using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models.Dao
{
    public class BillDao
    {
        private DBAuctioningWebEntities db = new DBAuctioningWebEntities();

        public List<Bill> GetBillsByAccount(int id_account)
        {
            return db.Bills.Where(q => q.id_account == id_account).ToList();
        }

        public void CreateBill(Bill bill)
        {
            db.Bills.Add(bill);
            db.SaveChanges();
        }
        public Bill GetBillByProduct(int id_auction)
        {
            return db.Bills.Where(q => q.id_auction == id_auction).SingleOrDefault();
        }

        public Bill FindBillById(int id)
        {
            return db.Bills.Find(id);
        }

        public Bill FindBillByIdAuction(int id_auction)
        {
            return db.Bills.Where(q=>q.id_auction==id_auction).SingleOrDefault();
        }

        public void UpdatePaymentBill(string payment_method, int id_bill)
        {
            Bill bill = FindBillById(id_bill);
            bill.payment_method = payment_method;
            db.SaveChanges();
        }

        public void DeleteBill(int id_auction)
        {
            Bill bill = FindBillByIdAuction(id_auction);
            db.Bills.Remove(bill);
            db.SaveChanges();
        }


    }
}