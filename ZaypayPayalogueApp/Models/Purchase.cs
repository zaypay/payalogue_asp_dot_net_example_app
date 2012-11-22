using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZaypayPayalogueApp.Models
{
    public class Purchase
    {
        public int ID { get; set; }
        public int ZaypayPaymentId { get; set; }
        public string Status { get; set; }

        public virtual Product Product { get; set; }

        public Purchase() { }

        public Purchase(int paymentId, Product product)
        {
            Product = product;
            Status = "prepared";
            ZaypayPaymentId = paymentId;

        }

        public void Update(string status)
        {            
            Status = status;                            
        }


    }
}