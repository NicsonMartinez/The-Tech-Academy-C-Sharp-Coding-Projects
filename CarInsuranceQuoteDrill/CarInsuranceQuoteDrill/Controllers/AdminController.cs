using CarInsuranceQuoteDrill.Models; //NOTE: This is needed for 'CarInsuranceQuoteDrillEntities' databse objects.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsuranceQuoteDrill.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (CarInsuranceQuoteDrillEntities db = new CarInsuranceQuoteDrillEntities())
            {
                var userQuotes = db.UserQuotes;
            }
            return View();
        }
    }
}