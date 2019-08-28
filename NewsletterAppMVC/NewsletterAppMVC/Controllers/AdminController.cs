using NewsletterAppMVC.Models; //NOTE: now that we moved 'Model1.edmx' to the 'Models' folder, this is needed for 'NewsletterEntities' objects.
using NewsletterAppMVC.ViewModels; //NOTE: Needed for 'SignupVm' objects.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {

                //NOTE: Below our, 'db' object has a property called 'SignUps' is a set of all of the records
                //      in our database stored as objects:
                //      'var signups = db.SignUps;' 
                //NOTE: We don't want to list all records anymore. Now we want to be more specific see below.


                //NOTE: This line looks at the database set, 'SignUps' of 'SignUp' objects and only return those
                //      objects where  property 'Removed' is equal to null, then storing it to 'var signups'.
                //      This is done so we only render the records/objects that are still subscribed:
                //      'var signups = db.SignUps.Where(x => x.Removed == null).ToList();'

                //NOTE: on the line above we are using a lambda expression. And this is one of the advantages of 
                //      using 'EntityFramework'. We can write query like logic on objects that are conveniently 
                //      mapped to a database.
                //NOTE: Another way to accomplish the same resut as the lambda expression above is by using
                //      Linq - Language Integrated Query:
                var signups = (from c in db.SignUps
                               where c.Removed == null
                               select c).ToList();

                //NOTE: Using 'View Model' is considered best practice in order to map eveything that needs to get
                //      mapped in the database without having to map it to objects that will be shown in views.
                //      In our example, SSN is a perfect example of what is important to map, and what is important
                //      to not show that information to any user.
                var signupVms = new List<SignupVm>();
                foreach (var signup in signups)
                {
                    var signupVm = new SignupVm();
                    signupVm.Id = signup.Id;
                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;
                    signupVms.Add(signupVm);
                }
                return View(signupVms);
            }
        }

        public ActionResult Unsubscribe(int Id)
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                //NOTE: The 'Find' method takes in a primary key value and looks for the record 
                //      (now an object) that matches that key (property Id) and then returns the 
                //      record 'SignUp' object to 'signup'. If it doesn't find that key, it will
                //      return null.
                var signup = db.SignUps.Find(Id);
                signup.Removed = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}