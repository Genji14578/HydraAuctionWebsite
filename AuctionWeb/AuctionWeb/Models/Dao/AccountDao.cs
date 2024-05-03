using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models.Dao
{
    public class AccountDao
    {
        private DBAuctioningWebEntities db = new DBAuctioningWebEntities();

        public Account Login(string username, string password)
        {
            return db.Accounts.Where(q => q.username == username & q.password == password).SingleOrDefault();
        }
        public List<Account> All()
        {
            return db.Accounts.ToList();
        }

        public bool Register(Account account)
        {
            if (FindAccountByUsername(account.username) == null)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Account FindAccountById(int id)
        {
            return db.Accounts.Find(id);
        }

        public Account FindAccountByUsername(string username)
        {
            return db.Accounts.Where(q => q.username == username).Single();
        }



        public void UpdateAccount(int id_account, string username, string password, string phone, string email, string paypal_account, string avatar)
        {
            Account account = FindAccountById(id_account);
            account.username = username;
            account.password = password;
            account.phone = phone;
            account.email = email;
            account.paypal_account = paypal_account;
            account.avatar = avatar;
            account.avatar = avatar;

            db.SaveChanges();
        }

        public void UpdatePassword(int id_account, string password)
        {
            
                Account account = FindAccountById(id_account);
                account.password = password;

                db.SaveChanges();
            

        }



    }
}