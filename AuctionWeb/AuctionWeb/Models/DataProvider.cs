using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get { if (_ins == null) _ins = new DataProvider(); return _ins; }
            set { _ins = value; }
        }
        public DBAuctioningWebEntities DB { get; set; }
        private DataProvider()
        {
            DB = new DBAuctioningWebEntities();
        }
    }
}