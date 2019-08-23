using StudentManagementSystem.Models; //NOTE: this is needed to access the 'Student' class
using System;
using System.Collections.Generic;
using System.Data;  //NOTE: This is neede for 'SqlDbType' objects. 
using System.Data.SqlClient; //NOTE: This is needed for the 'SqlConnection', 'SqlCommand' objects
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private string _connectionString = @"Data Source=DESKTOP-MEVIRUK\SQLEXPRESS;Initial Catalog=School;Integrated Security=True;
                                            Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                            MultiSubnetFailover=False";
        // GET: Student
        public ActionResult Index()
        {
            //NOTE: Here we are copying each record from the databse and assigning their values to their corresponding objects.

            //NOTE: Here we are creating a query string so we can use later to apply it on a databse.
            string queryString = "Select * From Students";

            //NOTE: Here we are instantiating a 'students' object where we will store each student record (objects) into.
            List<Student> students = new List<Student>();

            //NOTE: Here we are using instantiating a 'connection' object created from the 'SqlConnection' class where
            //      we get to pass in the '_connectionString' access data from the 'School' databse that we created.
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //NOTE: Here we are instantiating a 'command' object from the 'SqlCommand' class in order to query the database
                //      by passing in 'queryString' & 'connection'.
                SqlCommand command = new SqlCommand(queryString, connection);

                //NOTE: Here we are opening the connection so that we can (temporarily) read and use the data in the databse
                //      until we close the connection.
                connection.Open();

                //NOTE: Here we are taking our 'command object', calling the 'ExecuteReader' method which returns a 
                //      'SqlDataReader' object which we called, 'reader'.
                SqlDataReader reader = command.ExecuteReader();

                //NOTE: Here we are iterating through the databse by calling the 'Read' method on 'reader' which returns a 
                //      boolean, 'true', as long as it is able to advance the 'SqlDataReader' to the next record in the database.
                //      once the next record is null, it will return 'false'.
                while (reader.Read())
                {
                    //NOTE: Here we created a new 'student' object, getting each field data from the record, then converting 
                    //      and assigning each value and storing it in the corresponding property of the object and then storing
                    //      that object into the list of students, 'students' that we instantiated earlier.
                    Student student = new Student();
                    student.Id = Convert.ToInt32(reader["Id"]);
                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                    students.Add(student);
                }
                //NOTE: Immediately after reading and using the data from the databse we are closing the connection to prevent 
                //      future inconveniences in security and hard drive space. 
                connection.Close();
            }
            //NOTE: Here we are returning the 'students' list object to views.
            return View(students);
        }

        //NOTE: Here we added a new method called Add.
        //NOTE: This method gets called on a 'GET' request in route '/student/add' which
        //      will render 'Add.cshtml'
        public ActionResult Add()
        {
            return View();
        }


        //NOTE: This method gets called on a 'POST' request in route '/Student/Add'.
        //NOTE: Represents an attribute that is used to restrict an action method so that the method 
        //      handles only HTTP POST requests.
        [HttpPost]
        public ActionResult Add(Student student)
        {
            //NOTE: This method adds creates students objects, and inserts those students into 
            //      our 'Students' table in the 'School' database.


            //NOTE: Here we are using 'Parameterized Queries'. We want to insert values of parameters 
            //      '@FirstName' and '@LastName' into fields(columns), 'FirstName' and 'LastName'. in our
            //      'student' table in the school  database. 
            string queryString = "Insert into Students (FirstName, LastName) Values (@FirstName, @LastName)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                //NOTE: Here we are validating the parameters '@FirstName' and '@LastName' making sure their data type is 
                //      correct to protect from Sql injections.
                command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                command.Parameters.Add("@LastName", SqlDbType.VarChar);

                //NOTE: Here we are assigning the student property values in 'FirstName' & 'LastName' to the parameters.
                command.Parameters["@FirstName"].Value = student.FirstName;
                command.Parameters["@LastName"].Value = student.LastName;

                connection.Open();

                //Executes a Transact-SQL statement against the connection and returns an int (the number of rows affected).
                command.ExecuteNonQuery();

                connection.Close();
            }
            return RedirectToAction("Index");
            /*NOTE: Notice that we specified that it is a POST method in square brackets above the Add method. MVC allows you to “decorate” 
                    methods this way to provide extra information about the method itself (the term used for something like this is “metadata” 
                    which simply means “data about the data”).
              NOTE: Another thing to notice is that we aren’t returning a View here. We could have returned the Add View, but it wouldn’t 
                    have made much sense. The User would have clicked “Add User” and would have had the page refresh with no indication that 
                    the Add succeeded. MVC provides us a way of redirecting to other Controller methods. In this case, we’re redirecting 
                    back to the Index method, which collects the list of Users and hands it to the View.*/
        }

        //NOTE: On the index.cshtml, we are have a link to each student each having their own Id number
        //      (Each student will have a link like this href="/student/details/@student.Id" with their
        //      respective Ids). Once the user clicks on a student, the 'Details' method below gets called
        //      passing in the 'ID' int from the URL needed in order to run the query of finding the record
        //      in the 'Students' table
        public ActionResult Details(int id)
        {
            string queryString = "Select * From Students where id = @id";
            Student student = new Student();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                //NOTE: Here  we are making sure the '@id' parameter only takes Sql int data type.
                command.Parameters.Add("@id", SqlDbType.Int);

                //NOTE: Here we are assigning the 'id' int that was passed in, and assigning it to the 
                //      parameter, in order to run the query and read the data
                command.Parameters["@id"].Value = id;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //NOTE: This will read through each records found in the 'Student' table after running the 
                //      query string (it will only find One since each record has distinct Ids), and assign 
                //      each field(column) value from the database to each student object property
                //      respectively. 
                while (reader.Read())
                {
                    student.Id = Convert.ToInt32(reader["Id"]);
                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                }
                connection.Close();
            }
            //NOTE: Now we are passing that 'student' object to Views, where 'Details.cshtml' under the
            //      'Student' folder will be able use the object to for the user can see id, first name,
            //      and last name.
            return View(student);
        }

        //NOTE: Once the user is viewing the details (route, 'Student/Details') on a student in the 'Details.cshtml', 
        //      there will be a link, 'Edit' which the user will be able to click on which takes them 
        //      to href="../edit/@Model.Id". that href will end up painting route, 'Student/edit/@Model.Id'. 
        //      That url route produces a GET request and it calls the method below passing in that Model.Id, which
        //      is the current student we're viewing.
        public ActionResult Edit(int id)
        {
            string queryString = "Select * From Students where id = @id";
            Student student = new Student();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@id", SqlDbType.Int);

                command.Parameters["@id"].Value = id;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    student.Id = Convert.ToInt32(reader["Id"]);
                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                }
                connection.Close();
            }

            //NOTE: By using the same logic as the 'Details' method we are able to render 'Edit.cshtml'
            //      which will include the students details but in an editable form that can get submitted,
            //      using a 'POST' method so that those edits can be translated and updated in the database
            //      after calling the othe 'Edit' method that will go below this one.
            return View(student);
        }

        //NOTE: After a form (that uses the post method) is submitted and is in route, 'Student/edit/@Model.Id',
        //      it will call the method, 'Edit' below.
        //NOTE: This method uses the same logic as the 'Add' method (on a POST request), except it is passing in 
        //      an object that already has an Id (it is not auto-generated), and we are simply taking in that new
        //      student object creted by the form, and passing/updating the values to the corresponding fields(columns)
        //      in 'Students' table in the 'School' databse.
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            
            string queryString = "Update Students set FirstName = @FirstName, LastName = @LastName where id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                command.Parameters.Add("@LastName", SqlDbType.VarChar);

                command.Parameters["@id"].Value = student.Id;
                command.Parameters["@FirstName"].Value = student.FirstName;
                command.Parameters["@LastName"].Value = student.LastName;

                connection.Open();
                command.ExecuteNonQuery();

                connection.Close();
            }
            //NOTE: After updating the Database, we just return to route, '/Student' (which will reflect the changes).
            return RedirectToAction("Index");
        }
    }
}