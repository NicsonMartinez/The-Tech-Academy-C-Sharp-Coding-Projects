using NewsletterAppMVC.Models; //NOTE: now that we moved 'Model1.edmx' to the 'Models' folder, this is needed for 'NewsletterEntities' objects.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        //NOTE: We no longer need the 'connectionString' because entityFramework saved it in a 'Web.config' file.

        public ActionResult Index()
        {
            return View();
        }

        //NOTE: Below MVC is able to map the form's name attributes we just created in NewsletterAppMVC\Views\Home\index.cshtml,
        //      'FirstName', 'LastName', and 'EmailAddress' by matching it with the parameter names. In MVC this is called 'Model Binding'.
        [HttpPost]
        public ActionResult SignUp(string firstName, string lastName, string emailAddress)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                //NOTE: Tilda('~') is the root of this project.
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (NewsletterEntities db = new NewsletterEntities())
                {

                    //NOTE: Here are instantiating an object from the 'SignUp' class that 'EntityFramwork' created for us.  
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;

                    //NOTE: Here we taking that object and adding its property values to create a database record.
                    db.SignUps.Add(signup);

                    //NOTE: After adding the objects to the databse, saves changes needs to get called on the
                    //      'db' object or else it won't save those changes to the database.
                    db.SaveChanges();
                }
                return View("Success");
            }
        }

    }
}