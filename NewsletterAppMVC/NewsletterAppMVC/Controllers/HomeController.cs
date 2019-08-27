using NewsletterAppMVC.ViewModels; //NOTE: To import our 'SignuVm' class.
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

        //private readonly string connectionString = @"Data Source=DESKTOP-MEVIRUK\SQLEXPRESS;Initial Catalog=Newsletter;Integrated Security=True;
        //                                            Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
        //                                            MultiSubnetFailover=False";
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

        public ActionResult Admin()
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                //NOTE: Here our, 'db' object has a property called 'SignUps' which represents all of the records
                //      in our database.
                var signups = db.SignUps;

                //NOTE: Using 'View Model' is considered best practice in order to map eveything that needs to get
                //      mapped in the database without having to map it to objects that will be shown in views.
                //      In our example, SSN is a perfect example of what is important to map, and what is important
                //      to not show that information to any user.
                var signupVms = new List<SignupVm>();
                foreach (var signup in signups)
                {
                    var signupVm = new SignupVm();
                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;
                    signupVms.Add(signupVm);
                }
                return View(signupVms);
            }
        }

    }
}