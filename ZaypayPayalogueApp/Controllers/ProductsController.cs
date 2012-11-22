using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZaypayPayalogueApp.Models;
using System.Collections.Specialized;
using Zaypay.WebService;
using Zaypay;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ZaypayPayalogueApp.Controllers
{
    public class ProductsController : Controller
    {

        private ZaypayDBContext db = new ZaypayDBContext();
        LogEntry logEntry = new LogEntry();

        public ViewResult Index()
        {            
            return View(db.Products.ToList());
        }

        public ActionResult Details(int Id = 0)
        {
            Product product = db.Products.Find(Id);
            return View(product);
        }

        public ActionResult Reporting()
        {

            NameValueCollection parameters = Request.QueryString;
            
            int priceSettingId = 0;
            int paymentId = 0;
            int productId = 0;
            int payalogueId = 0;

            Int32.TryParse(parameters["price_setting_id"], out priceSettingId);
            Int32.TryParse(parameters["payment_id"], out paymentId);
            Int32.TryParse(parameters["product_id"], out productId);
            Int32.TryParse(parameters["payalogue_id"], out payalogueId);


            System.Diagnostics.Debug.WriteLine("params are " + parameters);

            if (AllValuesPresent(ref parameters))
            {
                Product product = db.Products.Find(productId);
                System.Diagnostics.Debug.WriteLine("all val present");

                if (product != null && product.PriceSettingId == priceSettingId && product.PayalogueId == payalogueId)
                {

                    System.Diagnostics.Debug.WriteLine("prod is god, and ps id is good an payaid is good");

                    try
                    {
                        PriceSetting ps = new PriceSetting(product.PriceSettingId);
                        PaymentResponse payment = ps.ShowPayment(paymentId);

                        string status = payment.Status();

                        if (status == parameters["status"])
                        {
                                                        
                            Purchase purchase = db.Purchases.FirstOrDefault(x => x.ZaypayPaymentId == paymentId);
                            
                            if (purchase == null)
                            {                            
                                purchase = new Purchase(paymentId, product);
                                db.Purchases.Add(purchase);
                            }
                            else                            
                                purchase.Update(status);
                            
                            db.SaveChanges();
                            
                        }

                    }
                    catch (Exception ex)
                    {
                        LogEntry(ex.Message);   
                        System.Diagnostics.Debug.WriteLine("Some exception ocured");
                    }

                }                
            }
            
            return Content("*ok*");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool AllValuesPresent(ref NameValueCollection collection)
        {

            if (
                    (collection["status"] != null) &&
                    (collection["price_setting_id"] != null) &&
                    (collection["payment_id"] != null) &&
                    (collection["product_id"] != null) &&
                    (collection["payalogue_id"] != null)
                )
                return true;
            else
                return false;

        }

        private void LogEntry(string mesg)
        {
            logEntry.Message = mesg;
            Logger.Write(logEntry);
        }

    }
}
