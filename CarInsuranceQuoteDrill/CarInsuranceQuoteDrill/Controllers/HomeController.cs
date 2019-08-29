using CarInsuranceQuoteDrill.Models; //NOTE: This is needed for me to instatiate a 'CarInsuranceQuoteDrillEntities' object.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsuranceQuoteDrill.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Quote(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear,
                                  string carMake, string carModel, string dui, int speedingTicketNum, string fullCoverageOrLiability)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (CarInsuranceQuoteDrillEntities db = new CarInsuranceQuoteDrillEntities())
                {
                    UserQuote userQuote = new UserQuote();

                    userQuote.FirstName = firstName;
                    userQuote.LastNAme = lastName;
                    userQuote.EmailAddress = emailAddress;
                    userQuote.DateOfBirth = dateOfBirth;
                    userQuote.CarMake = carMake;
                    userQuote.CarYear = carYear;
                    userQuote.CarModel = carModel;
                    userQuote.DUI = dui;
                    userQuote.SpeedingTicketNum = speedingTicketNum;
                    userQuote.FullCoverageOrLiability = fullCoverageOrLiability;

                    db.UserQuotes.Add(userQuote);

                    db.SaveChanges();
                }
            }
            return View();
        }

    }
}