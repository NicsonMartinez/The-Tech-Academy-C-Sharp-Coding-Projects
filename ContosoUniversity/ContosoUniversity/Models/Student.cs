using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        /*NOTE: The Required attribute makes the name properties required fields. The 'Required' attribute is not needed for value 
         *      types such as DateTime, int, double, and float. Value types cannot be assigned a null value, so they are inherently 
         *      treated as required fields.
         *NOTE: The 'Required' attribute must be used with 'MinimumLength' for the 'MinimumLength' (below) to be enforced.*/
        [Required]

        /*NOTE: You can also specify data validation rules and validation error messages using attributes. The 'StringLength' 
         *      attribute sets the maximum length in the database and provides client side and server side validation for 
         *      ASP.NET MVC. You can also specify the minimum string length in this attribute, but the minimum value has no 
         *      impact on the database schema.*/
        [StringLength(50, MinimumLength = 2)]

        /*NOTE: The Display attribute specifies that the caption for the text boxes should be "First Name", "Last Name", 
         *      "Full Name", and "Enrollment Date" instead of the property name in each instance (which has no space dividing the words).*/
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        /*NOTE: The StringLength attribute won't prevent a user from entering white space for a name. You can use the 
         *      'RegularExpression' attribute to apply restrictions to the input. For example the following code requires 
         *      the first character to be upper case and the remaining characters to be alphabetical:
         *
         *      '[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]'      */
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]

        /*NOTE: By using 'System.ComponentModel.DataAnnotations.Schema', we are able to use the annotation below to change 
                the column name in the database from 'FisrtMidName' to 'FirstName' while still keeping the model and the 
                database mapped.*/
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }


        /*NOTE: The 'DataType' attribute is used to specify a data type that is more specific than the database intrinsic type. 
         *      In this case we only want to keep track of the date, not the date and time. The 'DataType' Enumeration provides 
         *      for many data types, such as Date, Time, PhoneNumber, Currency, EmailAddress and more. The DataType attribute 
         *      can also enable the application to automatically provide type-specific features. For example, a mailto: link 
         *      can be created for DataType.EmailAddress, and a date selector can be provided for DataType.Date in browsers
         *      that support HTML5. The DataType attributes emits HTML 5 data- (pronounced data dash) attributes that HTML 5 
         *      browsers can understand. The DataType attributes do not provide any validation.
         *NOTE: 'DataType.Date' does not specify the format of the date that is displayed. By default, the data field is displayed 
         *      according to the default formats based on the server's CultureInfo.*/
        [DataType(DataType.Date)]

        /*NOTE: The DisplayFormat attribute is used to explicitly specify the date format.
         *NOTE: You can use the 'DisplayFormat' attribute by itself, but it's generally a good idea to use the 'DataType' attribute also. 
         *      The DataType attribute conveys the semantics of the data as opposed to how to render it on a screen, and provides 
         *      specific benefits that you don't get with 'DisplayFormat'.*/
        /*NOTE: If you use the 'DataType' attribute with a date field, you have to specify the DisplayFormat attribute also in order 
         *      to ensure that the field renders correctly in Chrome browsers. */
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        /*NOTE: FullName is a calculated property that returns a value that's created by concatenating two other properties. Therefore it 
         *      has only a get accessor, and no FullName column will be generated in the database.*/
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

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