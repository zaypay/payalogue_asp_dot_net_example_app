using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ZaypayPayalogueApp.Models
{
    public class ZaypayDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }
}