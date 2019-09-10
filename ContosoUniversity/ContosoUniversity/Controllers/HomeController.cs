using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext();
        public ActionResult Index()
        {
            return View();
        }


        //NOTE: The LINQ statement groups the student entities by enrollment date, calculates the number of entities in each group, 
        //      and stores the results in a collection of 'EnrollmentDateGroup' view model objects.
        public ActionResult About()
        {
            IQueryable<EnrollmentDateGroup> data = from student in db.Students
                        group student by student.EnrollmentDate into dateGroup
                        select new EnrollmentDateGroup()
                        {
                            EnrollmentDate = dateGroup.Key,
                            StudentCount = dateGroup.Count()
                        };
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /*NOTE: To close database connections and free up the resources they hold as soon as possible, dispose the context 
 *      instance when you are done with it. That is why the scaffolded code provides a Dispose method at the end 
 *      of this StudentController class in StudentController.cs, as shown in the code below:*/
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}