using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models.Dao
{
    public class CategoryDao
    {
        DBAuctioningWebEntities db = new DBAuctioningWebEntities();

        public List<Category> GetCategories() {
            return db.Categories.ToList();
        }

    }
}