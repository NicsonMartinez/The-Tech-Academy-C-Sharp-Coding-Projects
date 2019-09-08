using System;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
    public class Student
    {
        /*NOTE: There's a one-to-many relationship between Student and Enrollment entities, and there's a one-to-many 
         *      relationship between Course and Enrollment entities. In other words, a student can be enrolled in any number 
         *      of courses, and a course can have any number of students enrolled in it.*/

        /*NOTE: The ID property will become the primary key column of the database table that corresponds to this class. 
         *      By default, Entity Framework interprets a property that's named ID or classname ID as the primary key.*/
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }


        /*NOTE: The ID property will become the primary key column of the database table that corresponds to this class. 
         *      By default, 'Entity Framework' interprets a property that's named ID or classname ID as the primary key. 
         * 
         *NOTE: The 'Enrollments' property is a 'navigation property'. Navigation properties hold other entities that are related 
         *      to this entity. In this case, the 'Enrollments' property of a 'Student' entity will hold all of the 'Enrollment' 
         *      entities that are related to that 'Student' entity. In other words, if a given 'Student' row in the database has 
         *      two related 'Enrollment' rows (rows that contain that student's primary key value in their 'StudentID' foreign key column), 
         *      that Student entity's Enrollments navigation property will contain those two Enrollment entities.

         *NOTE: 'Navigation properties' are typically defined as 'virtual' so that they can take advantage of certain Entity Framework 
         *      functionality such as 'lazy loading' (So that it can create views with 'CRUD'- Create, Read, Update, & Delete 'Views' while
         *      at the same time connecting that functionality with the database). */
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}