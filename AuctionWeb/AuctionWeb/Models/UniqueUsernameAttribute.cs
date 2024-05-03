using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Account account = DataProvider.Ins.DB.Accounts.Where(x => x.username.Equals(value.ToString())).SingleOrDefault();
            return account == null;
        }
    }
}