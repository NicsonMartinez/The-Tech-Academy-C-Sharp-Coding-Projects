using System;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
    public class Student
    {
        /*NOTE: The ID property will become the primary key column of the database table that corresponds to this class. 
         *      By default, Entity Framework interprets a property that's named ID or classname ID as the primary key.*/
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}