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
                    UserQuote user = new UserQuote();

                    user.FirstName = firstName;
                    user.LastNAme = lastName;
                    user.EmailAddress = emailAddress;
                    user.DateOfBirth = dateOfBirth;
                    user.CarMake = carMake;
                    user.CarYear = carYear;
                    user.CarModel = carModel;
                    user.DUI = dui;
                    user.SpeedingTicketNum = speedingTicketNum;
                    user.FullCoverageOrLiability = fullCoverageOrLiability;
                    user.GeneratedQuote = QuoteGenerator.GenerateQuote(user);
                                                         
                    db.UserQuotes.Add(user);

                    db.SaveChanges();
                    
                }
            }
            return View();
        }
    }
}