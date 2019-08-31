using CarInsuranceQuoteDrill.Models; //NOTE: This is needed for 'CarInsuranceQuoteDrillEntities' databse objects.
using CarInsuranceQuoteDrill.ViewModel; //NOTE: this is to have access to 'userQuoteVm'
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
            var userQuoteVms = new List<UserQuoteVm>();

            using (CarInsuranceQuoteDrillEntities db = new CarInsuranceQuoteDrillEntities())
            {
                var userQuotes = (from x in db.UserQuotes select x).ToList();

                foreach(var userQuote in userQuotes)
                {
                    var userQuoteVm = new UserQuoteVm();
                    userQuoteVm.Id = userQuote.Id;
                    userQuoteVm.FirstName = userQuote.FirstName;
                    userQuoteVm.LastName = userQuote.LastName;
                    userQuoteVm.EmailAddress = userQuote.EmailAddress;
                    userQuoteVm.GeneratedQuote = userQuote.GeneratedQuote;
                    userQuoteVms.Add(userQuoteVm);
                }
            }
            return View(userQuoteVms);
        }
    }
}