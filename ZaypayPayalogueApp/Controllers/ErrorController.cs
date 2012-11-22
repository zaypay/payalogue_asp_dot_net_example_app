using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace ZaypayPayalogueApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            System.Diagnostics.Debug.WriteLine("exception is   : " + Server.GetLastError());
            return View();
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }

    }
}
