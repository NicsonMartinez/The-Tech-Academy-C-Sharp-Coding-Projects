using ContosoUniversity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContosoUniversity.DAL
{
    //NOTE: The 'DAL' folder this file is in is called, 'Data Access Layer'.

    public class SchoolContext : DbContext
    {
        /*NOTE: The name of the connection string (which you'll add to the Web.config file later) is passed in to the constructor.
         *      'SchoolContext.SchoolContext' ( or 'public SchoolContext()') inherits 'DbContext.DbContext(String nameOrConnectionString)'
         *      (or 'base("SchoolContext")')
         *NOTE: If you don't specify a connection string or the name of one explicitly, 'Entity Framework' assumes that the connection 
         *      string name is the same as the class name. The default connection string name in this example would then be 'SchoolContext', 
         *      the same as what you're specifying explicitly.*/
        public SchoolContext() : base("SchoolContext")
        {
        }

        /*NOTE: This class, 'SchoolContext' derives (inherits) from the 'System.Data.Entity.DbContext' class. This 
         *      code creates a DbSet property for each entity set. In Entity Framework terminology, an entity set typically 
         *      corresponds to a database table, and an entity corresponds to a row in the table.
         */
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        /*NOTE: You can omit the 'DbSet<Enrollment>' and 'DbSet<Course>' statements and it would work the same. 
         * 'Entity Framework' would include them implicitly because the 'Student' entity references the 'Enrollment' 
         * entity and the 'Enrollment' entity references the 'Course' entity.*/

        //NOTE: Here we are overriding a method from the class we're inheriting from, 'DbContext'.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*NOTE: The 'modelBuilder.Conventions.Remove' statement in this 'OnModelCreating' method prevents table names from being pluralized. 
             *      If you didn't do this, the generated tables in the database would be named 'Students', 'Courses', and 'Enrollments'. Instead, the 
             *      table names will be 'Student', 'Course', and 'Enrollment'. Developers disagree about whether table names should be pluralized 
             *      or not. This tutorial uses the singular form, but the important point is that you can select whichever form you prefer by 
             *      including or omitting this line of code.*/
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}