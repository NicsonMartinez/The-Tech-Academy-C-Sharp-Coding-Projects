using NewsletterAppMVC.Models; //NOTE: To import our 'NewsletterSignUp' class.
using NewsletterAppMVC.ViewModels; //NOTE: To import our 'SignuVm' class.
using System;
using System.Collections.Generic;
using System.Data; //NOTE: This is for SqlDbType.
using System.Data.SqlClient; //NOTE: This is for SqlConnection.
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString =  @"Data Source=DESKTOP-MEVIRUK\SQLEXPRESS;Initial Catalog=Newsletter;Integrated Security=True;
                                                    Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                                    MultiSubnetFailover=False";
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
                string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress) VALUES
                                      (@FirstName, @LastName, @EmailAddress)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return View("Success");
            }
        }

        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, SocialSecurityNumber FROM SignUps";
            List<NewsletterSignUp> signups = new List<NewsletterSignUp>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var signup = new NewsletterSignUp();
                    signup.Id = Convert.ToInt32(reader["Id"]);
                    signup.FirstName = reader["FirstName"].ToString();
                    signup.LastName = reader["LastName"].ToString();
                    signup.EmailAddress = reader["EmailAddress"].ToString();
                    signup.SocialSecurityNumber = reader["SocialSecurityNumber"].ToString();
                    signups.Add(signup);
                }
            }

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