using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]

        /*NOTE: Earlier you used the Column attribute to change column name mapping. In the code for the 'Department' entity, 
         *      the 'Column' attribute is being used to change SQL data type mapping so that the column will be defined using 
         *      the SQL Server money type in the database.*/
        /*NOTE: Column mapping is generally not required, because the Entity Framework usually chooses the appropriate 
         *      SQL Server data type based on the CLR type that you define for the property. The CLR 'decimal' type maps 
         *      to a SQL Server 'decimal' type. But in this case you know that the column will be holding currency amounts, 
         *      and the money data type is more appropriate for that. For more information about CLR data types and how they 
         *      match to SQL Server data types, see 'SqlClient for Entity FrameworkTypes':
         *      https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/sqlclient-for-ef-types?redirectedfrom=MSDN */
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /*NOTE: A department may or may not have an administrator, and an administrator is always an instructor. Therefore the 
         *      'InstructorID' property is included as the foreign key to the 'Instructor' entity, and a question mark is added 
         *      after the 'int' type designation to mark the property as nullable. The navigation property is named 'Administrator' 
         *      but holds an 'Instructor' entity.*/
        public int? InstructorID { get; set; }

        /*NOTE: By convention, the Entity Framework enables cascade delete for non-nullable foreign keys and for many-to-many 
         *      relationships. This can result in circular cascade delete rules, which will cause an exception when you try 
         *      to add a migration. For example, if you didn't define the 'Department.InstructorID' property as nullable, 
         *      you'd get the following exception message: "The referential relationship will result in a cyclical reference 
         *      that's not allowed." If your business rules required 'InstructorID' property to be non-nullable, you would have 
         *      to use the following fluent API statement to disable cascade delete on the relationship:
         *
         *      modelBuilder.Entity().HasRequired(d => d.Administrator).WithMany().WillCascadeOnDelete(false);
         */


        public virtual Instructor Administrator { get; set; }

        //NOTE: A department may have many courses, so there's a Courses navigation property.
        public virtual ICollection<Course> Courses { get; set; }
    }
}