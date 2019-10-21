using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Instructor
    {
        //NOTE: You can put multiple attributes on one line, so you could also write the instructor class as follows.

        public int ID { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        //[StringLength(50)]
        [Display(Name = "Last Name"), StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        [Column("FirstName"), Display(Name = "First Name"), StringLength(50, MinimumLength = 1)]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date), Display(Name = "Hire Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        /*NOTE: The 'Courses' and 'OfficeAssignment' properties are navigation properties. As was explained earlier, they are 
         *      typically defined as virtual so that they can take advantage of an Entity Framework feature called 'lazy loading'.
         *      In addition, if a navigation property can hold multiple entities, its type must implement the ICollection<T> Interface. 
         *      For example IList<T> qualifies but not IEnumerable<T> because 'IEnumerable<T>' doesn't implement Add.*/
        //NOTE: An instructor can teach any number of courses, so 'Courses' is defined as a collection of 'Course' entities.
        public virtual ICollection<Course> Courses { get; set; }

        /*NOTE: Our business rules state an instructor can only have at most one office, so 'OfficeAssignment' is defined as a single
         *      'OfficeAssignment' entity (which may be 'null' if no office is assigned).*/
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}