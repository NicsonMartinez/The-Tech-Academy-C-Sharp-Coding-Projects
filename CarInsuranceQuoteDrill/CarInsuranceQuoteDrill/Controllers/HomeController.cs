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
        public ActionResult Index(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear,
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
                    user.LastName = lastName;
                    user.EmailAddress = emailAddress;
                    user.DateOfBirth = dateOfBirth;
                    user.CarMake = carMake;
                    user.CarYear = carYear;
                    user.CarModel = carModel;
                    user.DUI = dui;
                    user.SpeedingTicketNum = speedingTicketNum;
                    user.FullCoverageOrLiability = fullCoverageOrLiability;

                    //NOTE: Here I am using a 'QuoteGenerator' class that I created which has a method called, 'GenerateQuote'
                    //      which passes in a parameter 'UserQuote' object, calculates and returnd a quote value represented  
                    //      as 'decimal' type, and gets stored into 'user' property, 'GeneratedQuote'.
                    user.GeneratedQuote = QuoteGenerator.GenerateQuote(user);
                    user.DateAndTime = DateTime.Now;
                                                         
                    db.UserQuotes.Add(user);

                    db.SaveChanges();

                    //NOTE: Here I am using a 'ViewBag' to store the quote generated so I can use imediately use it in my view,
                    //      to be able to show the user the quote.
                    ViewBag.Quote = user.GeneratedQuote;        
                }
            }
            return View();
        }
    }
}