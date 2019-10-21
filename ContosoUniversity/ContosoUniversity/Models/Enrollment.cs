using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        /*NOTE: The 'EnrollmentID' property will be the primary key; this entity uses the classname ID pattern instead 
         *      of ID by itself as you saw in the 'Student' entity. Ordinarily you would choose one pattern and use it throughout 
         *      your data model. Here, the variation illustrates that you can use either pattern. In a later tutorial, 
         *      you'll see how using ID without classname makes it easier to implement inheritance in the data model.*/
        public int EnrollmentID { get; set; }

        //NOTE: The 'CourseID' property is a foreign key, and the corresponding navigation property is 'Course'. An 'Enrollment' 
        //      entity is associated with one Course entity.
        public int CourseID { get; set; }

        /*NOTE: The 'StudentID' property is a foreign key, and the corresponding navigation property is 'Student'. An 'Enrollment' 
         *      entity is associated with one 'Student' entity, so the property can only hold a single Student entity 
         *      (unlike the Student.Enrollments navigation property [in the 'Student' class] you saw earlier, which can hold 
         *      multiple Enrollment entities).*/
        public int StudentID { get; set; }

        //
        [DisplayFormat(NullDisplayText = "No grade")]

        /*NOTE: The Grade property is an enum. The question mark after the Grade type declaration indicates that the Grade property 
         *      is nullable. A grade that's null is different from a zero grade — null means a grade isn't known or hasn't been 
         *      assigned yet.*/
        public Grade? Grade { get; set; }

        /*NOTE: This enrollment object has properties 'Course' and 'Student' (that will essentially go along with their respective
         *      ID properties). They are made 'virtual' because each is a naviagation property. Many enrollments can be related to a 
         *      'Course' & a 'Student' */
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

        /*NOTE: 'Entity Framework' interprets a property as a foreign key property if it's named
         *      <navigation property name><primary key property name> (for example, StudentID for the Student navigation property 
         *      since the Student entity's primary key is ID). Foreign key properties can also be named the same simply
         *      <primary key property name> (for example, CourseID since the Course entity's primary key is CourseID).*/
    }
}